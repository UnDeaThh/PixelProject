using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeckAI : BaseEnemy
{
    private int facingDir = 1;
    [SerializeField] float environmentCheckDistance;
    [SerializeField] float timeBtwAttacks;
    [SerializeField] float detectionDistance;
    [SerializeField] float playerLosedTime = 0.3f;
    private float cntLosedTime;
    private float cntTimeBtwAttacks;

    [SerializeField] Vector2 attackRange;
    [SerializeField] Vector2 firstAttackRange;
    [SerializeField] Vector2 backArea;
    [SerializeField] Transform firstAttackPos;
    [SerializeField] Transform environmentLocatorPos;
    [SerializeField] Transform attackPos;
    [SerializeField] Transform backPos;
    [SerializeField] Collider2D colTrigger;
    [SerializeField] LayerMask floorMask;

    public bool firstAttack = false;
    private bool firstAttackFinished;
    private bool ffAtack = false;
    private bool groundInFront;
    private bool wallInFront;
    private bool playerInFront = false;
    private bool playerFoundedBack;
    private bool ffBack;
    public bool isAttacking;
    public bool makeAnAttack;
    private bool canAttack;
    private bool ffFound;
    private bool playerFounded;


    [HideInInspector] public Rigidbody2D rb;
    private Transform player;
    [SerializeField] ParticleSystem ps;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        mat = GetComponentInChildren<SpriteRenderer>().material;
        sprite = GetComponentInChildren<SpriteRenderer>();
        sprite.enabled = false;
        Physics2D.IgnoreCollision(transform.GetComponent<Collider2D>(), GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>());
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if(nLifes > 0)
        {
            FirstAttack();
            CheckEnvironment();
            CanAttack();
            Attack();
        }

        Stuned();
        Dead();
    }
    private void FixedUpdate()
    {
        if(nLifes > 0)
        {
            ApplyMovement();
        }
    }
    void FirstAttack()
    {
        if (firstAttack && !ffAtack)
        {
            Collider2D col = Physics2D.OverlapBox(firstAttackPos.position, firstAttackRange, 0, whatIsDetected);
            if (col != null)
            {
                if (player.GetComponent<PlayerParry>().IsParry)
                {
                    StartStun();
                }
                else
                {
                    col.GetComponent<PlayerController2>().PlayerDamaged(damage, transform.position);
                }
            }
            ffAtack = true;
            firstAttackFinished = true;
        }
    }
    void CheckEnvironment()
    {
        if (firstAttackFinished)
        {
            groundInFront = Physics2D.Raycast(environmentLocatorPos.position, Vector2.down, environmentCheckDistance, floorMask);
            wallInFront = Physics2D.Raycast(environmentLocatorPos.position, transform.right, environmentCheckDistance, floorMask);
            playerInFront = Physics2D.Raycast(new Vector2(transform.position.x, attackPos.position.y), transform.right, detectionDistance, whatIsDetected);


            playerFoundedBack = Physics2D.OverlapBox(backPos.position, backArea, 0, whatIsDetected);

            if(facingDir > 0)
            {
                if(player.position.x < transform.position.x)
                {
                    if (playerFoundedBack)
                    {
                        ffBack = true;
                    }
                }
            }
            else
            {
                if(player.position.x > transform.position.x)
                {
                    if (playerFoundedBack)
                    {
                        ffBack = true;
                    }
                }
            }

            //Checkear si puede caminar
            if (playerInFront || playerFoundedBack)
            {
                playerFounded = true;
                ffFound = true;
            }
            else if(!playerInFront && !playerFoundedBack)
            {
                playerFounded = false;
            }

            if (!playerInFront && ffFound) // El player se aleja y esperas unos segundos para moverte
            {
                if(cntLosedTime > 0)
                {
                    cntLosedTime -= Time.deltaTime;
                }
                else
                {
                    cntLosedTime = playerLosedTime;
                    playerFounded = false;
                    ffFound = false;
                }
            }
        }
    }

    void ApplyMovement()
    {
        if (firstAttackFinished)
        {
            if (!isStuned)
            {
                if (!playerFounded)
                {
                    if (groundInFront && !wallInFront)
                    {
                        rb.velocity = new Vector2(facingDir * movSpeed * Time.deltaTime, rb.velocity.y);
                    }
                    else
                    {
                        Flip();
                    }

                }
                else
                {
                    if (ffBack)
                    {
                        Flip();
                        ffBack = false;
                    }
                }
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }
    }
    void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
        facingDir *= -1;
    }

    void CanAttack()
    {
        if (playerInFront && !isAttacking)
        {
            if(cntTimeBtwAttacks > 0f)
            {
                cntTimeBtwAttacks -= Time.deltaTime;
            }
            else
            {
                isAttacking = true;
                cntTimeBtwAttacks = timeBtwAttacks;
            }
        }
    }
    void Attack()
    {
        if (makeAnAttack)
        {
            Collider2D col = Physics2D.OverlapBox(attackPos.position, attackRange, 0, whatIsDetected);
            if(col != null)
            {
                if (player.GetComponent<PlayerParry>().IsParry)
                {
                    StartStun();
                }
                else
                {
                    col.gameObject.GetComponent<PlayerController2>().PlayerDamaged(damage, transform.position);
                }
            }
            makeAnAttack = false;
        }
    }

    public override void StartStun()
    {
        isStuned = true;
        isAttacking = false;
        makeAnAttack = false;
        player.GetComponent<PlayerParry>().CallParry();
        cntTimeStuned = timeStuned;
    }
    //El player pasa por encima del agua
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            sprite.enabled = true;
            anim.SetTrigger("playerAbove");
            colTrigger.enabled = false;
            ParticleSystem.EmissionModule em = ps.emission;
            em.enabled = false;
        }
    }

    public override void Dead()
    {
        base.Dead();
        if(nLifes <= 0)
        {
            rb.gravityScale = 0;
        }
    }

    public override void TakeDamage(int damage, Vector2 playerPos)
    {
        if (firstAttackFinished)
        {
            base.TakeDamage(damage, playerPos);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (!firstAttackFinished)
        {
            Gizmos.DrawWireCube(firstAttackPos.position, firstAttackRange);
        }

        Gizmos.DrawWireCube(backPos.position, backArea);
        Gizmos.DrawWireCube(attackPos.position, attackRange);
        Gizmos.color = Color.black;
        if (facingDir == 1)
        {
            Gizmos.DrawLine(environmentLocatorPos.position, new Vector3(environmentLocatorPos.position.x + environmentCheckDistance, environmentLocatorPos.position.y, transform.position.z));
            Gizmos.DrawLine(new Vector2(transform.position.x, attackPos.position.y), new Vector2(transform.position.x + detectionDistance, attackPos.position.y ));
        }
        else
        {
            Gizmos.DrawLine(environmentLocatorPos.position, new Vector3(environmentLocatorPos.position.x - environmentCheckDistance, environmentLocatorPos.position.y, transform.position.z));
            Gizmos.DrawLine(new Vector2(transform.position.x, attackPos.position.y), new Vector2(transform.position.x - detectionDistance, attackPos.position.y ));
        }
        Gizmos.DrawLine(environmentLocatorPos.position, new Vector3(environmentLocatorPos.position.x, environmentLocatorPos.position.y - environmentCheckDistance, transform.position.z));
    }
}
