using UnityEngine;

public class CursorManager : MonoBehaviour
{
    private void Awake()
    {
        // Main game starts with cursor locked.
        Cursor.lockState = CursorLockMode.Locked;
        // Cursor shouldn't be visible at the start.
        Cursor.visible = false;
    }
}