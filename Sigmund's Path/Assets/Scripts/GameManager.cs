using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private PlayerController2 plController2;

    [Header("DeadPanel")]
    public GameObject deadPanelUI;
    private float alphaSpeed = 0.02f;
    private float currentAlphaDeadPanel = 0.0f;

    #region Vendedor
    public bool inShop;
    #endregion

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        plController2 = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController2>();
        if (deadPanelUI != null)
        {
            deadPanelUI.SetActive(false);
        }
    }
    void Update(){
        inShop = Vendedor.seller.inShop;
        DeadState();

        AbilitiesGODControl();
    }

    void AbilitiesGODControl()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            plController2.dashUnlocked = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            plController2.highJumpUnlocked = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            plController2.wallJumpUnlocked = true;
        }
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
        if (!plController2.isDead)
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
        SceneManager.LoadScene(index);     
    }

}
