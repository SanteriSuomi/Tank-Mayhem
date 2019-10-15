using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
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
    private TextMeshProUGUI volumeNameText = default;
    [SerializeField]
    private TextMeshProUGUI qualityNameText = default;

    private string[] qualitySettings;
    // Initialize at start because menusettings is awake.
    private void Start()
    {
        qualitySettings = QualitySettings.names;
        // Initialize these settings only once during the game.
        if (!MenuSettings.Instance.MainMenuHasInitialized)
        {
            #if UNITY_EDITOR
            Debug.Log($"{gameObject} initialized.");
            #endif

            MenuSettings.Instance.MainMenuHasInitialized = true;

            volumeSlider.maxValue = MenuSettings.Instance.VolumeValue;
            //volumeSlider.value = MenuSettings.Instance.VolumeValue;

            QualitySettings.SetQualityLevel(qualitySettings.Length - 1);
        }
        // Update the volume and quality sliders after scene switches.
        volumeSlider.value = MenuSettings.Instance.VolumeValue;

        qualitySlider.maxValue = qualitySettings.Length - 1;
        qualitySlider.value = QualitySettings.GetQualityLevel();
    }

    private void Update()
    {
        VolumeSlider();
        QualitySlider();
    }

    #region Private Methods
    private void VolumeSlider()
    {
        // Update the volume to correspont the slider value.
        AudioListener.volume = volumeSlider.value;
        // Store the volume.
        MenuSettings.Instance.VolumeValue = AudioListener.volume;

        try
        {
            // Round the multiplied slider value.
            int volumeMultiplier = Mathf.RoundToInt(AudioListener.volume * 100);
            // Convert this value to string.
            string volumeToString = Convert.ToString(volumeMultiplier);
            // Update the volume text.
            volumeNameText.text = volumeToString;
        }
        catch (Exception e)
        {
            #if UNITY_EDITOR
            Debug.Log(e);
            #endif
        }
    }

    private void QualitySlider()
    {
        try
        {
            // Update quality to correspond the slider value.
            QualitySettings.SetQualityLevel((int)qualitySlider.value);
            qualityNameText.text = qualitySettings[(int)qualitySlider.value];
        }
        catch (Exception e)
        {
            #if UNITY_EDITOR
            Debug.Log(e);
            #endif
        }
    }
    #endregion

    #region Public Methods
    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
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

    public void ExitGame()
    {
        Application.Quit(0);
    }
    #endregion
}