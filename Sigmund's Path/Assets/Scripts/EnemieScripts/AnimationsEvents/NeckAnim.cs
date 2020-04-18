using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeckAnim : MonoBehaviour
{
    [SerializeField] NeckAI neckBrain;
    private Animator anim;
    private int nAttacks = 0;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        anim.SetBool("isAttacking", neckBrain.isAttacking);
        anim.SetFloat("movSpeed", Mathf.Abs(neckBrain.rb.velocity.x));
        anim.SetBool("isStuned", neckBrain.isStuned);
    }
    void FirstAttack()
    {
        neckBrain.firstAttack = true;
    }

    void Attack()
    {
        neckBrain.makeAnAttack = true;
        nAttacks++;
        if(nAttacks >= 2)
        {
            neckBrain.isAttacking = false;
            nAttacks = 0;
        }
    }

    void CallDead()
    {
        neckBrain.callDead = true;
        neckBrain.InstantiateSoul(neckBrain.enemyType);
    }
}
