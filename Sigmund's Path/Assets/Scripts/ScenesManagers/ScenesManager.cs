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
    public List<bool> palancasState = new List<bool>();
    public List<bool> heartsPickUp = new List<bool>();
    public bool cutSceneDone = false;
    public bool swordPicked;
    public bool firstTalkAska;

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
        Debug.Log(heartsPickUp.Count);
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

    public void ChangeScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    private void LoadSceneManager()
    {
        ScenesData data = SaveSystem.LoadSceneData();
        if(data != null)
        {
            toLoadScene = data.toLoadScene;
            cutSceneDone = data.cutSceneDone;
            if(palancasState.Count != 0)
            {
                for (int i = 0; i < palancasState.Count; i++)
                {
                    palancasState[i] = data.palancasState[i];
                }
            }
            if (heartsPickUp.Count > 0) 
            {
                for (int i = 0; i < heartsPickUp.Count; i++)
                {
                    heartsPickUp[i] = data.heartsPickUp[i];
                }
            }
            swordPicked = data.swordPicked;
            firstTalkAska = data.firstTalkAska;
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
