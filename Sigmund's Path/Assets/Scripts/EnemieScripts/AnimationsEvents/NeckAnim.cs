using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeckAnim : MonoBehaviour
{
    [SerializeField] NeckAI neckBrain;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        anim.SetBool("isAttacking", neckBrain.isAttacking);
    }
    void FirstAttack()
    {
        neckBrain.firstAttack = true;
    }

    void Attack()
    {
        neckBrain.makeAnAttack = true;
    }
}
