using UnityEngine;

public class DontDestroyThis : MonoBehaviour
{
<<<<<<< HEAD
    private void Awake()
    {
        // Ensure that this gameObject and its children won't get destroyed between scene loads.
=======
    // Don't destroy objects this script is attached to when changing scenes.

    private void Awake()
    {
>>>>>>> TankControllerMaking
        DontDestroyOnLoad(gameObject);
    }
}
