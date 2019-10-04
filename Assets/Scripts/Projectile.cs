using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float deactiveTimer = 5;

    private void OnEnable()
    {
        StartCoroutine(DeactiveTimer());
    }

    private IEnumerator DeactiveTimer()
    {
        yield return new WaitForSeconds(deactiveTimer);
        gameObject.SetActive(false);
        PoolManager.Instance.PushAmmo(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<IDamageable>() != null)
        {
            int randomDamage = Random.Range(1, 10);
            collision.gameObject.GetComponent<IDamageable>().TakeDamage(randomDamage);
        }
    }
}
