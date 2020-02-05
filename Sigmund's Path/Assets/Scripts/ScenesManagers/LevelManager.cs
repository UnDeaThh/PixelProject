using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private PlayerController2 player;
    private Inventory2 inventory;
    public Transform[] apearsPos;

    public int actualScene;

    private void Awake()
    {
        actualScene += 2;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController2>();
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory2>();


        LoadPlayer();

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
