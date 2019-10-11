using UnityEngine;
using System.Collections;

public class TankEnemy : Tank, IDamageable
{
    public float HitPoints { get; set; } = 50;

    private WaveManager waveManager;

    [SerializeField]
    private GameObject shootParticle = default;
    [SerializeField]
    private GameObject bossProjectile = default;
    [SerializeField]
    private GameObject destroyedTankPrefab = default;
    [SerializeField]
    private Transform turretBody = default;
    [SerializeField]
    private Transform turretBarrelHole = default;
    [SerializeField]
    private Transform tankTurretBarrel = default;
    [SerializeField]
    private Collider barrelCollider = default;

    private Transform player;
    private AudioSource shootSound;

    private Vector3 targetPoint;
    private Vector3 rayForward;
    private RaycastHit rayHit;

    [SerializeField]
    private int maxDistanceFromTarget = 1;
    [SerializeField]
    private int randomDistanceMin = 30;
    [SerializeField]
    private int randomDistanceMax = 60;
    [SerializeField]
    private float stopWaitTimeMax = 2.5f;
    [SerializeField]
    private float rotationSpeed = 80;
    [SerializeField]
    private float turretRotationSpeed = 120;
    [SerializeField]
    private float moveSpeed = 5.5f;
    [SerializeField]
    private float playerDistanceThreshold = 75;
    [SerializeField]
    private float playerDistanceCheckTime = 0.5f;
    [SerializeField]
    private float projectileSpeed = 55;
    [SerializeField]
    private float shootRate = 0.5f;
    [SerializeField]
    private float attackRayCheckTime = 3;

    private float targetDistance;
    private float playerDistance;
    private float playerDistanceCheckTimer;
    private float shootTimer;

    [SerializeField]
    private bool isBoss = default;
    private bool rotateDefault;
    private bool executedWait;

    private Vector3 tankDirection;
    private Quaternion tankLookRotation;

    private Vector3 tankTurretDirection;
    private Quaternion tankTurretLookRotation;

    protected override void Initialize()
    {
        player = GameObject.Find("PRE_Tank_Player").GetComponent<Transform>();
        waveManager = GameObject.Find("WaveManager").GetComponent<WaveManager>();

        shootSound = GetComponentInChildren<AudioSource>();

        if (isBoss)
        {
            HitPoints *= 10f;
        }
    }

    protected override void StartState()
    {
        GetNewDestination();
        currentState = TankStates.Patrol;
    }

    private enum TankStates
    {
        Stop,
        Patrol,
        Attack
    }

    private TankStates currentState;

    protected override void UpdateState()
    {
        #region LogState
        #if UNITY_EDITOR
        // Log state changes in the unity editor.
        LogState();
#endif
        #endregion
        // Draw a ray from the gun barrel.
        DrawBarrelRay();
        // Destroy enemy if hitpoints below zero.
        CheckDestroySelf();
        // Keep track of player's distance every X seconds.
        PlayerDistanceCheck();

        switch (currentState)
        {
            case TankStates.Stop:
                Stop();
                break;
            case TankStates.Patrol:
                Patrol();
                break;
            case TankStates.Attack:
                Attack();
                break;
            default:
                break;
        }
    }
    
    private void DrawBarrelRay()
    {
        rayForward = turretBarrelHole.TransformDirection(Vector3.up);
        Physics.Raycast(turretBarrelHole.position, rayForward * playerDistanceThreshold, out rayHit);
    }

    private void PlayerDistanceCheck()
    {
        playerDistanceCheckTimer += Time.deltaTime;
        if (player != null && playerDistanceCheckTimer >= playerDistanceCheckTime)
        {
            playerDistance = Vector3.Distance(transform.position, player.position);
        }
    }

    private void Stop()
    {
        if (!executedWait)
        {
            executedWait = true;
            StartCoroutine(Wait());
        }
    }

    private IEnumerator Wait()
    {
        // Wait for a random amount of seconds before getting a new destination.
        yield return new WaitForSeconds(Random.Range(0, stopWaitTimeMax));
        executedWait = false;
        GetNewDestination();
        currentState = TankStates.Patrol;
    }

