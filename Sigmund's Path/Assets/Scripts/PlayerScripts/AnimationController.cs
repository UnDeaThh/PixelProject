﻿using System.Collections;
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
        if (player.IsDamaged)
        {
            if (plAttack.isAttacking)
            {
                plAttack.CanSecondAttack = false;
                plAttack.isAttacking = false;
                plAttack.gndAttackingFront = false;
                plAttack.gndAttackingUp = false;
                plAttack.airAttackingFront = false;
                plAttack.airAttackingUp = false;
                plAttack.nClicks = 0;
            }
            player.IsDamaged = false;
        }

        if(Mathf.Abs(player.rb.velocity.x) > 0.2)
        {
            isRuning = true;
        }
        else
        {
            isRuning = false;
        }

        if (!player.IsGrounded)
        {
            isJumping = true;
        }
        else
        {
            isJumping = false;
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("AirFrontAttack2"))
        {
            if (player.IsGrounded)
            {
                plAttack.nClicks = 0;
                plAttack.isAttacking = false;
                plAttack.airAttackingFront = false;
            }
        }

        UpdateAnimations();
	}
    public void EnableSecondAttack()
    {
        plAttack.CanSecondAttack = true;
    }

    public void StopFirstAttack()
    {
        if(plAttack.nClicks < 2) // Al final del primer ataque
        {
            plAttack.CanSecondAttack = false;
            plAttack.isAttacking = false;
            plAttack.gndAttackingFront = false;
            plAttack.gndAttackingUp = false;
            plAttack.airAttackingFront = false;
            plAttack.airAttackingUp = false;
            plAttack.nClicks = 0;
        }
        
    }

    public void StopUpAttack()
    {
        plAttack.CanSecondAttack = false;
        plAttack.isAttacking = false;
        plAttack.gndAttackingFront = false;
        plAttack.gndAttackingUp = false;
        plAttack.airAttackingFront = false;
        plAttack.airAttackingUp = false;
        plAttack.nClicks = 0;
    }

    public void StopSecondAttack()
    {
        plAttack.CanSecondAttack = false;
        plAttack.isAttacking = false;
        plAttack.gndAttackingFront = false;
        plAttack.gndAttackingUp = false;
        plAttack.airAttackingFront = false;
        plAttack.airAttackingUp = false;
        plAttack.nClicks = 0; 
    }
    void StopHealing()
    {
        player.health++;
        player.isDrinking = false;
        player.heedArrows = true;
        plAudio.healingSound[1].Play();
        plAudio.HeartObtained();
        player.cntTimeNextDrink = 0;
    }


    void TeleportSound()
    {
        plAudio.TeleportSound();
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
        Anim.SetBool("isParry", plParry.IsParry);
  
        Anim.SetBool("isDead", player.isDead);
        Anim.SetBool("ffDead", ffDead);
        Anim.SetBool("haveSword", plAttack.haveSword);
        anim.SetBool("isWallSliding", player.isWallSliding);
        anim.SetBool("startTP", player.StartTP);


        if (player.isDead && !ffDead)
        {
            ffDead = true;
        }
    }


    void WalkSounds()
    {
        plAudio.walkSound.pitch = Random.Range(0.75f, 1.3f);
        plAudio.walkSound.Play();
    }
}
