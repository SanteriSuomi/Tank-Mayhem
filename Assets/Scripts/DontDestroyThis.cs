using UnityEngine;

public class DontDestroyThis : MonoBehaviour
{
    private void Awake()
    {
        // Don't destroy objects this script is attached to when loading scenes.
        DontDestroyOnLoad(gameObject);
    }
}