public class TankPlayer : Tank, IDamageable
{
    protected override float HitPoints { get; set; } = 200;

    protected override void Initialize()
    {
        
    }

    protected override void UpdateState()
    {
        CheckDestroySelf();
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
