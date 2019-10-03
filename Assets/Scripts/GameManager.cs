using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        // Lock cursor to the center of the game.
        Cursor.lockState = CursorLockMode.Locked;
    }
}
