using UnityEngine;
using System.Collections;

public class TankEnemy : Tank, IDamageable
{
    protected override float HitPoints { get; set; } = 100;

    public float GetHitpoints() { return HitPoints; }

    //private TankEnemyPatrol enemyPatrol;
    //private TankEnemyAttack enemyAttack;

    [SerializeField]
    private Transform turretBody = default;
    [SerializeField]
    private Transform turretBarrelHole = default;
    [SerializeField]
    private Transform tankTurretBarrel = default;
    [SerializeField]
    private Transform tankTurretBody = default;
    [SerializeField]
    private Transform player = default;
    [SerializeField]
    private GameObject ammo = default;
    [SerializeField]
    private Collider barrelCollider = default;

    private Vector3 targetPoint;

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

    private float targetDistance;
    private float playerDistance;
    private float playerDistanceCheckTimer;
    private float shootTimer;

    private bool rotateDefault;
    private bool executedWait;

    private Vector3 tankDirection;
    private Quaternion tankLookRotation;

    private Vector3 tankTurretDirection;
    private Quaternion tankTurretLookRotation;

    protected override void Initialize()
    {
        //enemyPatrol = GetComponent<TankEnemyPatrol>();
        //enemyAttack = GetComponent<TankEnemyAttack>();
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
        // Destroy enemy if hitpoints below zero.
        CheckDestroySelf();
        // Keep track of player's distance every X seconds.
        PlayerDistanceCheckTimer();

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

    private void PlayerDistanceCheckTimer()
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

    private void GetNewDestination()
    {
        // Randomise the targetPoint vector.
        float random = Random.Range(0, 3);
        if (random == 0)
        {
            targetPoint.x = transform.position.x + Random.Range(randomDistanceMin, randomDistanceMax);
            targetPoint.y = transform.position.y;
            targetPoint.z = transform.position.z + Random.Range(randomDistanceMin, randomDistanceMax);
        }
        else if (random == 1)
        {
            targetPoint.x = transform.position.x - Random.Range(randomDistanceMin, randomDistanceMax);
            targetPoint.y = transform.position.y;
            targetPoint.z = transform.position.z - Random.Range(randomDistanceMin, randomDistanceMax);
        }
        else if (random == 2)
        {
            targetPoint.x = transform.position.x + Random.Range(randomDistanceMin, randomDistanceMax);
            targetPoint.y = transform.position.y;
            targetPoint.z = transform.position.z - Random.Range(randomDistanceMin, randomDistanceMax);
        }
        else
        {
            targetPoint.x = transform.position.x - Random.Range(randomDistanceMin, randomDistanceMax);
            targetPoint.y = transform.position.y;
            targetPoint.z = transform.position.z + Random.Range(randomDistanceMin, randomDistanceMax);
        }

        Debug.Log($"{gameObject.name} is retrieving a new destination X {targetPoint.x}, Z {targetPoint.z}");
    }

    private void Attack()
    {
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

    private void Shoot()
    {
        shootTimer += Time.deltaTime;

        RaycastHit rayHit;
        Vector3 forward = turretBarrelHole.TransformDirection(Vector3.up);
        Physics.Raycast(turretBarrelHole.position, forward, out rayHit);

        #if UNITY_EDITOR
        Debug.DrawRay(turretBarrelHole.position, forward);
        #endif

        if (rayHit.collider != null && rayHit.collider.gameObject.CompareTag("Player") && shootTimer >= shootRate)
        {
            shootTimer = 0;

            GameObject projectile = Instantiate(ammo);
            Physics.IgnoreCollision(barrelCollider, projectile.GetComponent<Collider>());
            projectile.transform.position = turretBarrelHole.position;
            projectile.transform.rotation = tankTurretBarrel.rotation;
            projectile.GetComponent<Rigidbody>().velocity = (tankTurretBody.forward + tankTurretBarrel.up) * projectileSpeed;
            projectile.SetActive(true);
        }
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
            Destroy(gameObject);
        }
    }
    #endregion

    #region Debug Logs
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
    #endregion

    #region Gizmos
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, targetPoint);
        Gizmos.DrawWireSphere(transform.position, playerDistanceThreshold);
    }
    #endif
    #endregion
}