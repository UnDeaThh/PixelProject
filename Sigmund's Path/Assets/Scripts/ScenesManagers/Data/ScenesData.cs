using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScenesData 
{
    public int toLoadScene;

    public bool[] palancasState = new bool[10];
    public bool[] heartsPickUp = new bool[10];
    public bool[] cumuloState = new bool[20];
    public bool[] unlokedZone = new bool[26];
    public bool[] fountainUnlocked = new bool[5];
    public bool[] destruibleWall = new bool[50];
    public bool[] bossKilled = new bool[3];
    public bool[] shopUnlocked = new bool[3];

    public bool cutSceneDone;
    public bool apearsOnFountain;
    public bool swordPicked;
    public bool firstTalkAska;
    public ScenesData(ScenesManager SM)
    {
        toLoadScene = SM.toLoadScene;
        cutSceneDone = SM.CutSceneDone;
        apearsOnFountain = SM.ApearsOnFountain;

        for (int i = 0; i < SM.PalancasState.Length; i++)
        {
            palancasState[i] = SM.PalancasState[i];
        }     
        for (int i = 0; i < SM.HeartsPickUp.Length; i++)
        {
            heartsPickUp[i] = SM.HeartsPickUp[i];
        }
        for (int i = 0; i < SM.CumuloState.Length; i++)
        {
            cumuloState[i] = SM.CumuloState[i];
        }
        for (int i = 0; i < SM.UnlokedZone.Length; i++)
        {
            unlokedZone[i] = SM.UnlokedZone[i];
        }
        for (int i = 0; i < SM.FountainUnlocked.Length; i++)
        {
            fountainUnlocked[i] = SM.FountainUnlocked[i];
        }
        for (int i = 0; i < SM.DestruibleWall.Length; i++)
        {
            destruibleWall[i] = SM.DestruibleWall[i];
        }
        for (int i = 0; i < SM.BossKilled.Length; i++)
        {
            bossKilled[i] = SM.BossKilled[i];
        }

        for (int i = 0; i < SM.ShopUnlocked.Length; i++)
        {
            shopUnlocked[i] = SM.ShopUnlocked[i];
        }
        
        swordPicked = SM.SwordPicked;
        firstTalkAska = SM.FirstTalkAska;
    }
}
