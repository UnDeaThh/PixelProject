using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
	//private Animator anim;
	private PlayerController2 player;
	private PlayerAttack plAttack;
	private Inventory2 inventory;
	private PlayerParry plParry;
    public PlayerAudio plAudio;
    private Animator anim;
    private bool ffDead = false;


    private bool isRuning;
    private bool isJumping;

	void Start()
	{
		//anim = GetComponent<Animator>();
		player = GetComponentInParent<PlayerController2>();
		plAttack = GetComponentInParent<PlayerAttack>();
		plParry = GetComponentInParent<PlayerParry>();
		inventory = GetComponentInChildren<Inventory2>();
        anim = GetComponent<Animator>();
	}

	void Update()
	{
        if(Mathf.Abs(player.rb.velocity.x) > 0.1)
        {
            isRuning = true;
        }
        else
        {
            isRuning = false;
        }

        if (!player.isGrounded)
        {
            isJumping = true;
        }
        else
        {
            isJumping = false;
        }

        UpdateAnimations();
	}

	void PlayerStopAttack()
	{
        if(plAttack.nClicks < 2)
        {
		    plAttack.isAttacking = false;
            plAttack.gndAttackingFront = false;
            plAttack.gndAttackingUp = false;
            plAttack.airAttackingFront = false;
            plAttack.airAttackingUp = false;
        }
	}

    void PlayerStopSecondAttack()
    {
        plAttack.isAttacking = false;
        plAttack.gndAttackingFront = false;
        plAttack.gndAttackingUp = false;
        plAttack.airAttackingFront = false;
        plAttack.airAttackingUp = false;
    }
    void StopHealing()
    {
        player.health++;
        player.isDrinking = false;
        player.heedArrows = true;
        plAudio.healingSound[1].Play();
        player.cntTimeNextDrink = 0;
    }

    void UpdateAnimations()
    {
        anim.SetFloat("velocityY", player.rb.velocity.y);
        anim.SetBool("isRuning", isRuning);
        anim.SetBool("isJumping", isJumping);
        anim.SetBool("isDashing", player.isDashing);
        anim.SetBool("isAttacking", plAttack.isAttacking);
        anim.SetInteger("nClicks", plAttack.nClicks);
        anim.SetBool("gndAttackingFront", plAttack.gndAttackingFront);
        anim.SetBool("gndAttackingUp", plAttack.gndAttackingUp);
        anim.SetBool("airAttackingFront", plAttack.airAttackingFront);
        anim.SetBool("airAttackingUp", plAttack.airAttackingUp);
        anim.SetBool("isDrinking", player.isDrinking);
        anim.SetBool("isParry", plParry.isParry);
  
        anim.SetBool("isDead", player.isDead);
        anim.SetBool("ffDead", ffDead);
        if (player.isDead && !ffDead)
        {
            ffDead = true;
        }
    }
}
