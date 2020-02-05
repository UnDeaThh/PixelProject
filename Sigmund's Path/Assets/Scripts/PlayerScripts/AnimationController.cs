using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
	private Animator anim;
	private PlayerController2 player;
	private PlayerAttack plAttack;
	private Inventory2 inventory;
	private PlayerParry plParry;


	void Start()
	{
		anim = GetComponent<Animator>();
		player = GetComponentInParent<PlayerController2>();
		plAttack = GetComponentInParent<PlayerAttack>();
		plParry = GetComponentInParent<PlayerParry>();
		inventory = GetComponent<Inventory2>();
	}

	void Update()
	{
		anim.SetBool("isAttacking", plAttack.isAttacking);
	}

	void PlayerFrontAttack()
	{
        anim.SetBool("isAttacking", false);
	}

	void PlayerUpAttack()
	{
		anim.SetBool("isAttacking", false);
	}

}
