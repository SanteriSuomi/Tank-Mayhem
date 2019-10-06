using UnityEngine;

public class HeavyTankShoot : MonoBehaviour
{
    [SerializeField]
    private Transform tankTurretBarrelBase = default;
    [SerializeField]
    private Transform tankTurretBarrelHole = default;
    [SerializeField]
    private Collider tankCollider = default;

    [SerializeField]
    private float projectileSpeed = 35;

    private bool pressedLeftClick;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pressedLeftClick = true;
        }
    }

    private void FixedUpdate()
    {
        if (pressedLeftClick)
        {
            pressedLeftClick = false;

            GameObject projectile = AmmoPoolManager.Instance.PopHeavyAmmo();
            Physics.IgnoreCollision(tankCollider, projectile.GetComponent<Collider>());
            projectile.transform.position = tankTurretBarrelHole.position;
            projectile.GetComponent<Rigidbody>().velocity = (transform.forward * projectileSpeed) + (tankTurretBarrelBase.transform.up * projectileSpeed);
            projectile.SetActive(true);
        }
    }
}
