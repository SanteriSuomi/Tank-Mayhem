using UnityEngine;

public class DontDestroyThis : MonoBehaviour
{
    [SerializeField]
    private bool dontDestroy = default;

    private void Awake()
    {
        if (dontDestroy)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}