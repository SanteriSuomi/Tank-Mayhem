using UnityEngine;

public class DontDestroyThis : MonoBehaviour
{
    [SerializeField]
    private bool dontDestroy = false;

    private void Awake()
    {
        if (dontDestroy)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}