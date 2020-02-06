using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private PlayerController2 player;
    private Inventory2 inventory;
    private PlayerAttack plAttack;
    public Transform[] apearsPos;

    public int levelScene;

    private void Awake()
    {
        levelScene += 2;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController2>();
        plAttack = player.gameObject.GetComponent<PlayerAttack>();
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory2>();

        //Carga la player Info
        LoadPlayer();

        //Posiciona al Player
        if (player.lastScene == 3)
        {
            player.gameObject.transform.position = apearsPos[0].position;
        }
        else if (player.lastScene == 4)
        {
            player.gameObject.transform.position = apearsPos[0].position;
        }
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
            player.highJumpUnlocked = data.highJumpUnlocked;
            player.wallJumpUnlocked = data.highJumpUnlocked;
            inventory.waterPasive = data.waterPasive;
            inventory.swordPasive = data.swordPasive;
        }
        else
        {
            print("no Data");
        }
    }
}
