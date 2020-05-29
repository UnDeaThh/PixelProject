﻿using System.Collections;
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
    private bool apearsOnFountain = false;

    private bool[] palancasState = new bool[10];
    private bool[] heartsPickUp = new bool[10];
    private bool[] cumuloState = new bool[20];
    private bool[] unlokedZone = new bool[26];
    private bool[] bossKilled = new bool[3];
    private bool[] destruibleWall = new bool[50];
    private bool[] fountainUnlocked = new bool[5];
    public bool cutSceneDone = false;
    private bool swordPicked;
    private bool firstTalkAska;
    private bool[] shopUnlocked = new bool[3];

    public bool SwordPicked { get => swordPicked; set => swordPicked = value; }
    public bool FirstTalkAska { get => firstTalkAska; set => firstTalkAska = value; }
    public bool[] DestruibleWall { get => destruibleWall; set => destruibleWall = value; }
    public bool[] BossKilled { get => bossKilled; set => bossKilled = value; }
    public bool[] UnlokedZone { get => unlokedZone; set => unlokedZone = value; }
    public bool[] FountainUnlocked { get => fountainUnlocked; set => fountainUnlocked = value; }
    public bool ApearsOnFountain { get => apearsOnFountain; set => apearsOnFountain = value; }
    public bool[] ShopUnlocked { get => shopUnlocked; set => shopUnlocked = value; }
    public bool[] PalancasState { get => palancasState; set => palancasState = value; }
    public bool[] HeartsPickUp { get => heartsPickUp; set => heartsPickUp = value; }
    public bool[] CumuloState { get => cumuloState; set => cumuloState = value; }

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
        if(toLoadScene != 0 && actualScene == 2)
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

            for (int i = 0; i < cumuloState.Length; i++)
            {
                cumuloState[i] = data.cumuloState[i];
            }

            for (int i = 0; i < unlokedZone.Length; i++)
            {
                unlokedZone[i] = data.unlokedZone[i];
            }
            for (int i = 0; i < fountainUnlocked.Length; i++)
            {
                fountainUnlocked[i] = data.fountainUnlocked[i];
            }
            for (int i = 0; i < destruibleWall.Length; i++)
            {
                destruibleWall[i] = data.destruibleWall[i];
            }
            for (int i = 0; i < bossKilled.Length; i++)
            {
                bossKilled[i] = data.bossKilled[i];
            }
            for (int i = 0; i < shopUnlocked.Length; i++)
            {
                shopUnlocked[i] = data.shopUnlocked[i];
            }
            SwordPicked = data.swordPicked;
            FirstTalkAska = data.firstTalkAska;
        }
        else
        {
            Debug.Log("Scene data empty");
            for (int i = 0; i < palancasState.Length; i++)
            {
                palancasState[i] = false;
            }
            for (int i = 0; i < heartsPickUp.Length; i++)
            {
                heartsPickUp[i] = false;
            }
            for (int i = 0; i < cumuloState.Length; i++)
            {
                cumuloState[i] = false;
            }
            for (int i = 0; i < unlokedZone.Length; i++)
            {
                unlokedZone[i] = false;
            }
            for (int i = 0; i < fountainUnlocked.Length; i++)
            {
                fountainUnlocked[i] = false;
            }
            for (int i = 0; i < destruibleWall.Length; i++)
            {
                destruibleWall[i] = false;
            }
            for (int i = 0; i < bossKilled.Length; i++)
            {
                bossKilled[i] = false;
            }
            for (int i = 0; i < shopUnlocked.Length; i++)
            {
                shopUnlocked[i] = false;
            }

            SwordPicked = false;
            FirstTalkAska = false;
            toLoadScene = 0;
            cutSceneDone = false;
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
