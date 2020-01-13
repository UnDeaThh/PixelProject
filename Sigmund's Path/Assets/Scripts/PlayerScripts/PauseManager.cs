using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PauseManager : MonoBehaviour
{
    //SINGLETON
    public static PauseManager pauseManager;

    public GameObject pausePanelBegins;

    public bool isPaused = false;
    public bool isOnInventory = false;
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
    public Slider sliderVolumen;
    public Dropdown qualityDropdown;
    public Toggle fullScreenToggle;

    public Dropdown resolutionDropdown;
    private Resolution[] resolutions = new Resolution[3];

    public GameObject options;
    public GameObject goToMainMenuQuest;
    public GameObject exitGameQuest;
    private void Awake()
    {
        if(pauseManager == null)
        {
            pauseManager = this;
        }
        else if(pauseManager != this)
        {
            Destroy(gameObject);
        }
        

        Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        if(canvas != null)
        {
            canvas.enabled = true;
        }
        if(pausePanel != null)
        {
            pausePanelBegins.SetActive(true);
        }
        isPaused = false;

       
        blackFade.enabled = false;
		pausePanel.SetActive(false);
        options.SetActive(false);
        exitGameQuest.SetActive(false);
        goToMainMenuQuest.SetActive(false);
        bookContainer.SetActive(false);
		mapPanel.SetActive(false);

        QualitySettings.SetQualityLevel(3); //Empezamos en Ultra

        sliderVolumen.value = PlayerPrefs.GetFloat("volume", 0);
        qualityDropdown.value = PlayerPrefs.GetInt("quality", 3);
        fullScreenToggle.isOn = intToBool(PlayerPrefs.GetInt("isFullScreen", 1));


        Debug.Log(Screen.currentResolution);
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

        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = PlayerPrefs.GetInt("resolution", 2);
        resolutionDropdown.RefreshShownValue();
    }

    private void Update()
    {
        if (!Vendedor.seller.inShop)
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
        if (isPaused)
            Time.timeScale = 0f;
        else 
            Time.timeScale = 1f;
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
        fullScreenToggle.isOn = isFullScreen;
        PlayerPrefs.SetInt( "isfullScreen", boolToInt(fullScreenToggle.isOn));
	}

    public void SetResolution(Dropdown dropdown)
    {
        Resolution resolution = resolutions[dropdown.value];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("resolution", resolutionDropdown.value);
        Debug.Log(Screen.currentResolution);

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
        GameManager.gameManager.ChangeScene(1);
    }

    public void NoMainMenu()
    {
        goToMainMenuQuest.SetActive(false);
    }

    public void YesExitGame()
    {
        isPaused = false;
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
