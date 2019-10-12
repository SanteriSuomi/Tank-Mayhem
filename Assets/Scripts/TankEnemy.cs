using UnityEngine;
using System.Collections;

public class TankEnemy : Tank, IDamageable
{
    public float HitPoints { get; set; } = 50;

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
    private WaveManager waveManager;

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
    private float stopWaitTimeMin = 1;
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
    [SerializeField]
    private float bossHealth = 10;

    private float targetDistance;
    private float playerDistance;
    private float playerDistanceCheckTimer;
    private float shootTimer;

    [SerializeField]
    private bool isBoss = false;
    private bool rotateToDefault;
    private bool executedWait;
    private bool executedRayCheck;

    private Vector3 tankDirection;
    private Quaternion tankLookRotation;
    private Vector3 tankTurretDirection;
    private Quaternion tankTurretLookRotation;

    protected override void Initialize()
    {
        player = GameObject.Find("PRE_Tank_Player").GetComponent<Transform>();
        waveManager = GameObject.Find("WaveManager").GetComponent<WaveManager>();
        shootSound = GetComponentInChildren<AudioSource>();
        // If this instance is the boss, multiply health.
        if (isBoss)
        {
            HitPoints *= bossHealth;
        }
    }

    protected override void StartState()
    {
        // Start the AI by getting a new destination and settings initial state to patrol.
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

        DrawBarrelRay();
        CheckDestroySelf();
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
                #if UNITY_EDITOR
                Debug.Log($"{name} currentState probably shouldn't be default");
                #endif
                break;
        }
    }
    
    private void DrawBarrelRay()
    {
        // Raycast from barrel for future use.
        rayForward = turretBarrelHole.TransformDirection(Vector3.up);
        Physics.Raycast(turretBarrelHole.position, rayForward * playerDistanceThreshold, out rayHit);
    }

    private void PlayerDistanceCheck()
    {
        // Constantly check the player distance including a timer.
        playerDistanceCheckTimer += Time.deltaTime;
        if (player != null && playerDistanceCheckTimer >= playerDistanceCheckTime)
        {
            playerDistance = Vector3.Distance(transform.position, player.position);
        }
    }
    // Patrol state.
    private void Stop()
    {
        // Ensure coroutine isn't executed constantly.
        if (!executedWait)
        {
            executedWait = true;
            StartCoroutine(Wait());
        }
    }

    private IEnumerator Wait()
    {
        // Wait for a random amount of seconds before getting a new destination.
        yield return new WaitForSeconds(Random.Range(stopWaitTimeMin, stopWaitTimeMax));
        executedWait = false;
        GetNewDestination();
        currentState = TankStates.Patrol;
    }

    private void Patrol()
    {
        CheckPlayerDistance();
        RotateToDefault();
        MoveToTarget();
        StopAtDestination();
        RotateTowardsTarget();
    }

    private void CheckPlayerDistance()
    {
        // First check if player is within the threshold distance.
        if (playerDistance <= playerDistanceThreshold)
        {
            currentState = TankStates.Attack;
        }
    }

    private void RotateToDefault()
    {
        // Rotate the turret to default position when player gets out of range.
        if (rotateToDefault)
        {
            turretBody.rotation = Quaternion.RotateTowards(turretBody.rotation, transform.rotation, turretRotationSpeed * Time.deltaTime);
            // Stop the rotation when it's the same as the body of the object.
            if (turretBody.rotation == transform.rotation)
            {
                rotateToDefault = false;
            }
        }
    }

    private void MoveToTarget()
    {
        // Move towards target.
        targetDistance = Vector3.Distance(transform.position, targetPoint);
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, moveSpeed * Time.deltaTime);
    }

    private void StopAtDestination()
    {
        // Stop when reached the target destination.
        if (targetDistance <= maxDistanceFromTarget)
        {
            currentState = TankStates.Stop;
        }
    }

    private void RotateTowardsTarget()
    {
        // Rotate towards target.
        tankDirection = (targetPoint - transform.position).normalized;
        tankLookRotation = Quaternion.LookRotation(tankDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, tankLookRotation, rotationSpeed * Time.deltaTime);
    }

    private void OnCollisionStay(Collision collision)
    {
        // Get a new destination when colliding with environment colliders.
        if (collision.collider.GetType() != typeof(TerrainCollider))
        {
            GetNewDestination();
        }
    }

    private void GetNewDestination()
    {
        // Randomize the target destination.
        float random = Random.Range(0, 3);
        switch (random)
        {
            case 0:
                targetPoint.x = transform.position.x + Random.Range(randomDistanceMin, randomDistanceMax);
                targetPoint.z = transform.position.z + Random.Range(randomDistanceMin, randomDistanceMax);
                break;
            case 1:
                targetPoint.x = transform.position.x - Random.Range(randomDistanceMin, randomDistanceMax);
                targetPoint.z = transform.position.z - Random.Range(randomDistanceMin, randomDistanceMax);
                break;
            case 2:
                targetPoint.x = transform.position.x + Random.Range(randomDistanceMin, randomDistanceMax);
                targetPoint.z = transform.position.z - Random.Range(randomDistanceMin, randomDistanceMax);
                break;
            default:
                targetPoint.x = transform.position.x - Random.Range(randomDistanceMin, randomDistanceMax);
                targetPoint.z = transform.position.z + Random.Range(randomDistanceMin, randomDistanceMax);
                break;
        }
        // Always keep the destination Y vector at the same height.
        targetPoint.y = transform.position.y;

        #if UNITY_EDITOR
        Debug.Log($"{gameObject.name} is retrieving a new destination X {targetPoint.x}, Z {targetPoint.z}");
        #endif
    }

    private void Attack()
    {
        RayCheck();
        // Check if player has left the check distance.
        if (playerDistance >= playerDistanceThreshold)
        {
            rotateToDefault = true;
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

    private void RayCheck()
    {
        // Only have one raycheck running at a time.
        if (!executedRayCheck)
        {
            executedRayCheck = true;
            StartCoroutine(WaitRayCheck());
        }
    }

    private IEnumerator WaitRayCheck()
    {
        yield return new WaitForSeconds(attackRayCheckTime);
        executedRayCheck = false;

        if (rayHit.collider != null && !rayHit.collider.CompareTag("Player"))
        {
            currentState = TankStates.Patrol;
        }
    }

    private void Shoot()
    {
        shootTimer += Time.deltaTime;

        if (rayHit.collider != null && rayHit.collider.CompareTag("Player") && shootTimer >= shootRate)
        {
            shootTimer = 0;

            ShootProjectile();
            ShowFiringParticle();
            shootSound.Play();
        }
    }

    private void ShootProjectile()
    {
        GameObject projectile;
        // Instantiate projectile depending on what enemy type this is.
        projectile = isBoss ? Instantiate(bossProjectile) : PoolManager.Instance.PopAmmo();
        // Ignore collision with the starting collider.
        Physics.IgnoreCollision(barrelCollider, projectile.GetComponent<Collider>());
        // Initialize projectile position and rotation.
        projectile.transform.position = turretBarrelHole.position;
        projectile.transform.rotation = tankTurretBarrel.rotation;
        // Set the projectile velocity depending on what enemy type this is.
        Vector3 projectileVelocity = isBoss ? turretBody.forward + tankTurretBarrel.up : turretBody.forward + tankTurretBarrel.up + new Vector3(0, 0.07175f, 0);
        // Shoot the projectile using it's rigidbody.
        projectile.GetComponent<Rigidbody>().velocity = projectileVelocity * projectileSpeed;
        projectile.SetActive(true);
    }

    private void ShowFiringParticle()
    {
        // Show the firing particle effect.
        GameObject fireParticle = Instantiate(shootParticle);
        fireParticle.transform.position = turretBarrelHole.position;
        Destroy(fireParticle, 3);
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