using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSystem : MonoBehaviour
{
    private LevelManager levelManager;
    [SerializeField] private GameObject[] masks;
    [SerializeField] GameObject playerIcon;
    [SerializeField] Transform[] scenesPositions;
    void Start()
    {
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        if(levelManager.levelScene -4 < masks.Length)
        {
            ScenesManager.scenesManager.UnlokedZone[levelManager.levelScene - 4] = true;
            LoadMap();
        }

        playerIcon.transform.position = scenesPositions[levelManager.levelScene - 4].position;
    }

    void LoadMap()
    {
        for (int i = 0; i < masks.Length; i++)
        {
            
            if (ScenesManager.scenesManager.UnlokedZone[i])
            {
                masks[i].SetActive(false);
            }
            else
            {
                masks[i].SetActive(true);
            }
            
        }
    }
}
