using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TatzelAI : BaseEnemy
{
    private Rigidbody2D rb;
    private bool groundFound;
    private bool wallFound;
    private bool playerFoundedBack = false;
    private bool ffBack;
    private bool ffAttack;
    private bool playerFoundedFront;
    private bool playerFounded;
    private bool playerJustInFront;

    private bool canAttack = false;
    private bool attack;

    private bool makeOneattack;
    public bool MakeOneattack { get => makeOneattack; set => makeOneattack = value; }

    [SerializeField] float backDistance;
    [SerializeField] float frontDistance;
    private float distancia;
    [SerializeField] float runingSpeed;
    [SerializeField] float maxDistanceToPlayer;
    [SerializeField] float groundDistance;
    [SerializeField] float checkDistance;
    [SerializeField] float maxHeight;

    [SerializeField] float timeBtwAttacks = 2f;
    [SerializeField] float cntTimeBtwAttacks;


    [SerializeField] Transform groundCheckerPos;
    [SerializeField] Transform frontCheckerPos;
    [SerializeField] Transform backChecker;
    [SerializeField] Transform frontChecker;
    [SerializeField] Transform attackPos;

    [SerializeField] Vector2 frontArea;
    [SerializeField] Vector2 backArea;
    [SerializeField] Vector2 attackRange;
    [SerializeField] LayerMask playerMask;

    private Transform player;
    private PlayerParry plParry;

    private int facingDir;

    [SerializeField] AudioSource source;

    public bool Attack1 { get => attack; set => attack = value; }
   

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreLayerCollision(9, 9);
        anim = GetComponentInChildren<Animator>();
        mat = GetComponentInChildren<SpriteRenderer>().material;
        sprite = GetComponentInChildren<SpriteRenderer>();

        int randomNumber = 0;
        while(randomNumber == 0)
        {
            randomNumber = Random.Range(-1, 1);
        }

        facingDir = randomNumber;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        plParry = player.gameObject.GetComponent<PlayerParry>();
    }

    // Update is called once per frame
    void Update()
    {
        if(nLifes > 0)
        {
            CheckEnvironment();
            PlayerAlreadyDetected();
            CheckIfCanAttack();

            if (makeOneattack)
            {
                source.pitch = Random.Range(0.8f, 1.15f);
                source.Play();
                Collider2D col = Physics2D.OverlapBox(attackPos.position, attackRange, 0, playerMask);
                if (col != null)
                {
                    if (player.gameObject.GetComponent<PlayerParry>().IsParry)
                    {
                        StartStun();
                    }
                    else
                    {
                        col.gameObject.GetComponent<PlayerController2>().PlayerDamaged(damage, transform.position); //Pongo los vectores al reves ya que en el metodo le doy la vuelta
                    }
                }
                makeOneattack = false;
            }

            UpdateAnimations();
            Stuned();
        }

        Dead();
    }

    private void FixedUpdate()
    {
        AplyMovement();
    }



    void CheckEnvironment()
    {
        groundFound = Physics2D.Raycast(frontCheckerPos.position, Vector2.down, checkDistance, whatIsDetected);
        wallFound = Physics2D.Raycast(frontCheckerPos.position, transform.right, checkDistance, whatIsDetected);


        playerFoundedFront = Physics2D.OverlapBox(frontChecker.position, frontArea, 0, playerMask);

        playerFoundedBack = Physics2D.OverlapBox(backChecker.position, backArea, 0, playerMask);
        if (playerFoundedBack)
        {
            ffBack = true;
        }
 /*
        if(facingDir > 0)
        {
            if(player.position.x > transform.position.x && player.position.x <= transform.position.x + frontDistance)
            {
                playerFoundedFront = true;
            }
            else
            {
                playerFoundedFront = false;
            }
        }
        else
        {
            if(player.position.x < transform.position.x && player.position.x >= transform.position.x - frontDistance)
            {
                playerFoundedFront = true;
            }
            else
            {
                playerFoundedFront = false;
            }
        }
        */

        if (playerFoundedBack || playerFoundedFront)
        {
            playerFounded = true;
        }
        else if(!playerFoundedBack && !playerFoundedFront)
        {
            playerFounded = false;
        }
    }
    void PlayerAlreadyDetected()
    {
        if (playerFounded)
        {
            distancia = Vector2.Distance(transform.position, player.position);

            if (distancia > maxDistanceToPlayer)
            {
                playerJustInFront = false;
            }
            else
            {
                playerJustInFront = true;
                if (!ffAttack)
                {
                    cntTimeBtwAttacks = 0.1f;
                    ffAttack = true;
                }
            }
        }
        else
        {
            playerJustInFront = false;
        }
    }

    void AplyMovement()
    {
        if (!isStuned)
        {
            if (!playerFounded)
            {
                if(groundFound && !wallFound)
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
                if (playerFoundedFront && !playerJustInFront)
                {
                    if(!wallFound && groundFound)
                    {
                        if (!attack)
                        {
                            rb.velocity = new Vector2(facingDir * runingSpeed * Time.deltaTime, rb.velocity.y);
                        }
                    }
                    else
                    {
                        rb.velocity = Vector2.zero;
                    }
                }

                if (playerJustInFront)
                {
                    rb.velocity = Vector2.zero;
                }
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }


    void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
        facingDir *= -1;
    }

    void CheckIfCanAttack()
    {
        if (playerJustInFront)
        {
            if(cntTimeBtwAttacks > 0)
            {
                cntTimeBtwAttacks -= Time.deltaTime;
            }
            else
            {
                attack = true;
                cntTimeBtwAttacks = timeBtwAttacks;
            }
        }
    }

    public override void Stuned()
    {
        if (isStuned)
        {
            if(cntTimeStuned > 0)
            {
                cntTimeStuned -= Time.deltaTime;
            }
            else
            {
                isStuned = false;
            }
        }
    }

    public override void StartStun()
    {
        isStuned = true;
        attack = false;
        makeOneattack = false;
        plParry.CallParry();
        cntTimeStuned = timeStuned;
    }

    public override void Dead()
    {
        if (nLifes <= 0 && !oneCallDead)
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
        base.Dead();
    }

    void UpdateAnimations()
    {
        if(Mathf.Abs(rb.velocity.x) > 0.1f)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }

        anim.SetBool("attack", attack);
        anim.SetBool("isStuned", isStuned);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController2>().PlayerDamaged(damage, transform.position);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCheckerPos.position, new Vector3(groundCheckerPos.position.x, groundCheckerPos.position.y - groundDistance, transform.position.z));
        Gizmos.DrawLine(frontCheckerPos.position, new Vector3(frontCheckerPos.position.x, frontCheckerPos.position.y - checkDistance, transform.position.z));

        if (facingDir == 1)
        {
            Gizmos.DrawLine(frontCheckerPos.position, new Vector3(frontCheckerPos.position.x + checkDistance, frontCheckerPos.position.y, transform.position.z));
        }
        else
        {
            Gizmos.DrawLine(frontCheckerPos.position, new Vector3(frontCheckerPos.position.x - checkDistance, frontCheckerPos.position.y, transform.position.z));
        }


      //  Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + frontDistance * facingDir, transform.position.y, 0));
        

        Gizmos.DrawWireCube(backChecker.position, backArea);
        Gizmos.DrawWireCube(frontChecker.position, frontArea);

        Gizmos.DrawWireCube(attackPos.position, attackRange);
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + maxHeight));
    }
}
