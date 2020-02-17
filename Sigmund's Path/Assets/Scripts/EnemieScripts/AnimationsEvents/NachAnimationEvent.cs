using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NachAnimationEvent : MonoBehaviour
{
    private NachAI nachBrain;

    private void Start()
    {
        nachBrain = GetComponentInParent<NachAI>();
    }

    void DestroyNach()
    {
        nachBrain.callDead = true;
        nachBrain.InstantiateSoul(nachBrain.enemyType);
    }
}
