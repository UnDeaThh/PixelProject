using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GamePlayManager : MonoBehaviour
{
    private PlayerController plContoller;
    [HideInInspector] public bool isPaused = false;
    private bool isOnInventory = false;
    private bool isOnPause = false;
	private bool isOnMap = false;
    [Header("UI PAUSE")]
    public GameObject bookContainer;
	public GameObject pausePanel;
    public GameObject inventoryPanel;
	public GameObject mapPanel;

	[Header("PAUSE SETTINGS")]
	public AudioMixer audioMixer;


    private void Awake()
    {
        plContoller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		pausePanel.SetActive(false);
        bookContainer.SetActive(false);
		mapPanel.SetActive(false);
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
        bookContainer.SetActive(false);
        isOnPause = false;
        isOnInventory = false;
		isOnMap = false;
		Time.timeScale = 1f;
		bookContainer.SetActive(false);
    }

    public void Pause()
    {
        isPaused = true;
		Time.timeScale = 0f;
        bookContainer.SetActive(true);

        InventoryTab();


    }

    public void InventoryTab()
    {
        isOnPause = false;
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
		isOnMap = true;
		inventoryPanel.SetActive(false);
		pausePanel.SetActive(false);
		mapPanel.SetActive(true);
	}

	public void SetVolume (float volume)
	{
		Debug.Log(volume);
		audioMixer.SetFloat("volume", volume);
	}

	public void SetFullScreen(bool isFullScreen)
	{
		Screen.fullScreen = isFullScreen;
	}
}
