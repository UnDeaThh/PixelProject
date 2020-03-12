using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    //SINGLETON
    PlayerInputs inputs;
    private PlayerController2 player;
    private Inventory2 inventory;
    private PlayerAttack plAttack;
    [SerializeField] EventSystemManager eventSystem;

    [SerializeField] GameObject dadCanvas;

    public bool isPaused = false;
    public bool isOnInventory = false;
    public bool isOnSettings = false;
	public bool isOnMap = false;
    public bool inShop = false;
    [Header("UI PAUSE")]
    [SerializeField] GameObject blackFade;
    
    [SerializeField] GameObject bookContainer;
    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject inventoryPanel;
    [SerializeField] GameObject mapPanel;

	[Header("PAUSE SETTINGS")]
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider sliderVolumen;
    [SerializeField] Toggle fullScreenToggle;
    [SerializeField] Dropdown qualityDropdown;
    [SerializeField] Dropdown resolutionDropdown;
    private Resolution[] resolutions = new Resolution[3];

    [SerializeField] GameObject rightOptions;
    [SerializeField] GameObject goToMainMenuQuest;
    [SerializeField] GameObject exitGameQuest;

    private void OnEnable()
    {
        inputs.Controls.Enable();
    }
    private void OnDisable()
    {
        inputs.Controls.Disable();
    }
    private void Awake()
    {
        inputs = new PlayerInputs();
        dadCanvas.SetActive(true);
        isPaused = false;


        blackFade.SetActive(false);
		settingsPanel.SetActive(false);
        rightOptions.SetActive(false);
        exitGameQuest.SetActive(false);
        goToMainMenuQuest.SetActive(false);
        bookContainer.SetActive(false);
		mapPanel.SetActive(false);

        QualitySettings.SetQualityLevel(3); //Empezamos en Ultra

        sliderVolumen.value = PlayerPrefs.GetFloat("volume", 0);
        qualityDropdown.value = PlayerPrefs.GetInt("quality", 3);
        fullScreenToggle.isOn = intToBool(PlayerPrefs.GetInt("isFullScreen", 1));
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController2>();
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory2>();
        plAttack = player.gameObject.GetComponent<PlayerAttack>();

        resolutions[0] = Screen.resolutions[0]; //640 x 480
        resolutions[1] = Screen.resolutions[6]; //1280 x 720
        resolutions[2] = Screen.resolutions[Screen.resolutions.Length - 1]; // La Maxima resolucion
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = PlayerPrefs.GetInt("resolution", 2);
        resolutionDropdown.RefreshShownValue();
    }

    private void Update()
    {
        if (!player.isOnKinematic)
        {
            if (inputs.Controls.Pause.triggered)
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
        
        if (isPaused)
        {
            Time.timeScale = 0f;
        }
        else 
            Time.timeScale = 1f;
    }

    public void Resume()
    {
        isPaused = false;
        eventSystem.ffPause = false;
        player.heedArrows = true;
        blackFade.SetActive(false);
        bookContainer.SetActive(false);
        isOnSettings = false;
        isOnInventory = false;
		isOnMap = false;
        rightOptions.SetActive(false);
        exitGameQuest.SetActive(false);
        goToMainMenuQuest.SetActive(false);
        Time.timeScale = 1f;
		bookContainer.SetActive(false);
    }

    public void Pause()
    {
        isPaused = true;
        player.heedArrows = false;
        blackFade.SetActive(true);
		Time.timeScale = 0f;
        bookContainer.SetActive(true);

        InventoryTab();
    }

    public void InventoryTab()
    {
        eventSystem.ffPause = false;
        isOnSettings = false;
        rightOptions.SetActive(false);
        exitGameQuest.SetActive(false);
        goToMainMenuQuest.SetActive(false);
        isOnMap = false;
        isOnInventory = true;
        settingsPanel.SetActive(false);
		mapPanel.SetActive(false);
        inventoryPanel.SetActive(true);
    }

    public void PauseTab()
    {
        eventSystem.ffPause = false;
        isOnInventory = false;
		isOnMap = false;
        isOnSettings = true;
        inventoryPanel.SetActive(false);
		mapPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

	public void MapTab(){
        eventSystem.ffPause = false;
        isOnInventory = false;
		isOnSettings =  false;
        rightOptions.SetActive(false);
        exitGameQuest.SetActive(false);
        goToMainMenuQuest.SetActive(false);
        isOnMap = true;
		inventoryPanel.SetActive(false);
		settingsPanel.SetActive(false);
		mapPanel.SetActive(true);
	}

    #region SettingsButtons
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

    public void ClickOnExitGame()
    {
        rightOptions.SetActive(false);
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
        fullScreenToggle.isOn = isFullScreen;
        PlayerPrefs.SetInt("isFullScreen", boolToInt(fullScreenToggle.isOn));
    }

    public void SetResolution(Dropdown dropdown)
    {
        Resolution resolution = resolutions[dropdown.value];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("resolution", resolutionDropdown.value);

    }

    public void SetQuality(Dropdown dropdown)
    {
        QualitySettings.SetQualityLevel(dropdown.value);
        PlayerPrefs.SetInt("quality", dropdown.value);
    }
    #endregion

    public void YesMainMenu()
    {
        isPaused = false;
        SaveSystem.SavePlayerData(player, inventory, plAttack);
        ScenesManager.scenesManager.ChangeScene(1);
    }

    public void NoMainMenu()
    {
        goToMainMenuQuest.SetActive(false);
    }

    public void YesExitGame()
    {
        isPaused = false;
        SaveSystem.SavePlayerData(player, inventory, plAttack);
        Application.Quit();
    }

    public void NoExitGame()
    {
        exitGameQuest.SetActive(false);
    }


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
        if(val != 0)
            return true;
        else
            return false;
    }
    #endregion
}
