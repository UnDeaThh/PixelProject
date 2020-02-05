using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager gameManager;

    private PlayerController2 plController2;
    private Vendedor vendedor;
	private PauseManager pauseManager;


    #region Vendedor
    public bool inShop;
    #endregion

    void Awake()
    { 

    }

    private void Start()
    {
        plController2 = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController2>();
        if(ScenesManager.scenesManager.actualScene == 7)
        {
            vendedor = GameObject.FindGameObjectWithTag("Vendedor").GetComponent<Vendedor>();
        }
    }
    void Update(){
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
        if (!pauseManager.isPaused)
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
        }
    }
}
