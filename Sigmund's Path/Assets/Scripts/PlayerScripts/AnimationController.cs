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
    private Animator anim;


    private bool isRuning;
    private bool isJumping;
    private bool isDashing;
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
        //anim.SetBool("isAttacking", false);
	}

    void UpdateAnimations()
    {
        anim.SetBool("isRuning", isRuning);
        anim.SetBool("isJumping", isJumping);
        anim.SetFloat("velocityY", player.rb.velocity.y);
        anim.SetBool("isDashing", player.isDashing);
        anim.SetBool("isAttacking", plAttack.isAttacking);
    }
}
