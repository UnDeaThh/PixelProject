using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BossNeck : BossBase
{
    private State actualState;

    private Animator anim;
    private Transform playePos;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] GameObject water;

    private int consecutiveDoble;
    private int consecutivePunch;
    private int attackType;
    [SerializeField] int pushForce;

    private float distancePlayer;

    private float distanceAtack;
    private float distanceRangeAttack;
    [SerializeField] float movSpeed;
    [SerializeField] float minDistanceDobleAttack;
    [SerializeField] float plusDistanceRA;
    [SerializeField] float timeToAttack;
    private float cntTimeToAttack;
    [SerializeField] float timeStill;
    private float cntTimeStill;

    [SerializeField] float timeToRangeAttack;
    private float cntTimeToRangeAttack;

    [SerializeField] Vector2 atackArea;
    [SerializeField] Transform atackPos;
    [SerializeField] Transform handPos;

    private bool playerInFront;
    private bool canWalk;
    private bool doDobleAttack = false;
    private bool punchAttack;
    private bool doRangeAttack;

    [SerializeField] GameObject parryParticle;
    public State ActualState { get => actualState; set => actualState = value; }
    public bool DoDobleAttack { get => doDobleAttack; set => doDobleAttack = value; }
    public bool DoRangeAttack { get => doRangeAttack; set => doRangeAttack = value; }
    public bool PunchAttack { get => punchAttack; set => punchAttack = value; }


    [SerializeField] float timeFreez;
    private float cntTimeFreze;
    [SerializeField] GameObject cameraFight;
    private bool oneShake;
    private float cntTimeShaking;



    [Header("Sonidos")]
    [SerializeField] AudioSource stepSounds;

    private void Awake()
    {
        if (ScenesManager.scenesManager.BossKilled[nBoss])
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        mat = sprite.material;
        col = GetComponent<Collider2D>();
        anim = GetComponentInChildren<Animator>();
        actualState = State.Nothing;
        playePos = GameObject.FindGameObjectWithTag("Player").transform;

        facingDir = -1;
        cntTimeToAttack = timeToAttack;
        cntTimeToRangeAttack = timeToRangeAttack;
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), playePos.GetComponent<Collider2D>());

        cntTimeFreze = timeFreez;
        cntTimeShaking = timeShaking;
    }

    // Update is called once per frame
    void Update()
    {
        if(nLifes > 0)
        {
            switch (actualState)
            {
                case State.Enter: // La animacion al entrar en la sala
                    actualState = State.H1;
                    break;
                case State.H1: // Aqui meteremos todo el comportamiento del NeckBoss ya que no va por fases, sino por situaciones
                    if(!doDobleAttack && !doRangeAttack)
                    {
                        if(facingDir < 0 && playePos.position.x > transform.position.x)
                        {
                            Flip();
                        }
                        else if(facingDir > 0 && playePos.position.x < transform.position.x)
                        {
                            Flip();
                        }
                    }

                    distancePlayer = Vector2.Distance(transform.position, playePos.position);


                    if (facingDir > 0)
                    {
                        distanceAtack = Vector2.Distance(transform.position, new Vector3(atackPos.position.x + atackArea.x / 2, transform.position.y, atackPos.position.z));
                        distanceRangeAttack = Vector2.Distance(transform.position, new Vector2(atackPos.position.x + atackArea.x + plusDistanceRA, transform.position.y));
                    }
                    else if (facingDir < 0)
                    {
                        distanceAtack = Vector2.Distance(transform.position, new Vector3(atackPos.position.x - atackArea.x / 2, transform.position.y, atackPos.position.z));
                        distanceRangeAttack = Vector2.Distance(transform.position, new Vector2(atackPos.position.x - atackArea.x - plusDistanceRA, transform.position.y));
                    }

                    if (distancePlayer <= distanceAtack)
                    {
                        canWalk = false;
                        if(cntTimeStill != timeStill)
                        {
                            cntTimeStill = timeStill;
                        }
                        playerInFront = true;
                        if (!doDobleAttack && !punchAttack)
                        {
                            if (cntTimeToAttack > 0)
                            {
                                cntTimeToAttack -= Time.deltaTime;
                            }
                            else
                            {
                                attackType = Random.Range(1, 3);
                                if(attackType == 1 && consecutiveDoble > 3)
                                {
                                    attackType = 2;
                                }
                                else if( attackType == 2 && consecutivePunch > 2)
                                {
                                    attackType = 1;
                                }

                                if(attackType == 1)
                                {
                                    doDobleAttack = true;
                                    consecutiveDoble++;
                                    consecutivePunch = 0;
                                }
                                else if(attackType == 2)
                                {
                                    punchAttack = true;
                                    consecutivePunch++;
                                    consecutiveDoble = 0;
                                }
                                cntTimeToAttack = timeToAttack;
                            }
                        }
                    }
                    else
                    {
                        playerInFront = false;
                        if(distancePlayer >= distanceRangeAttack)
                        {
                            if (!doRangeAttack)
                            {
                                if(cntTimeToRangeAttack > 0)
                                {
                                    cntTimeToRangeAttack -= Time.deltaTime;
                                }
                                else
                                {
                                    doRangeAttack = true;
                                    cntTimeToRangeAttack = timeToRangeAttack;
                                }
                            }
                        }

                        else // distancia mayor que closeRange pero menor que highRange
                        {
                            if(!doDobleAttack && !doRangeAttack)
                            {
                                if(cntTimeStill > 0)
                                {
                                    cntTimeStill -= Time.deltaTime;
                                }
                                else
                                {
                                    canWalk = true;
                                }
                            }
                        }
                    }
                    break;
            }
        }
        else
        {
            if(cntTimeFreze > 0)
            {
                AudioManager.instanceAudio.EndBossSong = true;
                cntTimeFreze -= Time.deltaTime;
                col.enabled = false;
                rb.gravityScale = 0f;
                anim.speed = 0;
                if(cntTimeFreze < 0.15f)
                {
                    cameraFight.SetActive(false);
                }
            }
            else
            {
                if (!oneShake)
                {
                    CameraController.cameraController.GenerateCamerashake(amplitudeShaking, frequencyShaking, timeShaking);
                    oneShake = true;
                }
                if(cntTimeShaking > 0)
                {
                    cntTimeShaking -= Time.deltaTime;
                    if(cntTimeShaking < timeShaking / 3)
                    {
                        if (!oneCallDead)
                        {
                            anim.speed = 1;
                            anim.SetBool("isDead", true);
                            oneCallDead = true;
                        }
                    }
                }
                Dead();
            }
        }
        UpdateSounds();
        UpdateAnims();
    }

    private void FixedUpdate()
    {
        if(actualState == State.H1)
        {
            if (canWalk)
            {
                rb.velocity = new Vector2(movSpeed * Time.deltaTime * facingDir, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }
    }
    public void CloseAttack()
    {
        Collider2D col = Physics2D.OverlapBox(atackPos.position, atackArea, 0, playerLayer);
        if (col != null)
        {
            PlayerController2 player = playePos.GetComponent<PlayerController2>();
            PlayerParry plParry = playePos.GetComponent<PlayerParry>();
            if (plParry.IsParry)
            {
                if(facingDir < 0)
                {
                    Vector2 pushDir = new Vector2(-1, 1f).normalized;
                    player.rb.AddForce( pushDir * pushForce);
                    PlayerParry();

                }
                else
                {
                    Vector2 pushDir = new Vector2(1, 1f).normalized;
                    player.rb.AddForce(pushDir * pushForce);
                    PlayerParry();
                }
                
            }
            else
            {
                col.gameObject.GetComponent<PlayerController2>().PlayerDamaged(1, transform.position);
            }
        }
    }

    public void ThrowWater()
    {
        Quaternion rotacionWater =  facingDir > 0 ?  Quaternion.identity :  new Quaternion(0f, 180f, 0f, 0f);
        Instantiate(water, new Vector2(handPos.position.x, -18.032f), rotacionWater);
    }

    private void UpdateSounds()
    {
        if(Mathf.Abs(rb.velocity.x) > 0.1f)
        {
            stepSounds.pitch = Random.Range(0.8f, 1.15f);
            if (!stepSounds.isPlaying)
            {
                stepSounds.Play();
            }
        }
    }
    private void UpdateAnims()
    {
        anim.SetBool("dobleAttack", doDobleAttack);
        anim.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));
        anim.SetBool("rangeAttack", doRangeAttack);
        anim.SetBool("punchAttack", punchAttack);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(atackPos.position, atackArea); //Collider ataque doble

        if(facingDir > 0)
        {
            Gizmos.DrawLine(transform.position, new Vector3(atackPos.position.x + atackArea.x/2, transform.position.y, atackPos.position.z));
            Gizmos.DrawLine(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), new Vector3(atackPos.position.x + atackArea.x + plusDistanceRA, transform.position.y + 1, atackPos.position.z));
        }
        else if(facingDir < 0)
        {
            Gizmos.DrawLine(transform.position, new Vector3(atackPos.position.x - atackArea.x/2, transform.position.y, atackPos.position.z));
            Gizmos.DrawLine(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), new Vector3(atackPos.position.x - atackArea.x - plusDistanceRA, transform.position.y + 1 , atackPos.position.z));
        }
    }

    private void PlayerParry()
    {
        Instantiate(parryParticle, playePos.position, Quaternion.identity);
    }
}
