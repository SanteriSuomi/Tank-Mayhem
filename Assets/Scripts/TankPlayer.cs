public class TankPlayer : Tank, IDamageable
{
    public override float HitPoints { get; set; } = 5;

    protected override void Initialize()
    {
        
    }

    protected override void UpdateState()
    {
        SelfDestroy();
    }

    public void TakeDamage(float damage)
    {
        HitPoints -= damage;
    }

    public void SelfDestroy()
    {
        if (HitPoints <= 0)
        {
            Destroy(gameObject);
        }
    }
}
