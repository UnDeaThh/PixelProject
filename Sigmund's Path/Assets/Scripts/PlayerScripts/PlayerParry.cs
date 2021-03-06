﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParry : MonoBehaviour
{
    PlayerInputs inputs;

    private PlayerController2 plController2;
    private PlayerAttack plAttack;
	private PauseManager pauseManager;

    private float timeBtwParry;
    public float startTimeBtwParry = 0.1f;
    public float parryDuration = 1f;
    private float currentParryTime;
    public float failParryTime = 0.3f;

    private bool canParry;
    private bool isParry;
    private bool parryDone = false;
    private bool justOneTime = false;
    private bool parrySuccesful = false;
    public bool parryFail = false;
    private bool alreadyClicked = false;


    public Collider2D parryCol;
    [SerializeField] GameObject parryParticle;

    public bool IsParry { get => isParry; set => isParry = value; }
    public bool ParryDone { get => parryDone; set => parryDone = value; }
    public bool ParrySuccesful { get => parrySuccesful; set => parrySuccesful = value; }

    private void OnEnable()
    {
        inputs.Controls.Enable();
    }
    private void OnDisable()
    {
        inputs.Controls.Disable();
    }
    private void Awake()
    {
        inputs = new PlayerInputs();
        plAttack = GetComponent<PlayerAttack>();
        IsParry = false;
        parryCol.enabled = false;
        
    }

    private void Start()
    {
		pauseManager = GameObject.FindGameObjectWithTag("PauseManager").GetComponent<PauseManager>();
        plController2 = GetComponent<PlayerController2>();
    }
    private void Update()
    {
        if (!plController2.isGODmode)
        {
		    if(!pauseManager.isPaused){
		
			    CheckIfCanParry();
			    ParryInput();
			    Parry();
		    }
        }
    }

    void CheckIfCanParry()
    {
        if (plAttack.haveSword)
        {
            if (!plController2.isDashing)
            {
                if (timeBtwParry <= 0 && plController2.IsGrounded == true)
                {
                    canParry = true;

                }
                else
                {
                    timeBtwParry -= Time.deltaTime;
                    canParry = false;
                }
            }
            else
            {
                canParry = false;
            }
        }
        else
            canParry = false;
    }

    void ParryInput()
    {
        if (canParry)
        {
            if (inputs.Controls.Parry.triggered && !alreadyClicked)
            {
                alreadyClicked = true;
                currentParryTime = parryDuration;
                plController2.heedArrows = false;
                plController2.rb.velocity = new Vector2(0f, 0f);
                IsParry = true;
                justOneTime = true;
                
            }
        }

    }

    void Parry()
    {
        //Stay till is doing parry see at PlayerController
        if (IsParry)
        {
            if(currentParryTime > 0f && !ParryDone)
            {
                currentParryTime -= Time.deltaTime;
                plController2.heedArrows = false;
                parryCol.enabled = true;
            }
            else if(currentParryTime <= 0f || ParryDone)
            {
                parryCol.enabled = false;
                timeBtwParry = startTimeBtwParry;
                plController2.heedArrows = true;

                IsParry = false;
                ParryDone = false;
                alreadyClicked = false;
            }

        }

        //AFTER PARRY NOT DONE, OSEA LO HE FALLADO
        else if (currentParryTime <= 0f && !ParryDone && justOneTime)
        {
            Debug.Log("salsa");
            parryFail = true;
            StartCoroutine(FailedParry());
            justOneTime = false;
        }

    }

    IEnumerator FailedParry()
    {
        //EN EL PLAYERCONTROLLER HARA QUE NO TE PUEDAS MOVER
        yield return new WaitForSeconds(failParryTime);
        parryFail = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsParry)
        {
            if(other.tag == "Enemy")
            {
                if(other.GetComponent<BaseEnemy>().enemyType == EnemyClass.Nach || other.GetComponent<BaseEnemy>().enemyType == EnemyClass.Changeling)
                {      
                    other.GetComponentInParent<BaseEnemy>().StartStun();
                    ParryDone = true;
                    ParrySuccesful = true;
                }
            }
            else
            {
                ParryDone = false;
            }
        }
    }

    public void CallParry()
    {
        parryDone = true;
        parrySuccesful = true;
        Instantiate(parryParticle, transform.position, Quaternion.identity);
    }
}
