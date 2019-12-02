using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayManager : MonoBehaviour
{
    private PlayerController plContoller;
    [HideInInspector] public bool isPause = false;
	public GameObject pausePanel;


    private void Awake()
    {
        plContoller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		pausePanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPause)
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
        isPause = false;
		Time.timeScale = 1f;
		pausePanel.SetActive(false);
    }

    public void Pause()
    {
        isPause = true;
		Time.timeScale = 0f;
		pausePanel.SetActive(true);
    }
}
