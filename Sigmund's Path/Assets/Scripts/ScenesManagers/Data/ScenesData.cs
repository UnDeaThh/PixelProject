using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScenesData 
{
    public int toLoadScene;
    public bool[] palancasState = new bool[10];
    public bool[] heartsPickUp = new bool[10];
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
        swordPicked = SM.SwordPicked;
        firstTalkAska = SM.FirstTalkAska;
    }
}
