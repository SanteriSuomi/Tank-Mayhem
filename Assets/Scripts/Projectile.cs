using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float deactiveTimer = 7.5f;

    private void Start()
    {
        StartCoroutine(DeactiveTimer());
    }

    private IEnumerator DeactiveTimer()
    {
        yield return new WaitForSeconds(deactiveTimer);
        gameObject.SetActive(false);
    }
}
