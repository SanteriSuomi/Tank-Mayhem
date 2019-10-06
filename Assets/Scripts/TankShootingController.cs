using UnityEngine;

public class TankShootingController : MonoBehaviour
{
    [SerializeField]
    private Transform tankTurretBody = default;
    [SerializeField]
    private Transform tankTurretBarrel = default;
    [SerializeField]
    private Transform barrelHole = default;
    [SerializeField]
    private Collider tankCollider = default;

    [SerializeField]
    private float projectileSpeed = 55;
    [SerializeField]
    private float timerThreshold = 1;
    private float timer;

    private bool pressedLeftClick;

    private void Update()
    {
        timer += Time.deltaTime;
        if (Input.GetMouseButtonDown(0))
        {
            pressedLeftClick = true;
        }
    }

    private void FixedUpdate()
    {
        if (pressedLeftClick && timer > timerThreshold)
        {
            timer = 0;

            GameObject projectile = PoolManager.Instance.PopAmmo();
            Physics.IgnoreCollision(tankCollider, projectile.GetComponent<Collider>());
            projectile.transform.position = barrelHole.position;
            projectile.transform.rotation = tankTurretBarrel.rotation;
            projectile.GetComponent<Rigidbody>().velocity = (tankTurretBody.transform.forward + tankTurretBarrel.transform.up) * projectileSpeed;
            projectile.SetActive(true);
        }

        pressedLeftClick = false;
    }
}
