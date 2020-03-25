using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Askafroa : MonoBehaviour
{
    public int sleepingState = 0;
    private Animator anim;
    public bool isSleeping;
    [SerializeField] GameObject particle;
    private bool[] array = new bool[10];
    private void Start()
    {
        anim = GetComponent<Animator>();
        if (ScenesManager.scenesManager.firstTalkAska)
        {
            isSleeping = true;
        }
    }

    private void Update()
    {
        anim.SetBool("sleeping", isSleeping);
        if (isSleeping)
        {
            particle.SetActive(true);
        }
        else
        {
            particle.SetActive(false);
        }
    }
}
