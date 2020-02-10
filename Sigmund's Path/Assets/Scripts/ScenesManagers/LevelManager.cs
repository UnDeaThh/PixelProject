using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private PlayerController2 player;
    private Inventory2 inventory;
    private PlayerAttack plAttack;
    public Transform[] apearsPos;
    private PauseManager pauseManager;
    public Transform fountainPos;

    public int levelScene;

    private void Awake()
    {
        levelScene += 2;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController2>();
        plAttack = player.gameObject.GetComponent<PlayerAttack>();
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory2>();
        pauseManager = GameObject.FindGameObjectWithTag("PauseManager").GetComponent<PauseManager>();

        //Carga la player Info
        LoadPlayer();
        if (ScenesManager.scenesManager.comeFromDead)
        {
            player.health = player.maxHealth;
            ScenesManager.scenesManager.comeFromDead = false;
        }

        //POSICIONA AL PLAYER
        if (ScenesManager.scenesManager.apearsOnFountain)
        {
            if (fountainPos)
            {
                player.gameObject.transform.position = fountainPos.position;
                ScenesManager.scenesManager.apearsOnFountain = false;
            }
        }

        else
        {
            if(apearsPos.Length > 0)
            {
                if(levelScene == 3) //ESTANDO EN T1
                {
                    if(player.lastScene == 3 || player.lastScene == 0) //vienes de la propia T1
                    {
                        player.gameObject.transform.position = apearsPos[0].position;
                    }
                    else if(player.lastScene == 4) //vienes de T2
                    {
                        player.gameObject.transform.position = apearsPos[1].position;
                    }
                }
                else if(levelScene == 4) //ESTANDO EN T2
                {
                    if(player.lastScene == 3)//vienes de T1
                    {
                        player.gameObject.transform.position = apearsPos[0].position;
                    }
                    else if(player.lastScene == 4)
                    {
                        player.gameObject.transform.position = apearsPos[0].position;
                    }
                    else if(player.lastScene == 5)//vienes de T3
                    {
                        player.gameObject.transform.position = apearsPos[1].position;
                    }
                }
                else if(levelScene == 5) //ESTANDO EN T3
                {
                    if(player.lastScene == 4) //vienes de T2
                    {
                        player.gameObject.transform.position = apearsPos[0].position;
                    }
                    else if(player.lastScene == 5)
                    {
                        player.gameObject.transform.position = apearsPos[0].position;
                    }
                    else if(player.lastScene == 6) //vienes de T4
                    {
                        player.gameObject.transform.position = apearsPos[1].position;
                    }
                }
                else if(levelScene == 6)
                {
                    if(player.lastScene == 5) // Vienes de T3
                    {
                        player.gameObject.transform.position = apearsPos[0].position;
                    }
                    else if(player.lastScene == 6) 
                    {
                        player.gameObject.transform.position = apearsPos[0].position;
                    }
                    else if(player.lastScene == 7) // Vienes de S1
                    {
                        player.gameObject.transform.position = apearsPos[1].position;
                    }
                }
            }
        }
    }

    private void Update()
    {
        AbilitiesGODControl();
        TimeScaleMethod();
        if (player.isDead)
        {
            ScenesManager.scenesManager.comeFromDead = true;
            StartCoroutine(ReloadLevel());
        }
    }
    IEnumerator ReloadLevel()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(levelScene);
    }

    void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayerData();
        if(data != null)
        {
            player.health = data.health;
            player.maxHealth = data.maxHealth;
            player.potions = data.potions;
            player.maxPotions = data.maxPotions;
            player.lastScene = data.lastScene;
            player.facingDir = data.facingDir;
            inventory.actualMoney = data.money;
            inventory.nBombs = data.bombs;
            inventory.nTP = data.telePorts;

            plAttack.haveSword = data.haveSword;
            player.dashUnlocked = data.dashUnlocked;
            player.dobleJumpUnlocked = data.highJumpUnlocked;
            player.wallJumpUnlocked = data.highJumpUnlocked;
            inventory.waterPasive = data.waterPasive;
            inventory.swordPasive = data.swordPasive;
        }
        else
        {
            print("no Data");
        }
    }

    void AbilitiesGODControl()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            player.dashUnlocked = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            player.dobleJumpUnlocked = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            player.wallJumpUnlocked = true;
        }
    }

    void CursorController()
    {
        if (!pauseManager.isPaused)
        {
            //Lockea el cursor en medio de la pantalla y lo deja invisible
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
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
}
