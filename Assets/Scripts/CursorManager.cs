using UnityEngine;

public class CursorManager : MonoBehaviour
{
    private void Awake()
    {
        // Lock cursor to the center of the game.
        Cursor.lockState = CursorLockMode.Locked;
    }
}
