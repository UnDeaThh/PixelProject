﻿using System.Collections;
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
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider fxSlider;


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
    [SerializeField] AudioSource source;

    [SerializeField] AudioClip[] audioClips; //El 0 es para el click de los botones, el 1 para los sliders 


    [SerializeField] GameObject[] askButtons;

    private bool onAwake = true;
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
        musicSlider.value = PlayerPrefs.GetFloat(musicVolume, 0);
        fxSlider.value = PlayerPrefs.GetFloat(fxVolume, 0);


        onAwake = false;
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


        audioMixer.SetFloat(masterVolume, masterSlider.value);
        audioMixer.SetFloat(musicVolume, musicSlider.value);
        audioMixer.SetFloat(fxVolume, fxSlider.value);

        fullScreenToggle.isOn = intToBool(PlayerPrefs.GetInt("isFullScreen", 1));
    }

    public void SetMasterVolume(float value)
    {
        audioMixer.SetFloat(masterVolume, value);
        if (!onAwake)
        {
            if(player.Gamepad != null)
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
        PlayerPrefs.Save();
    }
    public void SetMusicVolume(float value)
    {
        audioMixer.SetFloat(musicVolume, value);
        if (!onAwake)
        {
            if (player.Gamepad != null)
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
        PlayerPrefs.Save();
    }
    public void SetFXVolume(float value)
    {
        audioMixer.SetFloat(fxVolume, value);
        if (!onAwake)
        {
            if (player.Gamepad != null)
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
        PlayerPrefs.Save();
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

        source.clip = audioClips[0];
        source.Play();

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

        source.clip = audioClips[0];
        source.Play();

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

        source.clip = audioClips[0];
        source.Play();

        inventoryPanel.SetActive(false);
        mapPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    #region Button's SettingsTab


    public void Pause(int num)
    {
        pauseManager.isPaused = true;
        player.heedArrows = false;
        blackFade.SetActive(true);
        bookContainer.SetActive(true);

        if(num == 1)
        {
            InventoryTab();
        }
        else
        {
            MapTab();
        }
    }

    public void Resume()
    {
        pauseManager.isPaused = false;
        pauseManager.EventSystem.ffPause = false;
        player.heedArrows = true;
        blackFade.SetActive(false);
        bookContainer.SetActive(false);
        pauseManager.IsOnInventory = false;
        pauseManager.IsOnSettings = false;
        pauseManager.IsOnMap = false;
        rightOptions.SetActive(false);
        exitGameQuest.SetActive(false);
        goToMainMenuQuest.SetActive(false);
        bookContainer.SetActive(false);

        source.clip = audioClips[0];
        source.Play();
    }
    public void ClickOnSettings()
    {
        rightOptions.SetActive(true);
        for (int i = 0; i < rightOptions.transform.childCount; i++)
        {
            rightOptions.transform.GetChild(i).gameObject.SetActive(true);
        }

        source.clip = audioClips[0];
        source.Play();

        goToMainMenuQuest.SetActive(false);
        exitGameQuest.SetActive(false);
    }
    public void ClickOnMainMenu()
    {
        rightOptions.SetActive(false);
        exitGameQuest.SetActive(false);
        goToMainMenuQuest.SetActive(true);

        source.clip = audioClips[0];
        source.Play();
    }
    public void ClickOnExit()
    {
        rightOptions.SetActive(false);
        goToMainMenuQuest.SetActive(false);
        exitGameQuest.SetActive(true);

        source.clip = audioClips[0];
        source.Play();
    }

    public void YesMainMenu()
    {
        pauseManager.isPaused = false;
        AudioManager.instanceAudio.PlayerFirstMenuSong = false;
        SaveSystem.SavePlayerData(player, inventory, plAttack);
        SaveSystem.SaveSceneData(ScenesManager.scenesManager);
        SceneManager.LoadScene("MainMenuScene");


    }
    public void NoMainMenu()
    {
        goToMainMenuQuest.SetActive(false);
        pauseManager.EventSystem.SetearSelectedButton(askButtons[0]);

        source.clip = audioClips[0];
        source.Play();
    }

    public void YesExitGame()
    {
        pauseManager.isPaused = false;
        SaveSystem.SavePlayerData(player, inventory, plAttack);
        SaveSystem.SaveSceneData(ScenesManager.scenesManager);
        Application.Quit();
    }

    public void NoExitGame()
    {
        exitGameQuest.SetActive(false);

        pauseManager.EventSystem.SetearSelectedButton(askButtons[1]);
        source.clip = audioClips[0];
        source.Play();
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
