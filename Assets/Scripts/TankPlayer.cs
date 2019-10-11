using UnityEngine.SceneManagement;

public class TankPlayer : Tank, IDamageable
{
    public float HitPoints { get; set; } = 500;

    public float MaxHitpoints { get; private set; }

    protected override void Initialize()
    {
        MaxHitpoints = HitPoints;
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
            SceneManager.LoadScene("Scene01");
        }
    }
}