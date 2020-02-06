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
}
