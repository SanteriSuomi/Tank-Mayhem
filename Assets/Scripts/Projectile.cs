using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Separate explosion prefabs for normal enemies and the boss.
    [SerializeField]
    private GameObject explosionPrefab = default;
    [SerializeField]
    private GameObject explosionPrefabBoss = default;
    private GameObject explosion;
    private AudioSource explosionSound;

    private Vector3 regularSize;

    [SerializeField]
    private int projectileDamageMin = 30;
    [SerializeField]
    private int projectileDamageMax = 50;

    private bool alreadyHit;

    private void Awake()
    {
        explosionSound = GetComponent<AudioSource>();
        regularSize = gameObject.transform.localScale;
    }

    private void OnEnable()
    {
        alreadyHit = false;
        // Initialize the size to be regular.
        gameObject.transform.localScale = regularSize;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Ensure OnCollision enter is run only once.
        if (!alreadyHit)
        {
            alreadyHit = true;

            InstantiateExplosion(collision);
            PlayExplosionSound();
            DamageCollision(collision);
            // Make object invisible.
            gameObject.transform.localScale = Vector3.zero;
            StartCoroutine(DeactivateAndPush());
        }
    }

    private void InstantiateExplosion(Collision collision)
    {
        // Instantiate different explosion prefab on collision according the name of the gameobject.
        switch (gameObject.name)
        {
            case "PRE_Projectile_Boss":
                explosion = Instantiate(explosionPrefabBoss);
                break;
            default:
                explosion = Instantiate(explosionPrefab);
                break;
        }
        // Instantiate the explosion at the first contact point.
        ContactPoint contact = collision.GetContact(0);
        explosion.transform.position = contact.point;
        Destroy(explosion, 3);
    }

    private void PlayExplosionSound()
    {
        if (!explosionSound.isPlaying)
        {
            explosionSound.Play();
        }
    }

    private void DamageCollision(Collision collision)
    {
        // Deal random damage to the object if it has IDamageable interface.
        IDamageable collisionObject = collision.transform.root.gameObject.GetComponent<IDamageable>();
        if (collisionObject != null)
        {
            int randomDamage = Random.Range(projectileDamageMin, projectileDamageMax);
            collisionObject.TakeDamage(randomDamage);
        }
    }

    private IEnumerator DeactivateAndPush()
    {
        // Put this instance back to the stack.
        yield return new WaitForSeconds(explosionSound.clip.length);
        gameObject.SetActive(false);
        PoolManager.Instance.PushAmmo(gameObject);
    }
}