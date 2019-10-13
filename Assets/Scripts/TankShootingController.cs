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
    private Collider barrelCollider = default;
    [SerializeField]
    private GameObject shootParticle = default;
    private AudioSource shootSound;

    [SerializeField]
    private float projectileSpeed = 55;
    [SerializeField]
    private float timerThreshold = 1;
    private float timer;

    private bool pressedLeftClick;

    private void Awake()
    {
        shootSound = GetComponentInChildren<AudioSource>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        GetInput();
    }

    private void GetInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pressedLeftClick = true;
        }
    }

    private void FixedUpdate()
    {
        Shoot();
    }

    private void Shoot()
    {
        if (pressedLeftClick && timer > timerThreshold)
        {
            timer = 0;

            GameObject projectile = PoolManager.Instance.PopAmmo();
            if (projectile != null)
            {
                ShootProjectile(projectile);
                ShowFireParticle();
                shootSound.Play();
            }
        }

        pressedLeftClick = false;
    }

    private void ShootProjectile(GameObject projectile)
    {
        // Shoot the projectile from the barrel.
        Physics.IgnoreCollision(barrelCollider, projectile.GetComponent<Collider>());
        // Set the position and transform of the projectile to match the tank.
        projectile.transform.position = barrelHole.position;
        projectile.transform.rotation = tankTurretBarrel.rotation;
        // Add velocity for the projectile forward.
        projectile.GetComponent<Rigidbody>().velocity = (tankTurretBody.transform.forward + tankTurretBarrel.transform.up) * projectileSpeed;
        projectile.SetActive(true);
    }

    private void ShowFireParticle()
    {
        // Instantiate the firing particle at the barrel.
        GameObject fireParticle = Instantiate(shootParticle);
        fireParticle.transform.position = barrelHole.position;
        Destroy(fireParticle, 3);
    }
}