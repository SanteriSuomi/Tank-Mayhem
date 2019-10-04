﻿public class TankEnemy : Tank, IDamageable
{
    public override float HitPoints { get; set; } = 50;

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
