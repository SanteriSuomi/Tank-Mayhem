using UnityEngine;

public class TankShoot : MonoBehaviour
{
    [SerializeField]
    private Transform barrelHole = default;
    private TankAmmoPool ammoPool;

    private bool pressedLeftClick;

    private void Awake()
    {
        ammoPool = FindObjectOfType<TankAmmoPool>();
    }

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

            GameObject projectile = ammoPool.GetProjectile();
            projectile.transform.position = barrelHole.position;
            projectile.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, 10));
            Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), projectile.GetComponent<Collider>());
            projectile.SetActive(true);
        }
    }
}
