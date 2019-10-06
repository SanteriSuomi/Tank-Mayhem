using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Game manager will contains small items that will only have to be run once at the start of the game.
    private void Awake()
    {
        // Lock cursor to the center of the game.
        Cursor.lockState = CursorLockMode.Locked;
    }
}
