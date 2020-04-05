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

    public LayerMask whatIsEnemie;

    private CameraController cameraController;
    private PlayerAudio sound;

    public bool CanSecondAttack { get => canSecondAttack; set => canSecondAttack = value; }

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
        if(canSecondAttack && isAttacking && nClicks > 1)
        {
            Debug.Log("FUNCIONA");
        }

        if(nClicks >= 2)
        {
            nClicks = 2;
        }
    }

    void CheckIfCanAttack()
    {
        if (!player.isOnKinematic)
        {
            if (haveSword)
            {
                if(!player.isDrinking && ! player.isWallSliding && !plParry.isParry && !player.isDashing)
                {
                    if (!isAttacking)
                    {
                        canAttack = true;
                    }
                    else
                    {
                        if (canSecondAttack)
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
                    //clickAttack = false;
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
        if (plParry.parrySuccesful)
        {
            damage += 1;
        }

        if (canAttack)
        {
            if (inputs.Controls.Attack.triggered) 
            {
                #region FRONT ATTACK
                if (attackDirection.y <= 0.1f)
                {
                    
                    //FRONT ATTACK
                    if (player.isGrounded)
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
                            cameraController.letsShake = true;
                            if (enemiesToDamage[i].GetComponent<BaseEnemy>())
                            {
                                enemiesToDamage[i].GetComponent<BaseEnemy>().TakeDamage(damage);
                                plParry.parrySuccesful = false;
                            }
                        }
                        else if (enemiesToDamage[i].CompareTag("Nerbuz"))
                        {
                            if (enemiesToDamage[i].GetComponent<NerbuzBoss>().canBeDamaged)
                            {
                                enemiesToDamage[i].GetComponent<NerbuzBoss>().TakeDamge(damage);
                            }
                            plParry.parrySuccesful = false;
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

                    player.heedArrows = false;
                    if (player.isGrounded)
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

                    Vector3 finalPos = playerPos.position + upAttackPos;

                    attackPos.position = finalPos;
                    Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRangeUp, whatIsEnemie);

                    Debug.Log(enemiesToDamage.Length);
                    for (int i = 0; i < enemiesToDamage.Length; i++)
                    {
                        if (enemiesToDamage[i].CompareTag("Enemy"))
                        {
                            cameraController.letsShake = true;
                            if (enemiesToDamage[i].GetComponent<BaseEnemy>())
                            {
                                enemiesToDamage[i].GetComponent<BaseEnemy>().TakeDamage(damage);
                                plParry.parrySuccesful = false;
                            }
                        }
                        else if (enemiesToDamage[i].CompareTag("Nerbuz"))
                        {
                            if (enemiesToDamage[i].GetComponent<NerbuzBoss>().canBeDamaged)
                            {
                                enemiesToDamage[i].GetComponent<NerbuzBoss>().TakeDamge(damage);
                            }
                            plParry.parrySuccesful = false;
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

                if (canSecondAttack)
                {
                    canSecondAttack = false;
                }
               // clickAttack = false;
            } 
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        // if (gndAttackingFront)
        // {
        Gizmos.DrawWireCube(attackPos.position, attackRangeFront);
        //}
        if (gndAttackingUp)
        {
        Gizmos.DrawWireSphere(attackPos.position, attackRangeUp);
        }

    }
}
