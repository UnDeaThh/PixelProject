using System.Collections;
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
    public bool isParry;
    private bool parryDone = false;
    private bool justOneTime = false;
    public bool parrySuccesful = false;
    public bool parryFail = false;
    private bool alreadyClicked = false;


    public Collider2D parryCol;
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
        isParry = false;
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
            if (timeBtwParry <= 0 && plController2.isGrounded == true)
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
                plController2.heedArrows = false;
                parryCol.enabled = true;
            }
            else if(currentParryTime <= 0f || parryDone)
            {
                parryCol.enabled = false;
                timeBtwParry = startTimeBtwParry;
                plController2.heedArrows = true;

                isParry = false;
                parryDone = false;
                alreadyClicked = false;
            }

        }

        //AFTER PARRY NOT DONE, OSEA LO HE FALLADO
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
                other.GetComponentInParent<BaseEnemy>().StartStun();
                parryDone = true;
                parrySuccesful = true;
                
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
}
