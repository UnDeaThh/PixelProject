using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    PlayerInputs inputs;
    private PlayerController2 player;
    private PlayerParry plParry;
	private PauseManager pauseManager;
    private Inventory2 inventory;
    private AnimationController myAnimator;


    public int damage;

    public int nClicks = 0;

    public Vector2 attackRangeFront = new Vector2(3f, 2f);
    public Vector2 attackDirection;
    public float attackUpDistance = 1f;
    public float attackRangeUp = 3f;

    private bool clickAttack;
    public bool gndAttackingUp = false;
    public bool gndAttackingFront = false;
    public bool airAttackingUp = false;
    public bool airAttackingFront = false;
    public bool haveSword = false;
    [SerializeField]private bool canSecondAttack = false;

    private bool canAttack;
	public bool isAttacking = false;

    private Vector3 frontAttackPos = new Vector3(1.5f, 0.0f, 0.0f);
    private Vector3 upAttackPos;

    public Transform attackPos;
    private Transform playerPos;
    [SerializeField] Transform particlePos;

    public LayerMask whatIsEnemie;

    private CameraController cameraController;
    private PlayerAudio sound;

    public bool CanSecondAttack { get => canSecondAttack; set => canSecondAttack = value; }

    [SerializeField] GameObject[] hitParticle;
    [SerializeField] GameObject bigHitParticle;

    private void OnEnable()
    {
        inputs.Controls.Enable();
    }

    private void OnDisable()
    {
        inputs.Controls.Disable();
    }
    void Awake()
    {
        inputs = new PlayerInputs();
        //inputs.Controls.Attack.performed += ctx => clickAttack = true;
        inputs.Controls.AttackDirection.performed += ctx => attackDirection = ctx.ReadValue<Vector2>();
        inputs.Controls.AttackDirection.canceled += ctx => attackDirection = ctx.ReadValue<Vector2>();
        playerPos = GetComponent<Transform>();
        cameraController = GameObject.FindGameObjectWithTag("CameraManager").GetComponent<CameraController>();
        sound = GetComponentInChildren<PlayerAudio>();
        myAnimator = GetComponentInChildren<AnimationController>();
    }

    private void Start()
    {
        player = GetComponent<PlayerController2>();
        plParry = GetComponent<PlayerParry>();
		pauseManager = GameObject.FindGameObjectWithTag("PauseManager").GetComponent<PauseManager>();
        inventory = GetComponentInChildren<Inventory2>();

        upAttackPos = new Vector3(0.0f, attackUpDistance, 0.0f);
    }
    void Update()
    {
        if (!player.isGODmode)
        {
            if (!isAttacking)
            {
                nClicks = 0;
                canSecondAttack = false;
            }

            if(!player.isDead)
            {
		        if(!pauseManager.isPaused){
			        CheckIfCanAttack();
			        Attack();

                }
            }
            

        }
    }

    void CheckIfCanAttack()
    {
        if (!player.isOnKinematic)
        {
            if (haveSword)
            {
                if(!player.isDrinking && ! player.isWallSliding && !plParry.IsParry && !player.isDashing)
                {
                    if (!isAttacking)
                    {
                        canAttack = true;
                    }
                    else
                    {
                        if (canSecondAttack && nClicks < 2)
                        {
                            canAttack = true;
                        }
                        else
                        {
                            canAttack = false;
                        }
                    }
                }

                else
                {
                    canAttack = false;
                }
            }
            else
            {
                canAttack = false;
            }
        }
        else
        {
            canAttack = false;
        }

    }
    void Attack()
    {
        if (player.isWallSliding)
        {
            isAttacking = false;
        }
        if (inventory.swordPasive)
        {
            damage *= 2;
        }

        if (canAttack)
        {
            if (inputs.Controls.Attack.triggered) 
            {
                if (plParry.ParrySuccesful)
                {
                    damage += 1;
                }
                #region FRONT ATTACK
                if (attackDirection.y <= 0.1f)
                {
                    
                    //FRONT ATTACK
                    if (player.IsGrounded)
                    {
                        player.rb.velocity = Vector2.zero;
                        gndAttackingFront = true;
                        gndAttackingUp = false;
                        airAttackingFront = false;
                        airAttackingUp = false;
                        player.heedArrows = false;
                    }
                    else
                    {
                        gndAttackingFront = false;
                        gndAttackingUp = false;
                        airAttackingFront = true;
                        airAttackingUp = false;
                    }
                    isAttacking = true;
                    nClicks++;
                 

                    //Primero seteamos la posicion del collider
                    if (player.facingDir == 1)
                    {
                        Vector3 finalPos = playerPos.position + frontAttackPos;
                        attackPos.position = finalPos;
                    }
                    else if (player.facingDir == -1)
                    {
                        Vector3 finalPos = playerPos.position + (-frontAttackPos);
                        attackPos.position = finalPos;
                    }
                    //Luego creamos el Collider en ese sitio
                    Collider2D[] enemiesToDamage = Physics2D.OverlapBoxAll(attackPos.position, attackRangeFront, 0, whatIsEnemie);
                    for (int i = 0; i < enemiesToDamage.Length; i++)
                    {
                        if (enemiesToDamage[i].tag == "Enemy")
                        {
                            cameraController.GenerateCamerashake(0.3f, 0.5f, 0.3f);
                            if (enemiesToDamage[i].GetComponent<BaseEnemy>())
                            {
                                enemiesToDamage[i].GetComponent<BaseEnemy>().TakeDamage(damage, transform.position);

                                Vector3 randomPosParticle = new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), 0f);
                                if (plParry.ParrySuccesful)
                                {
                                    cameraController.CallHitAfterParry();
                                    Instantiate(bigHitParticle, particlePos.position + randomPosParticle, Quaternion.identity);
                                }
                                else
                                {
                                    int randomParticle = Random.Range(0, 2);
                                    Instantiate(hitParticle[randomParticle], particlePos.position + randomPosParticle, Quaternion.identity);
                                }
                                plParry.ParrySuccesful = false;
                            }
                        }

                        else if (enemiesToDamage[i].GetComponent<BossBase>())
                        {
                            enemiesToDamage[i].GetComponent<BossBase>().TakeDamage(damage);

                            Vector3 randomPosParticle = new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), 0f);
                            if (plParry.ParrySuccesful)
                            {
                                cameraController.CallHitAfterParry();
                                Instantiate(bigHitParticle, particlePos.position + randomPosParticle, Quaternion.identity);
                            }
                            else
                            {
                                int randomParticle = Random.Range(0, 2);
                                Instantiate(hitParticle[randomParticle], particlePos.position + randomPosParticle, Quaternion.identity);
                            }
                            plParry.ParrySuccesful = false;
                        }



                        else if (enemiesToDamage[i].CompareTag("CumuloEsencia"))
                        {
                            enemiesToDamage[i].GetComponent<CumuloEsencia>().TakeDamage();
                        }
                        else if (enemiesToDamage[i].CompareTag("Palanca"))
                        {
                            enemiesToDamage[i].GetComponent<Palanca>().OpenDoor();
                        }

                    }

                    if (enemiesToDamage.Length <= 0)
                    {
                        sound.attackSound.clip = sound.attackClips[0];
                        sound.attackSound.pitch = Random.Range(0.80f, 1.15f);
                        sound.attackSound.volume = Random.Range(0.80f, 1.1f);
                        sound.attackSound.Play();
                    }
                    else
                    {
                        sound.attackSound.clip = sound.attackClips[1];
                        sound.attackSound.pitch = Random.Range(0.8f, 1.15f);
                        sound.attackSound.volume = Random.Range(0.8f, 1.15f);
                        sound.attackSound.Play();
                    }
                }
                #endregion
                #region UP ATTACK
                //UP ATTACK
                else if (attackDirection.y > 0.1f)
                {

                    if (player.IsGrounded)
                    {
                        player.rb.velocity = Vector2.zero;
                        gndAttackingFront = false;
                        gndAttackingUp = true;
                        airAttackingFront = false;
                        airAttackingUp = false;
                        player.heedArrows = false;
                    }
                    else
                    {
                        gndAttackingFront = false;
                        gndAttackingUp = false;
                        airAttackingFront = false;
                        airAttackingUp = true;
                    }
                    isAttacking = true;
                    nClicks++;

                    Vector3 finalPos = playerPos.position + upAttackPos;

                    attackPos.position = finalPos;
                    Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRangeUp, whatIsEnemie);

                    Debug.Log(enemiesToDamage.Length);
                    for (int i = 0; i < enemiesToDamage.Length; i++)
                    {
                        if (enemiesToDamage[i].CompareTag("Enemy"))
                        {
                            cameraController.GenerateCamerashake(0.3f, 0.5f, 0.3f);
                            if (enemiesToDamage[i].GetComponent<BaseEnemy>())
                            {
                                enemiesToDamage[i].GetComponent<BaseEnemy>().TakeDamage(damage, transform.position);
                                plParry.ParrySuccesful = false;
                            }
                        }

                        else if (enemiesToDamage[i].GetComponent<BossBase>())
                        {
                            enemiesToDamage[i].GetComponent<BossBase>().TakeDamage(damage);

                            Vector3 randomPosParticle = new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), 0f);
                            if (plParry.ParrySuccesful)
                            {
                                cameraController.CallHitAfterParry();
                                Instantiate(bigHitParticle, attackPos.position + randomPosParticle, Quaternion.identity);
                            }
                            else
                            {
                                int randomParticle = Random.Range(0, 2);
                                Instantiate(hitParticle[randomParticle], particlePos.position + randomPosParticle, Quaternion.identity);
                            }
                            plParry.ParrySuccesful = false;
                        }

                        else if (enemiesToDamage[i].CompareTag("CumuloEsencia"))
                        {
                            enemiesToDamage[i].GetComponent<CumuloEsencia>().TakeDamage();
                        }
                    }

                    if (enemiesToDamage.Length <= 0)
                    {
                        sound.attackSound.clip = sound.attackClips[0];
                        sound.attackSound.pitch = Random.Range(0.80f, 1.15f);
                        sound.attackSound.Play();
                    }
                    else
                    {
                        sound.attackSound.clip = sound.attackClips[1];
                        sound.attackSound.pitch = Random.Range(0.80f, 1.15f);
                        sound.attackSound.Play();
                    }
                }
                #endregion
               // clickAttack = false;
            } 
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPos.position, attackRangeFront);
        if (gndAttackingUp)
        {
        Gizmos.DrawWireSphere(attackPos.position, attackRangeUp);
        }

    }
}
