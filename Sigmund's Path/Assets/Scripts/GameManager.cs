using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private PlayerController2 plController2;
    private Vendedor vendedor;


    #region Vendedor
    public bool inShop;
    #endregion

    void Awake()
    {   if(GameObject.FindGameObjectsWithTag("GameManager").Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        plController2 = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController2>();
        vendedor = GameObject.FindGameObjectWithTag("Vendedor").GetComponent<Vendedor>();
    }
    void Update(){
        if(vendedor != null)
        {
            inShop = vendedor.inShop;
        }
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
        }
    }
}
