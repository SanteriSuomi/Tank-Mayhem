using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject menuButtonsParent = default;

    private void Update()
    {
        // Halt the game progress while in game menu.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowCursor();
            FreezeAudio();
            ChangeCursorMode();
            ShowHideMenu();
            FreezeUnFreezeGame();
        }
    }

    private void ShowCursor()
    {
        Cursor.visible = !Cursor.visible;
    }

    private void FreezeAudio()
    {
        AudioListener.pause = !AudioListener.pause;
    }

    private void ChangeCursorMode()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void ShowHideMenu()
    {
        menuButtonsParent.SetActive(!menuButtonsParent.activeSelf);
    }

    private void FreezeUnFreezeGame()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }   

    public void LoadSceneGameMenu(string scene)
    {

        SceneManager.LoadScene(scene);
    }
}
