using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSystem : MonoBehaviour
{
    private LevelManager levelManager;
    [SerializeField] private GameObject[] masks;
    void Start()
    {
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        if(levelManager.levelScene -3 < masks.Length)
        {
            ScenesManager.scenesManager.unlokedZone[levelManager.levelScene - 3] = true;
            LoadMap();
        }
    }

    void LoadMap()
    {
        for (int i = 0; i < masks.Length; i++)
        {
            
            if (ScenesManager.scenesManager.unlokedZone[i])
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
