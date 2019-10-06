using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float deactiveTimer = 5;
    [SerializeField]
    private int projectileDamageMin = 15;
    [SerializeField]
    private int projectileDamageMax = 25;

    private void OnEnable()
    {
        StartCoroutine(DeactiveTimer());
    }

    private IEnumerator DeactiveTimer()
    {
        yield return new WaitForSeconds(deactiveTimer);
        DeactivateGameObject();
        PushAmmoToPool();
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamageable collisionObject = collision.gameObject.GetComponent<IDamageable>();
        if (collisionObject != null)
        {
            int randomDamage = Random.Range(projectileDamageMin, projectileDamageMax);
            collisionObject.TakeDamage(randomDamage);
            DeactivateGameObject();
            PushAmmoToPool();
        }
    }

    private void DeactivateGameObject()
    {
        gameObject.SetActive(false);
    }

    private void PushAmmoToPool()
    {
        AmmoPoolManager.Instance.PushHeavyAmmo(gameObject);
    }
}
