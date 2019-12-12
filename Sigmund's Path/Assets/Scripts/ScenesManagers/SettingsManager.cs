using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider slider;
    public Dropdown qualityDropdown;
    public Toggle fullScreenToggle;

    [Header("RESOLUTION ATRIBUTES")]
    private Resolution[] resolutions = new Resolution[3];
    public Dropdown resolutionDropdown;
   // private int currentResolutionIndex = 0;
    private void Awake()
    {
        slider.value = PlayerPrefs.GetFloat("volume", 0);
        qualityDropdown.value = PlayerPrefs.GetInt("quality", 3);
        fullScreenToggle.isOn = intToBool(PlayerPrefs.GetInt("isFullScreen", 1));


        ResolutionsForDropdown();
        Debug.Log(Screen.currentResolution);

    }
    private void ResolutionsForDropdown()
    {
        resolutions[0] = Screen.resolutions[0]; //640 x 480
        resolutions[1] = Screen.resolutions[6]; //1280 x 720
        resolutions[2] = Screen.resolutions[Screen.resolutions.Length - 1]; // La Maxima resolucion
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = PlayerPrefs.GetInt("resolution", 2);
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(Dropdown dropdown)
    {
        Resolution resolution = resolutions[dropdown.value];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("resolution", resolutionDropdown.value);
        Debug.Log(Screen.currentResolution);


    }


    public void BackMenuButton()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void SetVolume(Slider slider)
    {
        audioMixer.SetFloat("volume", slider.value);
        PlayerPrefs.SetFloat("volume", slider.value);
    }

    public void SetQuality(Dropdown dropdown)
    {
        QualitySettings.SetQualityLevel(dropdown.value);
        PlayerPrefs.SetInt("quality", dropdown.value);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        fullScreenToggle.isOn = isFullScreen;
        PlayerPrefs.SetInt("isFullScreen", boolToInt(fullScreenToggle.isOn));
    }

    #region Transforming bools and ints
    int boolToInt(bool val)
    {
        if (val)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    bool intToBool(int val)
    {
        if (val != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion
}
