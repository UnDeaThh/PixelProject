using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyerManager : MonoBehaviour
{
    private static DestroyerManager destroyerManager;

    private GameManager GM;
    private PlayerController2 player;
    public int[] gameplayScenes;
    private int actualScene;

    private void Awake()
    {
        if(destroyerManager == null)
        {
            destroyerManager = this;
        }
        if(destroyerManager != this)
        {
            Destroy(gameObject);
        }
        actualScene = SceneManager.GetActiveScene().buildIndex;
    }

    private void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController2>();

    }

    private void Update()
    {
        actualScene = SceneManager.GetActiveScene().buildIndex;

        for (int i = 0; i < gameplayScenes.Length; i++)
        {
            if (actualScene == gameplayScenes[i])
            {
                DontDestroyOnLoad(GM.gameObject);
                DontDestroyOnLoad(player.gameObject);
            }
        }
    }
}
