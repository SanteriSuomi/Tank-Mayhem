using UnityEngine;

public class DontDestroyThis : MonoBehaviour
{
    private void Awake()
    {
        // Ensure that this gameObject and its children won't get destroyed between scene loads.
        DontDestroyOnLoad(gameObject);
    }
}
