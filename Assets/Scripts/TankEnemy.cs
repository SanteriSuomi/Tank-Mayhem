public class TankEnemy : Tank, IDamageable
{
    public override float hitPoints { get; set; }

    protected override void Initialize()
    {

    }

    protected override void UpdateState()
    {

    }

    public void DestroySelf(float timer)
    {
        Destroy(gameObject, timer);
    }

    public void TakeDamage(float damage)
    {
        hitPoints -= damage;
    }
}
