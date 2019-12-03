using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayManager : MonoBehaviour
{
    private PlayerController plContoller;
    [HideInInspector] public bool isPaused = false;
    private bool isOnInventory = false;
    private bool isOnPause = false;
    [Header("UI PAUSE")]
    public GameObject bookContainer;
	public GameObject pausePanel;
    public GameObject inventoryPanel;


    private void Awake()
    {
        plContoller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		pausePanel.SetActive(false);
        bookContainer.SetActive(false);
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
        isOnInventory = true;
        inventoryPanel.SetActive(true);
        pausePanel.SetActive(false);
    }

    public void PauseTab()
    {
        isOnInventory = false;
        isOnPause = true;
        inventoryPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

}
