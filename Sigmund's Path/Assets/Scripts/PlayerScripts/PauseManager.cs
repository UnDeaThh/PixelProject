﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    private PlayerController plContoller;
    [HideInInspector] public bool isPaused = false;
    private bool isOnInventory = false;
    private bool isOnPause = false;
	private bool isOnMap = false;
    [Header("UI PAUSE")]
    public Image blackFade;
    public GameObject bookContainer;
	public GameObject pausePanel;
    public GameObject inventoryPanel;
	public GameObject mapPanel;

	[Header("PAUSE SETTINGS")]
	public AudioMixer audioMixer;
    public Dropdown resolutionDropdown;
    public Slider sliderVolumen;
    private Resolution[] resolutions = new Resolution[3];
    private int currentResolutionIndex = 0;

    public GameObject options;
    public GameObject goToMainMenuQuest;
    public GameObject exitGameQuest;
    private void Awake()
    {
        plContoller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        blackFade.enabled = false;
		pausePanel.SetActive(false);
        options.SetActive(false);
        exitGameQuest.SetActive(false);
        goToMainMenuQuest.SetActive(false);
        bookContainer.SetActive(false);
		mapPanel.SetActive(false);

        for(int i = 0; i < Screen.resolutions.Length; i++)
        {
            Debug.Log(Screen.resolutions[i]);
        }

        QualitySettings.SetQualityLevel(3); //Empezamos en Ultra

        sliderVolumen.value = PlayerPrefs.GetFloat("volume", 0);
        
    }

    private void Start()
    {
        resolutions[0] = Screen.resolutions[0]; //640 x 480
        resolutions[1] = Screen.resolutions[6]; //1280 x 720
        resolutions[2] = Screen.resolutions[Screen.resolutions.Length - 1]; // La Maxima resolucion
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height) //Esto en teoria esta mal segun el comentario del video
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
				
            }
        }
    }

    public void Resume()
    {
        isPaused = false;
        blackFade.enabled = false;
        bookContainer.SetActive(false);
        isOnPause = false;
        isOnInventory = false;
		isOnMap = false;
        options.SetActive(false);
        exitGameQuest.SetActive(false);
        goToMainMenuQuest.SetActive(false);
        Time.timeScale = 1f;
		bookContainer.SetActive(false);
    }

    public void Pause()
    {
        isPaused = true;
        blackFade.enabled = true;
		Time.timeScale = 0f;
        bookContainer.SetActive(true);

        InventoryTab();
    }

    public void InventoryTab()
    {
        isOnPause = false;
        options.SetActive(false);
        exitGameQuest.SetActive(false);
        goToMainMenuQuest.SetActive(false);
        isOnMap = false;
        isOnInventory = true;
        pausePanel.SetActive(false);
		mapPanel.SetActive(false);
        inventoryPanel.SetActive(true);
    }


    public void PauseTab()
    {
        isOnInventory = false;
		isOnMap = false;
        isOnPause = true;
        inventoryPanel.SetActive(false);
		mapPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

	public void MapTab(){
		isOnInventory = false;
		isOnPause =  false;
        options.SetActive(false);
        exitGameQuest.SetActive(false);
        goToMainMenuQuest.SetActive(false);
        isOnMap = true;
		inventoryPanel.SetActive(false);
		pausePanel.SetActive(false);
		mapPanel.SetActive(true);
	}

    #region SettingsButtons
    public void ClickOnSettings()
    {
        options.SetActive(true);
        goToMainMenuQuest.SetActive(false);
        exitGameQuest.SetActive(false);
    }

    public void ClickOnMainMenu()
    {
        options.SetActive(false);
        exitGameQuest.SetActive(false);
        goToMainMenuQuest.SetActive(true);

    }

    public void ClickOnExitGame()
    {
        options.SetActive(false);
        goToMainMenuQuest.SetActive(false);
        exitGameQuest.SetActive(true);
    }
    #endregion

    #region Ajustes
    public void SetVolume (Slider slider)
	{
		audioMixer.SetFloat("volume", slider.value);
        PlayerPrefs.SetFloat("volume", slider.value);
	}

	public void SetFullScreen(bool isFullScreen)
	{
		Screen.fullScreen = isFullScreen;
	}

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetFloat("widthResolution", resolution.width);
        PlayerPrefs.SetFloat("heightResolution", resolution.height);

    }

    public void SetQuality(Dropdown dropdown)
    {
        QualitySettings.SetQualityLevel(dropdown.value);
        PlayerPrefs.SetInt("qualityLevel",dropdown.value);
    }
    #endregion

    public void YesMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void NoMainMenu()
    {
        goToMainMenuQuest.SetActive(false);
    }
}
