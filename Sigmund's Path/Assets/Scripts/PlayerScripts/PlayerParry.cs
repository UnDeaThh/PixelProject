using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParry : MonoBehaviour
{
    public static PlayerParry plParry;

    private float timeBtwParry;
    public float startTimeBtwParry = 0.1f;
    public float parryDuration = 1f;
    private float currentParryTime;
    public float failParryTime = 0.3f;

    private bool canParry;
    public bool isParry;
    private bool parryDone = false;
    private bool justOneTime = false;
    public bool parrySuccesful = false;
    public bool parryFail = false;
    private bool alreadyClicked = false;


    public Collider2D parryCol;
    private Animator anim;
    private void Awake()
    {
        if(plParry == null)
        {
            plParry = this;
        }
        if(plParry != this)
        {
            Destroy(gameObject);
        }

        anim = GetComponentInChildren<Animator>();
        isParry = false;
        parryCol.enabled = false;
        
    }
    private void Update()
    {
        if (!PlayerController2.plController2.isGODmode)
        {
		    if(!PauseManager.pauseManager.isPaused){
		
			    CheckIfCanParry();
			    ParryInput();
			    Parry();
		    }


            UpdateAnimations();
        }
    }

    void CheckIfCanParry()
    {
        if(timeBtwParry <= 0 && PlayerController2.plController2.isGrounded == true)
        {
            canParry = true;
            
        }
        else 
        {
            timeBtwParry -= Time.deltaTime;
            canParry = false;
        }
    }

    void ParryInput()
    {
        if (canParry)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1) && !alreadyClicked)
            {
                alreadyClicked = true;
                currentParryTime = parryDuration;
                isParry = true;
                justOneTime = true;
                
            }
        }

    }

    void Parry()
    {
        //Stay till is doing parry see at PlayerController
        if (isParry)
        {
            if(currentParryTime > 0f && !parryDone)
            {
                currentParryTime -= Time.deltaTime;
                parryCol.enabled = true;

            }
            else if(currentParryTime <= 0f || parryDone)
            {
                parryCol.enabled = false;
                timeBtwParry = startTimeBtwParry;

                isParry = false;
                parryDone = false;
                alreadyClicked = false;
            }

        }

        //AFTER PARRY NOT DONE
        else if (currentParryTime <= 0f && !parryDone && justOneTime)
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
        if (isParry)
        {
            if(other.tag == "Enemy")
            {
                other.GetComponentInParent<BaseEnemy>().isStuned = true;
                parryDone = true;
                parrySuccesful = true;
                Debug.Log("parry");
                
            }
            else if(other.tag == "Arrow")
            {
                Destroy(other.gameObject);
                parryDone = true;
                
            }
            else
            {
                parryDone = false;
            }
        }
    }
    
    void UpdateAnimations()
    {
        anim.SetBool("isParry", isParry);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
    }
}
