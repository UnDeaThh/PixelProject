using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager scenesManager;

    [SerializeField] GameManager GM;
    [SerializeField] PlayerController2 player;
    [SerializeField] Canvas canvas;
    [SerializeField] PauseManager pauseManager;

    public int[] gameplayScenes;
    [ SerializeField] private int actualScene;

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
        DontDestoyObjects();

    }

    public void ChangeScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    void DontDestoyObjects()
    {
        for (int i = 0; i < gameplayScenes.Length; i++)
        {
            if (actualScene == gameplayScenes[i])
            {
                DontDestroyOnLoad(GM.gameObject);
                DontDestroyOnLoad(player.gameObject);
                DontDestroyOnLoad(canvas.gameObject);
                DontDestroyOnLoad(pauseManager.gameObject);
            }
        }
    }
}
