using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScenesData 
{
    public int toLoadScene;
    public bool[] palancasState = new bool[ScenesManager.scenesManager.palancasState.Length];
    public ScenesData(ScenesManager SM)
    {
        toLoadScene = SM.toLoadScene;
        for (int i = 0; i < palancasState.Length; i++)
        {
            palancasState[i] = SM.palancasState[i];
        }
    }
}
