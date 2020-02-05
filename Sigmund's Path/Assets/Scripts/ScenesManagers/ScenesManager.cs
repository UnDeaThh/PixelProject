using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager scenesManager;

    public int[] gameplayScenes;
    public int actualScene;

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
        actualScene = SceneManager.GetActiveScene().buildIndex;
    }

    private void Update()
    {
        actualScene = SceneManager.GetActiveScene().buildIndex;
    }

    public void ChangeScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
