using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NerbuzAnimController : MonoBehaviour
{
    private NerbuzBoss nerbuzBrain;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        nerbuzBrain = GetComponentInParent<NerbuzBoss>();
    }

    private void Update()
    {
        UpdateAnimations();
    }

    void UpdateAnimations()
    {
        anim.SetBool("H2Charging", nerbuzBrain.H2ChargingAnim);
        anim.SetBool("H2Attack", nerbuzBrain.H2attackAnim);
        anim.SetBool("isTired", nerbuzBrain.isTired);
        anim.SetBool("isCrazy", nerbuzBrain.isCrazy);
    }
    public void StopH2Attack()
    {
        nerbuzBrain.H2attackAnim = false;
    }

    public void StopCrazyTransition2()
    {
        nerbuzBrain.isCrazy = false;
        nerbuzBrain.isTired = false;
    }
}
