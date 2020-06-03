using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{

    //AUDIO
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider fxSlider;
    string masterVolume = "masterVolume";
    string musicVolume = "musicVolume";
    string fxVolume = "fxVolume";

    [SerializeField] Toggle fullScreenToggle;

    [Header("RESOLUTION ATRIBUTES")]
    [SerializeField] Dropdown resolutionDropdown;
    private Resolution[] resolutions = new Resolution[3];
    // private int currentResolutionIndex = 0;

    [SerializeField] AudioSource source;
    [SerializeField] AudioClip[] audioClips;

    private bool onAwake = true;
    private void Awake()
    {
        ResolutionsForDropdown();
        Debug.Log(Screen.currentResolution);

    }

    private void Start()
    {
        masterSlider.value = PlayerPrefs.GetFloat(masterVolume, 0);
        musicSlider.value = PlayerPrefs.GetFloat(musicVolume, 0);
        fxSlider.value = PlayerPrefs.GetFloat(fxVolume, 0);

        audioMixer.SetFloat(masterVolume, masterSlider.value);
        audioMixer.SetFloat(musicVolume, musicSlider.value);
        audioMixer.SetFloat(fxVolume, fxSlider.value);

        resolutionDropdown.value = PlayerPrefs.GetInt("resolution", 2);

        fullScreenToggle.isOn = intToBool(PlayerPrefs.GetInt("isFullScreen", 1));
        Debug.Log(resolutionDropdown.value);

        onAwake = false;
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
        resolutionDropdown.value = PlayerPrefs.GetInt("resolution", resolutions.Length - 1);
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("resolution", resolutionDropdown.value);

    }

    public void BackMenuButton()
    {
        PlayerPrefs.Save();
        source.clip = audioClips[0];
        source.Play();
        SceneManager.LoadScene("MainMenuScene");
    }

    public void SetMasterVolume(float value)
    {
        audioMixer.SetFloat(masterVolume, value);
        if (!onAwake)
        {
            if (ScenesManager.scenesManager.Gamepad != null)
            {
                source.clip = audioClips[1];
                source.Play();
            }
            else
            {
                if (!source.isPlaying)
                {
                    source.clip = audioClips[1];
                    source.Play();
                }
            }
        }
        PlayerPrefs.SetFloat(masterVolume, value);
    }
    public void SetMusicVolume(float value)
    {
        audioMixer.SetFloat(musicVolume, value);
        if (!onAwake)
        {
            if (ScenesManager.scenesManager.Gamepad != null)
            {
                source.clip = audioClips[1];
                source.Play();
            }
            else
            {
                if (!source.isPlaying)
                {
                    source.clip = audioClips[1];
                    source.Play();
                }
            }
        }
        PlayerPrefs.SetFloat(musicVolume, value);
    }
    public void SetFXVolume(float value)
    {
        audioMixer.SetFloat(fxVolume, value);
        if (!onAwake)
        {
            if (ScenesManager.scenesManager.Gamepad != null)
            {
                source.clip = audioClips[1];
                source.Play();
            }
            else
            {
                if (!source.isPlaying)
                {
                    source.clip = audioClips[1];
                    source.Play();
                }
            }
        }
        PlayerPrefs.SetFloat(fxVolume, value);
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
