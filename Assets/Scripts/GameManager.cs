using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Game manager will contains small items that will only have to be run once at the start of the game.

    // Implement a static property for accessing this manager globally (singleton pattern).
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        // Ensure that this instance of this script is the only one.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        // Lock cursor to the center of the game.
        Cursor.lockState = CursorLockMode.Locked;
    }
}
