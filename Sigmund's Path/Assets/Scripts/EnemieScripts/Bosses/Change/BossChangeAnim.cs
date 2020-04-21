using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChangeAnim : MonoBehaviour
{
    private BossChange brain;
    private Animator anim;

    private void Start()
    {
        brain = GetComponentInParent<BossChange>();
        anim = GetComponent<Animator>();
    }


    void ThrowBalls()
    {
        brain.ThrowBalls = true;
    }

    void StopAnimH3()
    {
        anim.SetBool("makeH3", false);
        brain.CntTimeToThrowH3 = brain.TimeThrowH3;
    }

    void CallDead()
    {
        brain.isDisolve = true;
    }
}
