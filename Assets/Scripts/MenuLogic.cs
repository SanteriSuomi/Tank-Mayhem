using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuLogic : MonoBehaviour
{
    [SerializeField]
    private GameObject menuButtonsParent = default;
    [SerializeField]
    private GameObject optionsButtonsParent = default;
    [SerializeField]
    private Slider volumeSlider = default;
    [SerializeField]
    private Slider qualitySlider = default;
    [SerializeField]
    private TextMeshProUGUI qualityNameText = default;

    private string[] qualitySettings;

    private void Awake()
    {
        qualitySettings = QualitySettings.names;

        volumeSlider.maxValue = AudioListener.volume;
        volumeSlider.value = AudioListener.volume / 2;

        QualitySettings.SetQualityLevel(qualitySettings.Length - 1);
        qualitySlider.maxValue = qualitySettings.Length - 1;
        qualitySlider.value = QualitySettings.GetQualityLevel();
    }

    private void Update()
    {
        VolumeSlider();
        QualitySlider();
    }
    
    private void VolumeSlider()
    {
        AudioListener.volume = volumeSlider.value;
    }

    private void QualitySlider()
    {
        try
        {
            QualitySettings.SetQualityLevel((int)qualitySlider.value);
            qualityNameText.text = qualitySettings[(int)qualitySlider.value];
        }
        catch (System.Exception e)
        {
            #if UNITY_EDITOR
            Debug.Log(e);
            #endif
        }
    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void ExitGame()
    {
        Application.Quit(0);
    }

    public void OptionsEnable()
    {
        menuButtonsParent.SetActive(false);
        optionsButtonsParent.SetActive(true);
    }

    public void OptionsDisable()
    {
        menuButtonsParent.SetActive(true);
        optionsButtonsParent.SetActive(false);
    }
}
