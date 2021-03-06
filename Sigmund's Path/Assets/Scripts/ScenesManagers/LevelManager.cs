﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private PlayerController2 player;
    private Inventory2 inventory;
    private PlayerAttack plAttack;
    [SerializeField] Transform[] apearsPos;
    private PauseManager pauseManager;
    public Transform fountainPos;
    [SerializeField] GameObject listener;

    public int levelScene;

    private void Awake()
    {
        for (int i = 0; i < apearsPos.Length; i++)
        {
            apearsPos[i].parent.transform.gameObject.SetActive(true);
        }

        levelScene += 3;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController2>();
        plAttack = player.gameObject.GetComponent<PlayerAttack>();
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory2>();
        pauseManager = GameObject.FindGameObjectWithTag("PauseManager").GetComponent<PauseManager>();

        //Carga la player Info
        LoadPlayer();

        if (ScenesManager.scenesManager.comeFromDead)
        {
            inventory.actualMoney = inventory.actualMoney / 2;
            ScenesManager.scenesManager.comeFromDead = false;
        }
        //POSICIONA AL PLAYER
        PlayerPosition();
    }

    private void Start()
    {
        Instantiate(listener, player.transform.position, Quaternion.identity);
        ResetAudioManager();
    }

    void PlayerPosition()
    {
        
        if (ScenesManager.scenesManager.CutSceneDone)
        {
            if (ScenesManager.scenesManager.ApearsOnFountain)
            {
                if (fountainPos)
                {
                    player.gameObject.transform.position = fountainPos.position;  //Si usamos el TP aparecemos en la fuente;
                    player.facingDir = 1;
                    ScenesManager.scenesManager.ApearsOnFountain = false;
                }
            }
            else
            {
                if (apearsPos.Length <= 2)
                {
                    if(apearsPos.Length == 1)
                    {
                        if(levelScene == 29) // Nerbuz Scene
                        {
                            player.gameObject.transform.position = apearsPos[0].position;
                            player.facingDir = -1;

                        }
                        else if(levelScene == 25) // Blemmis Scene
                        {
                            player.gameObject.transform.position = apearsPos[0].position;
                            player.facingDir = 1; 
                        }
                        else if(levelScene == 11)
                        {
                            player.gameObject.transform.position = apearsPos[0].position;
                            player.facingDir = -1;
                        }
                        else if(levelScene == 13) // Primer Boss
                        {
                            player.gameObject.transform.position = apearsPos[0].position;
                            player.facingDir = -1;
                        }
                    }
                    else
                    {
                        //HAY EXCEPCION EN G17 Y G19
                        if (levelScene == 26) //G17
                        {
                            if (player.lastScene <= levelScene)
                            {
                                player.gameObject.transform.position = apearsPos[0].position;
                                player.facingDir = -1;
                            }
                            else
                            {
                                player.gameObject.transform.position = apearsPos[1].position;
                                player.facingDir = 1;
                            }
                        }
                        else if (levelScene == 28) // G19
                        {
                            if (player.lastScene <= levelScene)
                            {
                                player.gameObject.transform.position = apearsPos[0].position;
                                player.facingDir = -1;
                            }
                            else
                            {
                                player.gameObject.transform.position = apearsPos[1].position;
                                player.facingDir = 1;
                            }
                        }
                        else if(levelScene  == 20)
                        {
                            if(player.lastScene <= levelScene)
                            {
                                player.gameObject.transform.position = apearsPos[0].position;
                                player.facingDir = -1;
                            }
                            else
                            {
                                player.gameObject.transform.position = apearsPos[1].position;
                                player.facingDir = 1;
                            }
                        }

                        else
                        {
                            if (player.lastScene <= levelScene)
                            {
                                player.gameObject.transform.position = apearsPos[0].position;
                                player.facingDir = 1;
                            }
                            else
                            {
                                player.gameObject.transform.position = apearsPos[1].position;
                                player.facingDir = -1;
                            }
                        }
                    }
                }
                else
                {
                    if (levelScene == player.lastScene)
                    {
                        player.gameObject.transform.position = apearsPos[0].position;
                        player.facingDir = 1;
                    }

                    else if (levelScene == 8)
                    {
                        switch (player.lastScene)
                        {
                            case 7:
                                player.gameObject.transform.position = apearsPos[0].position;
                                player.facingDir = 1;
                                break;
                            case 9:
                                player.gameObject.transform.position = apearsPos[3].position;
                                player.facingDir = 1;
                                break;
                            case 10:
                                player.gameObject.transform.position = apearsPos[2].position;
                                player.facingDir = 1;
                                break;
                            default:
                                player.gameObject.transform.position = apearsPos[1].position;
                                player.facingDir = -1;
                                break;
                        }
                    }
                    else if(levelScene == 9)
                    {
                        switch (player.lastScene)
                        {
                            case 8:
                                player.gameObject.transform.position = apearsPos[0].position;
                                player.facingDir = -1;
                                break;
                            case 10:
                                player.gameObject.transform.position = apearsPos[1].position;
                                player.facingDir = 1;
                                break;
                            case 11:
                                player.gameObject.transform.position = apearsPos[2].position;
                                player.facingDir = 1;
                                break;
                            default:
                                player.gameObject.transform.position = apearsPos[3].position;
                                player.facingDir = -1;
                                break;
                        }
                    }
                    else if(levelScene == 10)
                    {
                        switch (player.lastScene)
                        {
                            case 9:
                                player.gameObject.transform.position = apearsPos[0].position;
                                player.facingDir = -1;
                                break;
                            case 8:
                                player.gameObject.transform.position = apearsPos[1].position;
                                player.facingDir = -1;
                                break;
                            case 13:
                                player.gameObject.transform.position = apearsPos[2].position;
                                player.facingDir = 1;
                                break;
                            default:
                                player.gameObject.transform.position = apearsPos[0].position;
                                player.facingDir = -1;
                                break;
                        }
                    }
                    else if(levelScene == 12)
                    {
                        switch (player.lastScene)
                        {
                            case 9:
                                player.gameObject.transform.position = apearsPos[0].position;
                                player.facingDir = 1;
                                break;
                            case 22:
                                player.gameObject.transform.position = apearsPos[1].position;
                                player.facingDir = -1;
                                break;
                            case 27:
                                player.gameObject.transform.position = apearsPos[2].position;
                                player.facingDir = 1;
                                break;
                            default:
                                player.gameObject.transform.position = apearsPos[0].position;
                                player.facingDir = 1;
                                break;
                        }
                    }
                    else if(levelScene == 16)
                    {
                        switch (player.lastScene)
                        {
                            case 15:
                                player.gameObject.transform.position = apearsPos[0].position;
                                player.facingDir = 1;
                                break;
                            case 17:
                                player.gameObject.transform.position = apearsPos[1].position;
                                player.facingDir = -1;
                                break;
                            case 21:
                                player.gameObject.transform.position = apearsPos[2].position;
                                player.facingDir = -1;
                                break;
                            default:
                                player.gameObject.transform.position = apearsPos[0].position;
                                player.facingDir = 1;
                                break;
                        }
                    }
                    else if(levelScene == 18)
                    {
                        switch (player.lastScene)
                        {
                            case 17:
                                player.gameObject.transform.position = apearsPos[0].position;
                                player.facingDir = 1;
                                break;
                            case 19:
                                player.gameObject.transform.position = apearsPos[1].position;
                                player.facingDir = -1;
                                break;
                            case 21:
                                player.gameObject.transform.position = apearsPos[2].position;
                                player.facingDir = 1;
                                break;
                            default:
                                player.gameObject.transform.position = apearsPos[0].position;
                                player.facingDir = 1;
                                break;
                        }
                    }
                    else if(levelScene == 21)
                    {
                        switch (player.lastScene)
                        {
                            case 20:
                                player.gameObject.transform.position = apearsPos[0].position;
                                player.facingDir = -1;
                                break;
                            case 18:
                                player.gameObject.transform.position = apearsPos[1].position;
                                player.facingDir = -1;
                                break;
                            case 16:
                                player.gameObject.transform.position = apearsPos[2].position;
                                player.facingDir = 1;
                                break;
                            case 23:
                                player.gameObject.transform.position = apearsPos[3].position;
                                player.facingDir = 1;
                                break;
                            default:
                                player.gameObject.transform.position = apearsPos[0].position;
                                player.facingDir = -1;
                                break;
                        }
                    }
                    else if(levelScene == 23)
                    {
                        switch (player.lastScene)
                        {
                            case 21:
                                player.gameObject.transform.position = apearsPos[0].position;
                                player.facingDir = -1;
                                break;
                            case 22:
                                player.gameObject.transform.position = apearsPos[1].position;
                                player.facingDir = 1;
                                break;
                            case 24:
                                player.gameObject.transform.position = apearsPos[2].position;
                                player.facingDir = -1;
                                break;
                            default:
                                player.gameObject.transform.position = apearsPos[0].position;
                                player.facingDir = -1;
                                break;
                        }
                    }
                    else if(levelScene == 24)
                    {
                        switch (player.lastScene)
                        {
                            case 23:
                                player.gameObject.transform.position = apearsPos[0].position;
                                player.facingDir = 1;
                                break;
                            case 25:
                                player.gameObject.transform.position = apearsPos[1].position;
                                player.facingDir = -1;
                                break;
                            case 26:
                                player.gameObject.transform.position = apearsPos[2].position;
                                player.facingDir = 1;
                                break;
                            default:
                                player.gameObject.transform.position = apearsPos[0].position;
                                player.facingDir = 1;
                                break;
                        }
                    }
                    else if(levelScene == 27)
                    {
                        switch (player.lastScene)
                        {
                            case 26:
                                player.gameObject.transform.position = apearsPos[0].position;
                                player.facingDir = -1;
                                break;
                            case 12:
                                player.gameObject.transform.position = apearsPos[1].position;
                                player.facingDir = -1;
                                break;
                            case 28:
                                player.gameObject.transform.position = apearsPos[2].position;
                                player.facingDir = 1;
                                break;
                            default:
                                player.gameObject.transform.position = apearsPos[0].position;
                                player.facingDir = -1;
                                break;
                        }
                    }
                }
            }
            #region Old Appears System
            /*
            else
            {
                if (apearsPos.Length > 0)
                {
                    if (levelScene == 3) //ESTANDO EN T1
                    {
                        if (player.lastScene > 3) //vienes de T2
                        {
                            player.facingDir = -1;
                            player.gameObject.transform.position = apearsPos[1].position;
                        }
                        else
                        {
                            player.gameObject.transform.position = apearsPos[0].position;
                            player.facingDir = 1;
                        }
                    }
                    else if (levelScene == 4) //ESTANDO EN T2
                    {
                        if (player.lastScene == 3)//vienes de T1
                        {
                            player.gameObject.transform.position = apearsPos[0].position;
                        }
                        else if (player.lastScene == 4)
                        {
                            player.gameObject.transform.position = apearsPos[0].position;
                        }
                        else if (player.lastScene == 5)//vienes de T3
                        {
                            player.gameObject.transform.position = apearsPos[1].position;
                        }
                    }
                    else if (levelScene == 5) //ESTANDO EN T3
                    {
                        if (player.lastScene == 4) //vienes de T2
                        {
                            player.gameObject.transform.position = apearsPos[0].position;
                        }
                        else if (player.lastScene == 5)
                        {
                            player.gameObject.transform.position = apearsPos[0].position;
                        }
                        else if (player.lastScene == 6) //vienes de T4
                        {
                            player.gameObject.transform.position = apearsPos[1].position;
                        }
                    }
                    else if (levelScene == 6)
                    {
                        if (player.lastScene == 5) // Vienes de T3
                        {
                            player.gameObject.transform.position = apearsPos[0].position;
                        }
                        else if (player.lastScene == 6)
                        {
                            player.gameObject.transform.position = apearsPos[0].position;
                        }
                        else if (player.lastScene == 7) // Vienes de S1
                        {
                            player.gameObject.transform.position = apearsPos[1].position;
                        }
                    }
                    if (SceneManager.GetActiveScene().name == "NerbuzFightScene")
                    {
                        player.gameObject.transform.position = apearsPos[0].position;
                    }
                }
            }
            */
            #endregion
        }
        else
        {
            Debug.Log("Primera vez que entra al juego");
        }
    }

    private void Update()
    {
        //AbilitiesGODControl();
        TimeScaleMethod();
        CursorController();

        if (player.isDead)
        {
            ScenesManager.scenesManager.comeFromDead = true;
            if (player.pasSceneDead)
            {
                player.pasSceneDead = false;
                SaveSystem.SavePlayerData(player, inventory, plAttack);
                SceneManager.LoadScene(levelScene);
            }
        }
    }
    void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayerData();
        if(data != null)
        {
            if(data.health > 0)
            {
                player.health = data.health;
            }
            else
            {
                player.health = player.maxHealth;
            }
            player.maxHealth = data.maxHealth;
            player.potions = data.potions;
            player.maxPotions = data.maxPotions;
            player.lastScene = data.lastScene;
            inventory.actualMoney = data.money;
            inventory.nBombs = data.bombs;
            inventory.nTP = data.telePorts;

            plAttack.haveSword = data.haveSword;
            player.dashUnlocked = data.dashUnlocked;
            player.dobleJumpUnlocked = data.highJumpUnlocked;
            player.wallJumpUnlocked = data.wallJumpUnlocked;
            inventory.waterPasive = data.waterPasive;
            inventory.swordPasive = data.swordPasive;
        }
        else
        {
            player.health = player.maxHealth;
            player.maxHealth = 5;
            player.potions = 2;
            player.maxPotions = 5;
            player.lastScene = 0;
            inventory.actualMoney = 0;
            inventory.nBombs = 0;
            inventory.nTP = 0;
            plAttack.haveSword = false;
            player.dashUnlocked = false;
            player.dobleJumpUnlocked = false;
            player.wallJumpUnlocked = false;
            inventory.waterPasive = false;
            inventory.swordPasive = false;
        }
    }

    void AbilitiesGODControl()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            player.dashUnlocked = !player.dashUnlocked;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            player.dobleJumpUnlocked = !player.dobleJumpUnlocked;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            player.wallJumpUnlocked = !player.wallJumpUnlocked;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            plAttack.haveSword = !plAttack.haveSword;
        }


        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            //CARGAR CHECKPOINT1
            player.lastScene = levelScene;
            SaveSystem.SavePlayerData(player, inventory, plAttack);
            SaveSystem.SaveSceneData(ScenesManager.scenesManager);
            SceneManager.LoadScene(14);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            //CARGAR CHECKPOINT2
            player.lastScene = levelScene;
            SaveSystem.SavePlayerData(player, inventory, plAttack);
            SaveSystem.SaveSceneData(ScenesManager.scenesManager);
            SceneManager.LoadScene(18);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            //Crargar checkpoint 3
            player.lastScene = levelScene;
            SaveSystem.SavePlayerData(player, inventory, plAttack);
            SaveSystem.SaveSceneData(ScenesManager.scenesManager);
            SceneManager.LoadScene(28);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            //Crargar checkpoint 3
            player.lastScene = levelScene;
            SaveSystem.SavePlayerData(player, inventory, plAttack);
            SaveSystem.SaveSceneData(ScenesManager.scenesManager);
            SceneManager.LoadScene(10);
        }

    }

    void CursorController()
    {
        if(player.Gamepad != null)
        {
            Cursor.visible = false;
        }
        else
        {
            if (!pauseManager.isPaused && !pauseManager.inShop)
            {
                //Lockea el cursor en medio de la pantalla y lo deja invisible
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    void TimeScaleMethod()
    {
        if (!player.isDead)
        {
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0.5f;
        }
    }

    void ResetAudioManager()
    {
        for (int i = 0; i < AudioManager.instanceAudio.BossSongSource.Length; i++)
        {
            AudioManager.instanceAudio.BossSongSource[i].Stop();
            AudioManager.instanceAudio.BossSongSource[i].volume = 1;
        }
    }
}
