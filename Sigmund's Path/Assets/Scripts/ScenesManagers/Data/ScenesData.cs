﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScenesData 
{
    public int toLoadScene;

    public bool[] palancasState = new bool[10];
    public bool[] heartsPickUp = new bool[10];
    public bool[] cumuloState = new bool[20];
    public bool[] unlokedZone = new bool[20];
    public bool[] destruibleWall = new bool[50];

    public bool cutSceneDone;
    public bool swordPicked;
    public bool firstTalkAska;
    public ScenesData(ScenesManager SM)
    {
        toLoadScene = SM.toLoadScene;
        cutSceneDone = SM.cutSceneDone;

        for (int i = 0; i < SM.palancasState.Length; i++)
        {
            palancasState[i] = SM.palancasState[i];
        }     
        for (int i = 0; i < SM.heartsPickUp.Length; i++)
        {
            heartsPickUp[i] = SM.heartsPickUp[i];
        }
        for (int i = 0; i < SM.cumuloState.Length; i++)
        {
            cumuloState[i] = SM.cumuloState[i];
        }
        for (int i = 0; i < SM.unlokedZone.Length; i++)
        {
            unlokedZone[i] = SM.unlokedZone[i];
        }
        for (int i = 0; i < SM.DestruibleWall.Length; i++)
        {
            destruibleWall[i] = SM.DestruibleWall[i];
        }
        
        swordPicked = SM.SwordPicked;
        firstTalkAska = SM.FirstTalkAska;
    }
}
