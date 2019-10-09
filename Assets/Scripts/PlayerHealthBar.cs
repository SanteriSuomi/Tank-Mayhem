using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider healthBar = default;
    private TankPlayer player;

    private void Awake()
    {
        player = GetComponent<TankPlayer>();
    }

    private void Start()
    {
        healthBar.maxValue = player.HitPoints;
    }

    private void LateUpdate()
    {
        healthBar.value = player.HitPoints;
    }
}