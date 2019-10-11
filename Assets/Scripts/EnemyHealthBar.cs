using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider healthBar = default;
    [SerializeField]
    private TankEnemy tankEnemy = default;
    private Transform player;

    [SerializeField]
    private float playerDistanceCheckTime = 0.5f;
    private float playerDistance;
    private float playerDistanceCheckTimer;

    [SerializeField]
    private int healthBarScaleReductionX = 1750;
    [SerializeField]
    private int healthBarScaleReductionY = 1000;

    private void Awake()
    {
        player = GameObject.Find("PRE_Tank_Player").GetComponent<Transform>();
        healthBar.maxValue = tankEnemy.HitPoints;
    }

    private void LateUpdate()
    {
        playerDistanceCheckTimer += Time.deltaTime;
        if (player != null && playerDistanceCheckTimer >= playerDistanceCheckTime)
        {
            playerDistanceCheckTimer = 0;
            playerDistance = Vector3.Distance(transform.position, player.position);
        }

        healthBar.transform.LookAt(player);
        healthBar.value = tankEnemy.HitPoints;
        healthBar.transform.localScale = new Vector3(playerDistance / healthBarScaleReductionX, playerDistance / healthBarScaleReductionY, 0);
    }
}