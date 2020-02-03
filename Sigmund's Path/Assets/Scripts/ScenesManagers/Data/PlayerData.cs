﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int health;
    public int maxHealth;
    public int potions;
    public int maxPotions;
    public int money;
    public int bombs;
    public int telePorts;

    public bool dashUnlocked;
    public bool highJumpUnlocked;
    public bool wallJumpUnlocked;
    public bool waterPasive;
    public bool swordPasive;

    public PlayerData(PlayerController2 plController2, Inventory2 inventory2)
    {
        health = plController2.health;
        maxHealth = plController2.maxHealth;
        potions = plController2.potions;
        maxPotions = plController2.maxPotions;
        money = inventory2.actualMoney;
        bombs = inventory2.nBombs;
        telePorts = inventory2.nTP;

        dashUnlocked = plController2.dashUnlocked;
        highJumpUnlocked = plController2.highJumpUnlocked;
        wallJumpUnlocked = plController2.wallJumpUnlocked;
        waterPasive = inventory2.waterPasive;
        swordPasive = inventory2.swordPasive;
    }
}
