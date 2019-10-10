using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private GameObject explosionPrefab = default;
    [SerializeField]
    private GameObject explosionPrefabBoss = default;
    [SerializeField]
    private int projectileDamageMin = 30;
    [SerializeField]
    private int projectileDamageMax = 50;

    private Vector3 regularSize;

    private bool alreadyHit;

    private GameObject explosion;
    private AudioSource explosionSound;

    private void Awake()
    {
        explosionSound = GetComponent<AudioSource>();
        regularSize = gameObject.transform.localScale;
    }

    private void OnEnable()
    {
        alreadyHit = false;
        gameObject.transform.localScale = regularSize;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!alreadyHit)
        {
            alreadyHit = true;

            ContactPoint contact = collision.GetContact(0);
            if (gameObject.name == "PRE_Projectile_Boss")
            {
                explosion = Instantiate(explosionPrefabBoss);
            }
            else
            {
                explosion = Instantiate(explosionPrefab);
            }
            explosion.transform.position = contact.point;
            Destroy(explosion, 3);

            if (!explosionSound.isPlaying)
            {
                explosionSound.Play();
            }

            IDamageable collisionObject = collision.transform.root.gameObject.GetComponent<IDamageable>();
            if (collisionObject != null)
            {
                int randomDamage = Random.Range(projectileDamageMin, projectileDamageMax);
                collisionObject.TakeDamage(randomDamage);
            }

            gameObject.transform.localScale = new Vector3(0, 0, 0);
            DeactivateAndPush();
        }
    }

    private IEnumerator DeactivateAndPush()
    {
        yield return new WaitForSeconds(explosionSound.clip.length);
        gameObject.SetActive(false);
        PoolManager.Instance.PushAmmo(gameObject);
    }
}