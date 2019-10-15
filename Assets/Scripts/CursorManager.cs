using UnityEngine;

public class CursorManager : MonoBehaviour
{
    private void Awake()
    {
        // Main game starts with cursor locked.
        Cursor.lockState = CursorLockMode.Locked;
    }
}