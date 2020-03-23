using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Askafroa : MonoBehaviour
{
    public int sleepingState = 0;
    private Animator anim;
    public bool isSleeping;
    private void Start()
    {
        anim = GetComponent<Animator>();
        if (ScenesManager.scenesManager.firstTalkAska)
        {
            anim.SetTrigger("sleepingState");
        }
    }

    private void Update()
    {
        if (isSleeping)
        {
            anim.SetTrigger("sleepingState");
        }
    }
}
