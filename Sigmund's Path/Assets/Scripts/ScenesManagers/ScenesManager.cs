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
    }

    private void Start()
    {
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
}
