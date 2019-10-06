using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float deactiveTimer = 7.5f;
    [SerializeField]
    private int projectileDamageMin = 20;
    [SerializeField]
    private int projectileDamageMax = 30;

    private void OnEnable()
    {
        StartCoroutine(DeactivateTimer());
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamageable collisionObject = collision.gameObject.GetComponent<IDamageable>();
        if (collisionObject != null)
        {
            int randomDamage = Random.Range(projectileDamageMin, projectileDamageMax);
            collisionObject.TakeDamage(randomDamage);
            DeactivateAndPush();
        }
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
