using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParry : MonoBehaviour
{
    private float timeBtwParry;
    public float startTimeBtwParry = 0.1f;
    public float parryDuration = 1f;
    private float currentParryTime;
    private float failParryTime = 0.1f;

    private bool canParry;
    public bool isParry;
    private bool parryDone = false;
    private bool justOneTime;
    public bool parryFail = false;



    public Collider2D parryCol;
    private PlayerController plController;
    private Animator anim;
    private void Awake()
    {
        plController = GetComponent<PlayerController>();
        anim = GetComponentInChildren<Animator>();
        isParry = false;
        parryCol.enabled = false;
        
    }
    private void Update()
    {
        CheckIfCanParry();
        ParryInput();
        Parry();


        UpdateAnimations();
    }

    void CheckIfCanParry()
    {
        if(timeBtwParry <= 0 && plController.isGrounded == true)
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
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                currentParryTime = parryDuration;
                isParry = true;
                justOneTime = true;
            }
        }

    }

    void Parry()
    {
        if (isParry)
        {
            if(currentParryTime > 0f)
            {
                currentParryTime -= Time.deltaTime;
                parryCol.enabled = true;
            }
            else if(currentParryTime <= 0f)
            {
                parryCol.enabled = false;
                timeBtwParry = startTimeBtwParry;

                isParry = false;
            }
            //AFTER PARRY NOT DONE
            if (currentParryTime <= 0f && !parryDone && justOneTime)
            {
                Debug.Log("salsa");
                parryFail = true;
                StartCoroutine(FailedParry());
                justOneTime = false;
            }
        }

    }

    IEnumerator FailedParry()
    {
        yield return new WaitForSeconds(failParryTime);
        parryFail = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isParry)
        {
            if(other.tag == "Enemy")
            {
                other.GetComponentInParent<Enemy>().Stuned();
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
