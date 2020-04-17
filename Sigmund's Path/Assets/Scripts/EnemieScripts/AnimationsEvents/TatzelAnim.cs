using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TatzelAnim : MonoBehaviour
{
    [SerializeField] TatzelAI brain;

    public void StopAttack()
    {
        brain.Attack1 = false;
    }

    void DestroyTatzel()
    {
        brain.callDead = true;
        brain.InstantiateSoul(brain.enemyType);
    }

    void ColliderAttack()
    {
        brain.MakeOneattack = true;
    }
}
