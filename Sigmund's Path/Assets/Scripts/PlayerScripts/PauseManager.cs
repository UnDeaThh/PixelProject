using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    //SINGLETON
    PlayerInputs inputs;
    private PlayerController2 player;
    [SerializeField] EventSystemManager eventSystem;
    [SerializeField] DialogueManager dialogueManager;

    [SerializeField] GameObject dadCanvas;

    public bool isPaused = false;
    private bool isOnInventory = false;
    private bool isOnSettings = false;
    private bool isOnMap = false;
    public bool inShop = false;
    [SerializeField] GameObject blackFade;
    
    [SerializeField] GameObject bookContainer;
    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject inventoryPanel;
    [SerializeField] GameObject mapPanel;

	[Header("PAUSE SETTINGS")]

    //[SerializeField] Dropdown qualityDropdown;

    [SerializeField] GameObject rightOptions;
    [SerializeField] GameObject goToMainMenuQuest;
    [SerializeField] GameObject exitGameQuest;

    public EventSystemManager EventSystem { get => eventSystem; set => eventSystem = value; }
    public bool IsOnSettings { get => isOnSettings; set => isOnSettings = value; }
    public bool IsOnInventory { get => isOnInventory; set => isOnInventory = value; }
    public bool IsOnMap { get => isOnMap; set => isOnMap = value; }

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
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController2>();
    }

    private void Update()
    {
        if (dialogueManager)
        {
            if (dialogueManager.talking)
            {
                dadCanvas.SetActive(false);
            }
            else
            {
                dadCanvas.SetActive(true);
            }
        }
        //PAUSE MANAGEMENT
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

        OpenInventory();
    }

    public void OpenInventory()
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
    #region Ajustes


    public void SetQuality(Dropdown dropdown)
    {
        QualitySettings.SetQualityLevel(dropdown.value);
        PlayerPrefs.SetInt("quality", dropdown.value);
    }
    #endregion
}
