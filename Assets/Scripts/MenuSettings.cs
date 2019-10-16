using UnityEngine;

public class MenuSettings : MonoBehaviour
{
    // Store some data about the main menu settings.
    public static MenuSettings Instance { get; private set; }

    public bool MainMenuHasInitialized { get; set; } = false;

    public float VolumeValue { get; set; } = 1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
