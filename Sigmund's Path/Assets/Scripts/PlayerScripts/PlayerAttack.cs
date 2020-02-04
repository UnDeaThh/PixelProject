using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerController2 plController2;
    private PlayerParry plParry;


    public int damage;

    private float timeBtwttack;
    public float startTimeBtwAttack;

    public Vector2 attackRangeFront = new Vector2(3f, 2f);
    public float attackRangeUp = 3f;

    private bool attackingUp = false;
    private bool attackingFront = false;

    private bool canAttack;
    [HideInInspector] public bool isAttacking = false;

    private Vector3 frontAttackPos = new Vector3(1.5f, 0.0f, 0.0f);
    private Vector3 upAttackPos = new Vector3(0.0f, 3f, 0.0f);

    public Transform attackPos;
    private Transform playerPos;

    public LayerMask whatIsEnemie;

    private Animator anim;
    private CameraController cameraController;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        playerPos = GetComponent<Transform>();
        cameraController = GameObject.FindGameObjectWithTag("CameraManager").GetComponent<CameraController>();
    }

    private void Start()
    {
        plController2 = GetComponent<PlayerController2>();
        plParry = GetComponent<PlayerParry>();
    }
    void Update()
    {
        if (!plController2.isGODmode)
        {
            if(!plController2.isDead)
            {
		        if(!PauseManager.pauseManager.isPaused){
			        CheckIfCanAttack();
			        Attack();

			        UpdateAnimations();
		        }
            }
        }
    }

    void CheckIfCanAttack()
    {
        if(timeBtwttack <= 0 && !plController2.isDrinking //&& !PlayerController2.plController2.isHanging //
        && !plController2.isWallSliding)
        {
            canAttack = true;
            
        }
        else
            canAttack = false;
            
        if (timeBtwttack > 0)
        {
            timeBtwttack -= Time.deltaTime;
        }

    }

    void Attack()
    {
        if (plController2.isWallSliding)
        {
            isAttacking = false;
        }
        //Doble damage with parry
        if (plParry.parrySuccesful)
        {
            damage *= 2;
        }

        if (canAttack)
        {
            //FRONT ATTACK
            if (Input.GetKeyDown(KeyCode.Mouse0) && !Input.GetKey(KeyCode.W))
            {
                isAttacking = true;
                attackingFront = true;
                attackingUp = false;
                //Primero seteamos la posicion del collider
                if (plController2.facingDir == 1)
                {
                    Vector3 finalPos = playerPos.position + frontAttackPos;
                    attackPos.position = finalPos;
                }
                else if(plController2.facingDir == -1)
                {
                    Vector3 finalPos = playerPos.position + (-frontAttackPos);
                    attackPos.position = finalPos;
                }
                //Luego creamos el Collider en ese sitio
                Collider2D[] enemiesToDamage = Physics2D.OverlapBoxAll(attackPos.position, attackRangeFront, 0, whatIsEnemie);
                for(int i = 0; i < enemiesToDamage.Length; i++)
                {
                    if(enemiesToDamage[i].tag == "Enemy")
                    {
                        cameraController.letsShake = true;
                        if(enemiesToDamage[i].TryGetComponent(out ChangelingAI changeling))
                        {
                            changeling.TakeDamage(damage);
                            plParry.parrySuccesful = false;

                        }
                        if(enemiesToDamage[i].TryGetComponent(out BermonchAI bermonch)){
                            bermonch.TakeDamage(damage);
                            plParry.parrySuccesful = false;
                        }
                    }
                }
                canAttack = false;
                timeBtwttack = startTimeBtwAttack;
            }
            //UP ATTACK
            else if(Input.GetKeyDown(KeyCode.Mouse0) && Input.GetKey(KeyCode.W))
            {
                isAttacking = true;
                attackingFront = false;
                attackingUp = true;
                Debug.Log("UpAttack");
                Vector3 finalPos = playerPos.position + upAttackPos;

                attackPos.position = finalPos;
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRangeUp, whatIsEnemie);
	
                Debug.Log(enemiesToDamage.Length);
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    if (enemiesToDamage[i].CompareTag("Enemy"))
                    {
                        if (enemiesToDamage[i].TryGetComponent(out ChangelingAI changeling))
                        {
                            if (!plParry.parrySuccesful)
                            {
                                changeling.TakeDamage(damage);
                            }
                            else
                            {
                                changeling.TakeDamage(damage * 2);
                            }
                        }
                    }
                }

                canAttack = false;
                timeBtwttack = startTimeBtwAttack;
            }
        }
    }

    void UpdateAnimations()
    {
        anim.SetBool("isAttacking", isAttacking);
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (attackingFront)
        {
            Gizmos.DrawWireCube(attackPos.position, attackRangeFront);
        }
        if (attackingUp)
        {
        Gizmos.DrawWireSphere(attackPos.position, attackRangeUp);
        }

    }
}
