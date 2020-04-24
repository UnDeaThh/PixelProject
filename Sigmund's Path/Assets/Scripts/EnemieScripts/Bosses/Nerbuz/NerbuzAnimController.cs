using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NerbuzAnimController : MonoBehaviour
{
    private NerbuzAI nerbuzBrain;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        nerbuzBrain = GetComponentInParent<NerbuzAI>();
    }

    private void Update()
    {
        UpdateAnimations();


    }

    void UpdateAnimations()
    {
        anim.SetBool("H2Charging", nerbuzBrain.H2ChargingAnim1);
        anim.SetBool("H2Attack", nerbuzBrain.H2AttackAnim1);
        anim.SetBool("isTired", nerbuzBrain.IsTired);
        anim.SetBool("isCrazy", nerbuzBrain.IsCrazy);
    }
    public void StopH2Attack()
    {
        nerbuzBrain.H2AttackAnim1 = false;
    }

    public void StopCrazyTransition2()
    {
        nerbuzBrain.IsCrazy = false;
        nerbuzBrain.IsTired = false;
    }
}
