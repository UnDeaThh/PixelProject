using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager scenesManager;

 
    public int actualScene;
    public int toLoadScene = 0;

    public bool comeFromDead = false;
    public bool apearsOnFountain = false;
    public bool[] palancasState = new bool[10];
    public bool[] heartsPickUp = new bool[10];
    public bool cutSceneDone = false;
    private bool swordPicked;
    private bool firstTalkAska;

    public bool SwordPicked { get => swordPicked; set => swordPicked = value; }
    public bool FirstTalkAska { get => firstTalkAska; set => firstTalkAska = value; }

    private void Awake()
    {
        if(scenesManager == null)
        {
            scenesManager = this;
        }
        if(scenesManager != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        LoadSceneManager();
    }
    private void Update()
    {
        actualScene = SceneManager.GetActiveScene().buildIndex;
        if(toLoadScene != 0 && actualScene == 1)
        {
            apearsOnFountain = true;
        }

        CursorController();
    }

    private void LoadSceneManager()
    {
        ScenesData data = SaveSystem.LoadSceneData();
        if(data != null)
        {
            toLoadScene = data.toLoadScene;
            cutSceneDone = data.cutSceneDone;
  
            for (int i = 0; i < palancasState.Length; i++)
            {
                palancasState[i] = data.palancasState[i];
            }
            
            for (int i = 0; i < heartsPickUp.Length; i++)
            {
                heartsPickUp[i] = data.heartsPickUp[i];
            }
            
            SwordPicked = data.swordPicked;
            FirstTalkAska = data.firstTalkAska;
        }
        else
        {
            for (int i = 0; i < palancasState.Length; i++)
            {
                palancasState[i] = false;
            }
            for (int i = 0; i < heartsPickUp.Length; i++)
            {
                heartsPickUp[i] = false;
            }
        }

    }

    private void CursorController()
    {
        if(actualScene == 1 || actualScene == 2)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void PalancasSaveSystem()
    {

    }
}
