using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScenesData 
{
    public int toLoadScene;

    public ScenesData(ScenesManager SM)
    {
        toLoadScene = SM.toLoadScene;
    }
}
