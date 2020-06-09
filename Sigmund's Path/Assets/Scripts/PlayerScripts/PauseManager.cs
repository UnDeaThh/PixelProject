using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Cinemachine;

public class PauseManager : MonoBehaviour
{
    Settings settings;
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

    //[SerializeField] Dropdown qualityDropdown;
    [SerializeField] AudioMixerSnapshot unPausedSnapshot;
    [SerializeField] AudioMixerSnapshot pausedSnapshot;

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
        settings = GameObject.FindObjectOfType<Settings>();
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
                    unPausedSnapshot.TransitionTo(0.1f);
                    settings.Resume();
                }
                else
                {
                    settings.Pause(1);
                    pausedSnapshot.TransitionTo(0.1f);
                }
            }
            if (inputs.Controls.Map.triggered)
            {
                if (isPaused)
                {
                    unPausedSnapshot.TransitionTo(0.1f);
                    settings.Resume();
                }
                else
                {
                    settings.Pause(2);
                    pausedSnapshot.TransitionTo(0.1f);
                }
            }
        }
    }
    #region Ajustes


    public void SetQuality(Dropdown dropdown)
    {
        QualitySettings.SetQualityLevel(dropdown.value);
        PlayerPrefs.SetInt("quality", dropdown.value);
    }
    #endregion
}
