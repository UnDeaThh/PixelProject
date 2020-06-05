using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangelingEvent : MonoBehaviour
{
    private ChangelingAI changelingBrain;

    private void Start()
    {
        changelingBrain = GetComponentInParent<ChangelingAI>();
    }

    void DestroyChangeling()
    {
        changelingBrain.callDead = true;
        changelingBrain.InstantiateSoul(changelingBrain.enemyType);
    }
    
    void AleteoSound()
    {
        changelingBrain.MakeAleteoSound();
    }
}
