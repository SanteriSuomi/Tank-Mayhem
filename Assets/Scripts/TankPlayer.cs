public class TankPlayer : Tank, IDamageable
{
    protected override float HitPoints { get; set; } = 400;

    protected override void Initialize()
    {
        
    }

    protected override void StartState()
    {

    }

    protected override void UpdateState()
    {
        CheckDestroySelf();
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
