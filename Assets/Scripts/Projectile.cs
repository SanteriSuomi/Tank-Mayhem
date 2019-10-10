using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private GameObject explosionPrefab = default;
    [SerializeField]
    private float deactiveTimer = 7.5f;
    [SerializeField]
    private int projectileDamageMin = 30;
    [SerializeField]
    private int projectileDamageMax = 50;

    private void OnEnable()
    {
        StartCoroutine(DeactivateTimer());
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.GetContact(0);
        GameObject explosion = Instantiate(explosionPrefab);
        explosion.transform.position = contact.point;
        Destroy(explosion, deactiveTimer);

        IDamageable collisionObject = collision.transform.root.gameObject.GetComponent<IDamageable>();
        if (collisionObject != null)
        {
            int randomDamage = Random.Range(projectileDamageMin, projectileDamageMax);
            collisionObject.TakeDamage(randomDamage);
        }

        DeactivateAndPush();
    }

    private IEnumerator DeactivateTimer()
    {
        yield return new WaitForSeconds(deactiveTimer);
        DeactivateAndPush();
    }

    private void DeactivateAndPush()
    {
        gameObject.SetActive(false);
        PoolManager.Instance.PushAmmo(gameObject);
    }
}