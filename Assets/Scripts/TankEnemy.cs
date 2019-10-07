using UnityEngine;
using System.Linq;
using System.Collections;

public class TankEnemy : Tank, IDamageable
{
    protected override float HitPoints { get; set; } = 50;

    private TankEnemyPatrol enemyPatrol;
    private TankEnemyAttack enemyAttack;

    [SerializeField]
    private Transform turretBody;
    [SerializeField]
    private Transform player;

    private Vector3 targetPoint;

    [SerializeField]
    private int maxDistanceFromTarget = 1;
    [SerializeField]
    private int randomDistanceMin = 30;
    [SerializeField]
    private int randomDistanceMax = 60;
    [SerializeField]
    private float rotationSpeed = 80;
    [SerializeField]
    private float turretRotationSpeed = 120;
    [SerializeField]
    private float moveSpeed = 5.5f;
    [SerializeField]
    private float playerDistanceThreshold = 75;

    private float targetDistance;
    private float playerDistance;

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
        CheckDestroySelf();
        playerDistance = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case TankStates.Stop:
                Stop();
                Debug.Log($"{gameObject.name} stopped.");
                break;
            case TankStates.Patrol:
                Patrol();
                Debug.Log($"{gameObject.name} started patrolling.");
                break;
            case TankStates.Attack:
                Attack();
                Debug.Log($"{gameObject.name} started attacking.");
                break;
            default:
                break;
        }
    }

    private void Stop()
    {
        GetNewDestination();
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(Random.Range(0, 2.5f));
        Debug.Log($"{gameObject.name} started patrolling.");
        currentState = TankStates.Patrol;
    }

    private void Patrol()
    {
        // Check if player is within distance.
        if (playerDistance <= playerDistanceThreshold)
        {
            currentState = TankStates.Attack;
        }

        // Move towards target.
        targetDistance = Vector3.Distance(transform.position, targetPoint);
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, moveSpeed * Time.deltaTime);
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
        Debug.Log($"{gameObject.name} is retrieving a new destination.");
        targetPoint.x = Random.Range(randomDistanceMin, randomDistanceMax);
        targetPoint.y = transform.position.y;
        targetPoint.z = Random.Range(randomDistanceMin, randomDistanceMax);
    }

    private void Attack()
    {
        if (playerDistance >= playerDistanceThreshold)
        {
            //turretBody.rotation = Quaternion.identity;
            currentState = TankStates.Patrol;
        }

        // Rotate turret towards target.
        tankTurretDirection = (player.position - transform.position).normalized;
        tankTurretLookRotation = Quaternion.LookRotation(tankTurretDirection);
        turretBody.rotation = Quaternion.RotateTowards(turretBody.rotation, tankTurretLookRotation, turretRotationSpeed * Time.deltaTime);
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
}