    private void Patrol()
    {
        // Check if player is within distance.
        if (playerDistance <= playerDistanceThreshold)
        {
            currentState = TankStates.Attack;
        }
        // Rotate the turret to default position when player gets out of range.
        if (rotateDefault)
        {
            turretBody.rotation = Quaternion.RotateTowards(turretBody.rotation, transform.rotation, turretRotationSpeed * Time.deltaTime);
            if (turretBody.rotation == transform.rotation)
            {
                rotateDefault = false;
            }
        }
        // Move towards target.
        targetDistance = Vector3.Distance(transform.position, targetPoint);
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, moveSpeed * Time.deltaTime);
        // Stop when reached the target destination.
        if (targetDistance <= maxDistanceFromTarget)
        {
            currentState = TankStates.Stop;
        }
        // Rotate towards target.
        tankDirection = (targetPoint - transform.position).normalized;
        tankLookRotation = Quaternion.LookRotation(tankDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, tankLookRotation, rotationSpeed * Time.deltaTime);
    }
    // Change direction when colliding with environment.
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.GetType() != typeof(TerrainCollider))
        {
            GetNewDestination();
        }
    }

    private void GetNewDestination()
    {
        // Randomise the targetPoint vector.
        float random = Random.Range(0, 3);
        if (random == 0)
        {
            targetPoint.x = transform.position.x + Random.Range(randomDistanceMin, randomDistanceMax);
            targetPoint.z = transform.position.z + Random.Range(randomDistanceMin, randomDistanceMax);
        }
        else if (random == 1)
        {
            targetPoint.x = transform.position.x - Random.Range(randomDistanceMin, randomDistanceMax);
            targetPoint.z = transform.position.z - Random.Range(randomDistanceMin, randomDistanceMax);
        }
        else if (random == 2)
        {
            targetPoint.x = transform.position.x + Random.Range(randomDistanceMin, randomDistanceMax);
            targetPoint.z = transform.position.z - Random.Range(randomDistanceMin, randomDistanceMax);
        }
        else
        {
            targetPoint.x = transform.position.x - Random.Range(randomDistanceMin, randomDistanceMax);
            targetPoint.z = transform.position.z + Random.Range(randomDistanceMin, randomDistanceMax);
        }
        targetPoint.y = transform.position.y;

        #if UNITY_EDITOR
        Debug.Log($"{gameObject.name} is retrieving a new destination X {targetPoint.x}, Z {targetPoint.z}");
        #endif
    }

    private void Attack()
    {
        // Switch to patrol for a moment if the barrel ray doesn't hit player.
        StartCoroutine(WaitRayCheck());
        // Check if player has left the check distance.
        if (playerDistance >= playerDistanceThreshold)
        {
            rotateDefault = true;
            currentState = TankStates.Patrol;
        }
        // Rotate turret towards target.
        if (player != null)
        {
            tankTurretDirection = (player.position - transform.position).normalized;
            tankTurretLookRotation = Quaternion.LookRotation(tankTurretDirection);
            turretBody.rotation = Quaternion.RotateTowards(turretBody.rotation, tankTurretLookRotation, turretRotationSpeed * Time.deltaTime);
        }

        Shoot();
    }

    private IEnumerator WaitRayCheck()
    {
        yield return new WaitForSeconds(attackRayCheckTime);
        if (rayHit.collider != null && !rayHit.collider.CompareTag("Player"))
        {
            currentState = TankStates.Patrol;
        }
    }

    private void Shoot()
    {
        shootTimer += Time.deltaTime;

        GameObject projectile;
        if (rayHit.collider != null && rayHit.collider.CompareTag("Player") && shootTimer >= shootRate)
        {
            shootTimer = 0;
            // Get ammo from the ammo pool and fire it from the barrel.
            if (isBoss)
            {
                projectile = Instantiate(bossProjectile);
            }
            else
            {
                projectile = PoolManager.Instance.PopAmmo();
            }
            Physics.IgnoreCollision(barrelCollider, projectile.GetComponent<Collider>());
            projectile.transform.position = turretBarrelHole.position;
            projectile.transform.rotation = tankTurretBarrel.rotation;

            Vector3 projectileVelocity;
            if (isBoss)
            {
                 projectileVelocity = turretBody.forward + tankTurretBarrel.up;
            }
            else
            {
                 projectileVelocity = turretBody.forward + tankTurretBarrel.up + new Vector3(0, 0.071f, 0);
            }
            projectile.GetComponent<Rigidbody>().velocity = projectileVelocity * projectileSpeed;

            GameObject fireParticle = Instantiate(shootParticle);
            fireParticle.transform.position = turretBarrelHole.position;
            Destroy(fireParticle, 3);

            shootSound.Play();

            projectile.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        waveManager.AliveEnemies.Remove(gameObject);
    }

    #region Interface Methods
    public void TakeDamage(float damage)
    {
        HitPoints -= damage;
    }

    public void CheckDestroySelf()
    {
        if (HitPoints <= 0)
        {
            if (!isBoss)
            {
                GameObject destroyedTank = Instantiate(destroyedTankPrefab);
                destroyedTank.transform.position = transform.position + new Vector3(0, -0.5f, 0);
                destroyedTank.transform.rotation = transform.rotation * Quaternion.Euler(13, 23, -4);
            }

            Destroy(gameObject);
        }
    }
    #endregion

    #region State Debug Logs
    #if UNITY_EDITOR
    private void LogState()
    {
        switch (currentState)
        {
            case TankStates.Stop:
                Debug.Log($"{gameObject.name} stopped.");
                break;
            case TankStates.Patrol:
                Debug.Log($"{gameObject.name} started patrolling.");
                break;
            case TankStates.Attack:
                Debug.Log($"{gameObject.name} started attacking.");
                break;
            default:
                break;
        }
    }
    #endif
    #endregion

    #region Gizmos
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, targetPoint);
        Gizmos.DrawWireSphere(transform.position, playerDistanceThreshold);
        Gizmos.DrawRay(turretBarrelHole.position, turretBarrelHole.up * playerDistanceThreshold);
    }
    #endif
    #endregion
}