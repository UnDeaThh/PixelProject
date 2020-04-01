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

    public Animator Anim { get => anim; set => anim = value; }

    void Start()
	{
		//anim = GetComponent<Animator>();
		player = GetComponentInParent<PlayerController2>();
		plAttack = GetComponentInParent<PlayerAttack>();
		plParry = GetComponentInParent<PlayerParry>();
		inventory = GetComponentInChildren<Inventory2>();
        Anim = GetComponent<Animator>();
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

    /*
	public void PlayerStopAttack()
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

    public void PlayerStopSecondAttack()
    {
        Debug.Log("StopAttack2");
        plAttack.isAttacking = false;
        plAttack.gndAttackingFront = false;
        plAttack.gndAttackingUp = false;
        plAttack.airAttackingFront = false;
        plAttack.airAttackingUp = false;
        plAttack.nClicks = 0;
    }
    */
    public void StopAttacks()
    {
        if (Anim.GetCurrentAnimatorStateInfo(0).IsName("Player_FrontAttack"))
        {
            if(plAttack.nClicks < 2)
            {
                plAttack.isAttacking = false;
                plAttack.gndAttackingFront = false;
                plAttack.gndAttackingUp = false;
                plAttack.airAttackingFront = false;
                plAttack.airAttackingUp = false;
                plAttack.nClicks = 0;
            }
        }
        else
        {
            Debug.Log("StopAttack2");
            plAttack.isAttacking = false;
            plAttack.gndAttackingFront = false;
            plAttack.gndAttackingUp = false;
            plAttack.airAttackingFront = false;
            plAttack.airAttackingUp = false;
            plAttack.nClicks = 0;
        }
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
        Anim.SetFloat("velocityY", player.rb.velocity.y);
        Anim.SetBool("isRuning", isRuning);
        Anim.SetBool("isJumping", isJumping);
        Anim.SetBool("isDashing", player.isDashing);
        Anim.SetBool("isAttacking", plAttack.isAttacking);
        Anim.SetInteger("nClicks", plAttack.nClicks);
        Anim.SetBool("gndAttackingFront", plAttack.gndAttackingFront);
        Anim.SetBool("gndAttackingUp", plAttack.gndAttackingUp);
        Anim.SetBool("airAttackingFront", plAttack.airAttackingFront);
        Anim.SetBool("airAttackingUp", plAttack.airAttackingUp);
        Anim.SetBool("isDrinking", player.isDrinking);
        Anim.SetBool("isParry", plParry.isParry);
  
        Anim.SetBool("isDead", player.isDead);
        Anim.SetBool("ffDead", ffDead);
        if (player.isDead && !ffDead)
        {
            ffDead = true;
        }
    }
}
