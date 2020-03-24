using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScenesData 
{
    public int toLoadScene;
    public List<bool> palancasState = new List<bool>();
    public List<bool> heartsPickUp = new List<bool>();
    public bool cutSceneDone;
    public bool swordPicked;
    public bool firstTalkAska;
    public ScenesData(ScenesManager SM)
    {
        toLoadScene = SM.toLoadScene;
        cutSceneDone = SM.cutSceneDone;

        for (int i = 0; i < palancasState.Count; i++)
        {
            palancasState[i] = SM.palancasState[i];
        }     

        for (int i = 0; i < heartsPickUp.Count; i++)
        {
            heartsPickUp[i] = SM.heartsPickUp[i];
        }
        swordPicked = SM.swordPicked;
        firstTalkAska = SM.firstTalkAska;
    }
}
