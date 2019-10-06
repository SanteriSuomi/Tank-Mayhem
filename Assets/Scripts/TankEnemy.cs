public class TankEnemy : Tank, IDamageable
{
    protected override float HitPoints { get; set; } = 50;

    private enum TankStates
    {
        Stop,
        Patrol,
        Attack
    }

    private TankStates currentState;

    protected override void Initialize()
    {
        currentState = TankStates.Patrol;
    }

    protected override void UpdateState()
    {
        CheckDestroySelf();

        switch (currentState)
        {
            case TankStates.Stop:
                break;
            case TankStates.Patrol:
                break;
            case TankStates.Attack:
                break;
            default:
                break;
        }
    }

    protected override void UpdateStateFixed()
    {

    }

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
}
