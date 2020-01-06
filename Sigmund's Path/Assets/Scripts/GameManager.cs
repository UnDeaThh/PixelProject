using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private PlayerController plController;
    [Header("DeadPanel")]
    public GameObject deadPanelUI;
    private float alphaSpeed = 0.01f;
    private float currentAlphaDeadPanel = 0.0f;
    private PauseManager pauseManager;

    void Awake(){
        plController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        pauseManager = GameObject.FindGameObjectWithTag("PauseManager").GetComponent<PauseManager>();
        deadPanelUI.SetActive(false);
    }

    void Update(){

        if (!pauseManager.isPaused)
        {
            //Lockea el cursor en medio de la pantalla y lo deja invisible
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }

        if(!plController.isDead){
            Time.timeScale = 1f;
        }
        else{
            Time.timeScale = 0.5f;
            DeadPanelAppears();
        }
    }

    void DeadPanelAppears(){
        deadPanelUI.SetActive(true);
        if(currentAlphaDeadPanel >= 1f){
            currentAlphaDeadPanel = 1f;
        }
        else{
            currentAlphaDeadPanel += alphaSpeed;
        }
        Image deadImage = deadPanelUI.GetComponent<Image>();
        deadImage.color = new Color(0f, 0f, 0f, currentAlphaDeadPanel);
    }

}
