using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    [Header("DeadPanel")]
    public GameObject deadPanelUI;
    private float alphaSpeed = 0.02f;
    private float currentAlphaDeadPanel = 0.0f;

    #region Vendedor
    public bool inShop;
    #endregion

    void Awake()
    {
        if(gameManager == null)
        {
            gameManager = this;
        }
        else if(gameManager != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        if(deadPanelUI != null)
        {
            deadPanelUI.SetActive(false);
        }
        
    }

    void Update(){
        inShop = Vendedor.seller.inShop;
        DeadState();

    }

    void CursorController()
    {
        if (!PauseManager.pauseManager.isPaused)
        {
            //Lockea el cursor en medio de la pantalla y lo deja invisible
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
    void DeadState()
    {
        if (!PlayerController2.plController2.isDead)
        {
            Time.timeScale = 1f;
        }
        else
        {
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

    public void ChangeScene(int index)
    {
        if(index == 2)
        {
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(PlayerController.plContoller.gameObject);
        }
        SceneManager.LoadScene(index);
        
    }

}
