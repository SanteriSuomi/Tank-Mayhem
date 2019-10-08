using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider healthBar;
    [SerializeField]
    private TankEnemy tankEnemy;
    [SerializeField]
    private Transform player;

    private void Start()
    {
        healthBar.maxValue = tankEnemy.GetHitpoints();
    }

    private void Update()
    {
        healthBar.transform.LookAt(player);
        healthBar.value = tankEnemy.GetHitpoints();
    }
}
