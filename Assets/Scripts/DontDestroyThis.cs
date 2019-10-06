using UnityEngine;

public class DontDestroyThis : MonoBehaviour
{
    // Don't destroy objects this script is attached to when changing scenes.

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
