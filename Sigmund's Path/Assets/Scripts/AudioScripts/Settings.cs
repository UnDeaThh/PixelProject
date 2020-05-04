using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    PauseManager pauseManager;
    PlayerController2 player;
    Inventory2 inventory;
    PlayerAttack plAttack;

    [SerializeField] AudioMixer audioMixer;
    string masterVolume = "masterVolume";
    string musicVolume = "musicVolume";
    string fxVolume = "fxVolume";

    [SerializeField] Slider masterSlider;


    [SerializeField] Toggle fullScreenToggle;
    [SerializeField] Dropdown resolutionDropdown;
    private Resolution[] resolutions = new Resolution[3];

    [SerializeField] GameObject blackFade;

    [SerializeField] GameObject bookContainer;
    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject inventoryPanel;
    [SerializeField] GameObject mapPanel;


    [SerializeField] GameObject rightOptions;
    [SerializeField] GameObject goToMainMenuQuest;
    [SerializeField] GameObject exitGameQuest;
    void Awake()
    {
        pauseManager = GameObject.FindGameObjectWithTag("PauseManager").GetComponent<PauseManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController2>();
        inventory = player.GetComponentInChildren<Inventory2>();
        plAttack = player.gameObject.GetComponent<PlayerAttack>();


        blackFade.SetActive(false);
        settingsPanel.SetActive(false);
        rightOptions.SetActive(false);
        exitGameQuest.SetActive(false);
        goToMainMenuQuest.SetActive(false);
        bookContainer.SetActive(false);
        mapPanel.SetActive(false);

        masterSlider.value = PlayerPrefs.GetFloat(masterVolume, 0);
        fullScreenToggle.isOn = intToBool(PlayerPrefs.GetInt("isFullScreen", 1));
    }

    private void Start()
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

    public void SetMasterVolume(float value)
    {
        audioMixer.SetFloat(masterVolume, value);
        PlayerPrefs.SetFloat(masterVolume, value);
    }
    public void SetMusicVolume(float value)
    {
        audioMixer.SetFloat(musicVolume, value);
    }
    public void SetFXVolume(float value)
    {
        audioMixer.SetFloat(fxVolume, value);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("resolution", resolutionDropdown.value);

    }
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        fullScreenToggle.isOn = isFullScreen;
        PlayerPrefs.SetInt("isFullScreen", boolToInt(fullScreenToggle.isOn));
    }

    public void InventoryTab()
    {
        pauseManager.EventSystem.ffPause = false;

        rightOptions.SetActive(false);
        exitGameQuest.SetActive(false);
        goToMainMenuQuest.SetActive(false);

        pauseManager.IsOnSettings = false;
        pauseManager.IsOnMap = false;
        pauseManager.IsOnInventory = true;

        settingsPanel.SetActive(false);
        mapPanel.SetActive(false);
        inventoryPanel.SetActive(true);
    }

    public void MapTab()
    {
        pauseManager.EventSystem.ffPause = false;

        rightOptions.SetActive(false);
        exitGameQuest.SetActive(false);
        goToMainMenuQuest.SetActive(false);

        pauseManager.IsOnInventory = false;
        pauseManager.IsOnSettings = false;
        pauseManager.IsOnMap = true;

        inventoryPanel.SetActive(false);
        settingsPanel.SetActive(false);
        mapPanel.SetActive(true);
    }

    public void SettingsTab()
    {
        pauseManager.EventSystem.ffPause = false;

        pauseManager.IsOnInventory = false;
        pauseManager.IsOnMap = false;
        pauseManager.IsOnSettings = true;

        inventoryPanel.SetActive(false);
        mapPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    #region Button's SettingsTab

    public void ClcikOnResume()
    {
        pauseManager.Resume();
    }
    public void ClickOnSettings()
    {
        rightOptions.SetActive(true);
        for (int i = 0; i < rightOptions.transform.childCount; i++)
        {
            rightOptions.transform.GetChild(i).gameObject.SetActive(true);
        }
        goToMainMenuQuest.SetActive(false);
        exitGameQuest.SetActive(false);
    }
    public void ClickOnMainMenu()
    {
        rightOptions.SetActive(false);
        exitGameQuest.SetActive(false);
        goToMainMenuQuest.SetActive(true);
    }
    public void ClickOnExit()
    {
        rightOptions.SetActive(false);
        goToMainMenuQuest.SetActive(false);
        exitGameQuest.SetActive(true);
    }

    public void YesMainMenu()
    {
        pauseManager.isPaused = false;
        SaveSystem.SavePlayerData(player, inventory, plAttack);
        SceneManager.LoadScene("MainMenuScene");
    }
    public void NoMainMenu()
    {
        goToMainMenuQuest.SetActive(false);
    }

    public void YesExitGame()
    {
        pauseManager.isPaused = false;
        SaveSystem.SavePlayerData(player, inventory, plAttack);
        Application.Quit();
    }

    public void NoExitGame()
    {
        exitGameQuest.SetActive(false);
    }
    #endregion

    #region Transforming bools and ints
    int boolToInt(bool val)
    {
        if (val)
            return 1;
        else
            return 0;
    }

    bool intToBool(int val)
    {
        if (val != 0)
            return true;
        else
            return false;
    }
    #endregion
}
