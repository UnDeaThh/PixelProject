﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChange : BossBase
{
    private State actualState;
    public State ActualState { get => actualState; set => actualState = value; }
    public bool ThrowBalls { get => throwBalls; set => throwBalls = value; }
    public float TimeThrowH3 { get => timeThrowH3; }
    public float CntTimeToThrowH3 { get => cntTimeToThrowH3; set => cntTimeToThrowH3 = value; }

    [SerializeField] AudioClip[] clips;

    bool hasSounded = false;

    [SerializeField] Vector2 movDir;
    private Transform player;
    [SerializeField] GameObject cameraFight;

    [Header("H1")]
    [SerializeField] private float movSpeedH1 = 700;
    [SerializeField] float timeH1;
    float cntTime;
    [SerializeField] AudioSource hitGndSound;
    [SerializeField] GameObject impactGrounsParticle;

    [Header("Transition")]
    [SerializeField] float movSpeedTransition = 200;
    [SerializeField] float timeTransition = 3;
    private int transitionFases = 0;
    float cntTimeTransition;
    [SerializeField] Transform altitudeForH2;
    private int comeFrom = 1;

    [Header("H2")]
    [SerializeField] float timeDoingH2;
    [SerializeField] float movSpeedH3;
    [SerializeField] float timeToSpit;
    float cntTimeToSpit;
    [SerializeField] GameObject potionBall;
    [SerializeField] Transform mouth;

    [Header("H3")]
    [SerializeField] int nSeries;
    int cntSeries;
    private bool throwBalls;
    [SerializeField] float timeThrowH3;
    [SerializeField] GameObject ballH3Prefab;
    [SerializeField] int maxBallsH3;
    float cntTimeToThrowH3;
    Animator anim;

    [Header("DEAD")]
    [SerializeField] float timeFreeze;
    private float cntTimeFreeze;

    float cntTimeShaking;
    bool oneShake = false;
    [SerializeField] AudioSource earthquakeSound;

    private void Awake()
    {
        if (ScenesManager.scenesManager.BossKilled[nBoss])
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        actualState = State.Nothing;
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        mat = sprite.material;
        col = GetComponent<Collider2D>();
        anim = GetComponentInChildren<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        cameraFight.SetActive(false);

        cntTimeFreeze = timeFreeze;
        cntTimeShaking = timeShaking;
        AudioManager.instanceAudio.StartBossSong = false;
        AudioManager.instanceAudio.PlayedFirstBossSong = false;
        AudioManager.instanceAudio.EndBossSong = false;
    }

    private void Update()
    {
        if(rb.velocity.x > 0.1 && facingDir < 0)
        {
            Flip();
        }
        else if(rb.velocity.x < -0.1f && facingDir > 0)
        {
            Flip();
        }
        movDir.Normalize();
        if(nLifes > 0)
        {
            switch (actualState)
            {
          
                case State.Enter:
                    if (!audioSource.isPlaying && !hasSounded)
                    {
                        CameraController.cameraController.IsGenerateCS = true;
                        CameraController.cameraController.GenerateCamerashake(8, 3, clips[0].length - 0.3f);
                        audioSource.clip = clips[0];
                        audioSource.Play();
                        hasSounded = true;
                    }
                    if(!audioSource.isPlaying && hasSounded)
                    {
                        cameraFight.SetActive(true);
                        AudioManager.instanceAudio.StartBossSong = true;
                        cntTime = timeH1;
                        if(nLifes > 20)
                        {
                            nLifes = 20;
                        }
                        actualState = State.H1;
                    }
                    break;
                case State.H1:
                    if(cntTime > 0)
                    {
                        cntTime -= Time.deltaTime;
                    }
                    else
                    {
                        cntTime = 0;
                        cntTimeTransition = 1.5f;
                        transitionFases = 1;
                        comeFrom = 1;
                        actualState = State.Transition;
                    }
                    break;
                case State.H2:
                    if(cntTime > 0)
                    {
                        cntTime -= Time.deltaTime;
                        if(cntTimeToSpit > 0)
                        {
                            cntTimeToSpit -= Time.deltaTime;
                        }
                        else
                        {
                            Instantiate(potionBall, mouth.position, Quaternion.identity);
                            audioSource.clip = clips[1];
                            audioSource.Play();
                            cntTimeToSpit = timeToSpit;
                        }
                    }
                    else
                    {
                        transitionFases = 1;
                        cntTimeTransition = 1.5f;
                        comeFrom = 2;
                        actualState = State.Transition;
                    }
                    break;
                case State.H3:
                    if(cntTimeToThrowH3 > 0)
                    {
                        cntTimeToThrowH3 -= Time.deltaTime;
                    }
                    else
                    {
                        anim.SetBool("makeH3", true);
                        cntTimeToThrowH3 = timeThrowH3 + 10;
                    }

                    if (throwBalls)
                    {
                        for (int i = 0; i < maxBallsH3; i++)
                        {
                            Instantiate(ballH3Prefab, transform.position, Quaternion.Euler(0f, 0f, Random.Range(0 , 359)));
                        }
                        cntSeries++;
                        throwBalls = false;
                    }

                    if (cntSeries == nSeries && anim.GetBool("makeH3") == false)
                    {
                        transitionFases = 1;
                        cntTimeTransition = 1.5f;
                        comeFrom = 3;
                        actualState = State.Transition;
                    }
                    break;
                case State.Transition:
                    if(transitionFases == 1)
                    {
                        if(cntTimeTransition > 0)
                        {
                            cntTimeTransition -= Time.deltaTime;
                        }
                        else
                        {
                            CalculNewDirection();
                            cntTimeTransition = timeTransition;
                            transitionFases = 2;
                        }
                    }
                    else if(transitionFases == 2)
                    {
                        if(cntTimeTransition > 0)
                        {
                            cntTimeTransition -= Time.deltaTime;
                        }
                        else
                        {
                            transitionFases = 3;
                            cntTimeTransition = 0;
                        }
                    }
                    else if(transitionFases == 3)
                    {
                        if(comeFrom != 3)
                        {
                            if(transform.position.y > altitudeForH2.position.y - 0.3 && transform.position.y < altitudeForH2.position.y + 0.3f)
                            {
                                if(comeFrom == 1)
                                {

                                    if (transform.position.x < altitudeForH2.position.x)
                                    {
                                        movDir = Vector2.right;
                                    }
                                    else
                                    {
                                        movDir = Vector2.left;
                                    }
                                    cntTime = timeDoingH2;
                                    cntTimeToSpit = timeToSpit;
                                    actualState = State.H2;
                                }
                                else if(comeFrom == 2)
                                {
                                    cntTimeToThrowH3 = 0.7f;
                                    cntSeries = 0;
                                    actualState = State.H3;
                                }
                                Debug.Log(movDir);
                            }
                            else
                            {
                                Vector2 dir;
                                dir = new Vector2(altitudeForH2.position.x - transform.position.x, altitudeForH2.position.y - transform.position.y).normalized;
                                movDir = dir;
                            }
                        }
                        else
                        {
                            cntTime = timeH1;
                            actualState = State.H1;
                        }
                    }
                    break;
                case State.Dead:
                    break;
                case State.Nothing:
                    break;
                default:
                    break;
            }
        }
        else
        {
            if(cntTimeFreeze > 0)
            {
                AudioManager.instanceAudio.EndBossSong = true;
                cntTimeFreeze -= Time.deltaTime;
                col.enabled = false;
                anim.speed = 0;
                if(cntTimeFreeze < 0.15f)
                {
                    cameraFight.SetActive(false);
                }
            }
            else
            {
                if (!oneShake)
                {
                    earthquakeSound.Play();
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
                        if(earthquakeSound.volume > 0)
                        {
                            earthquakeSound.volume -= 0.008f;
                        }
                    }
                }
                Dead();
            }
        }
    }
    private void FixedUpdate()
    {
        if(nLifes > 0)
        {
            switch (actualState)
            {
                case State.Enter:
                    break;
                case State.H1:
                    if(cntTime > 0)
                    {
                        rb.velocity = movDir * movSpeedH1 * Time.deltaTime;
                    }
                    else
                    {
                        rb.velocity = Vector2.zero;
                    }
                    break;
                case State.H2:
                    rb.velocity = movDir * movSpeedH3 * Time.deltaTime;
                    break;
                case State.H3:
                    rb.velocity = Vector2.zero;
                    break;
                case State.Transition:
                    if(transitionFases == 1)
                    {
                        rb.velocity = Vector2.zero;
                    }
                    else if(transitionFases == 2)
                    {
                        if(cntTimeTransition > 0)
                        {
                            rb.velocity = movDir * movSpeedTransition * Time.deltaTime;
                        }
                        else
                        {
                            rb.velocity = Vector2.zero;
                        }
                    }
                    else
                    {
                        if(comeFrom != 3)
                        {
                            if(transform.position.y > altitudeForH2.position.y - 0.3f && transform.position.y < altitudeForH2.position.y + 0.3f)
                            {
                                rb.velocity = Vector2.zero;
                            }
                            else
                            {
                                rb.velocity = movDir * movSpeedTransition * Time.deltaTime;
                            }
                        }
                    }
                    break;
                case State.Dead:
                    break;
                case State.Nothing:
                    break;
                default:
                    break;
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
    void CalculNewDirection()
    {
        if(transform.position.x < player.position.x)
        {
            movDir = new Vector2(Random.Range(0f, 1f), Random.Range(-0.5f, 0.5f));
        }
        else
        {
            movDir = new Vector2(Random.Range(-1f, 0f), Random.Range(-0.5f, 0.5f));
        }
        Debug.Log(movDir);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            player.GetComponent<PlayerController2>().PlayerDamaged(1, transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Floor"))
        {
            if (actualState == State.H1)
            {
                if (other.transform.name == "LimitDown")
                {
                    movDir.y *= -1;
                    Instantiate(impactGrounsParticle, transform.position + new Vector3(0f, -2f, 0f), Quaternion.identity);
                    hitGndSound.Play();
                    CameraController.cameraController.GenerateCamerashake(4, 10, 0.2f);
                }
                else if (other.transform.name == "LimitUp")
                {
                    movDir.y *= -1;
                    Instantiate(impactGrounsParticle, transform.position + new Vector3(0f, +2f, 0f), Quaternion.identity);
                    hitGndSound.Play();
                    CameraController.cameraController.GenerateCamerashake(4, 10, 0.2f);
                }
                else if (other.transform.name == "LimitLeft")
                {
                    movDir.x *= -1;
                    Flip();
                }
                else if (other.transform.name == "LimitRight")
                {
                    movDir.x *= -1;
                    Flip();
                }
            }
            else
            {
                if (other.transform.name == "LimitDown")
                {
                    movDir.y *= -1;
                }
                else if (other.transform.name == "LimitUp")
                {
                    movDir.y *= -1;
                }
                else if (other.transform.name == "LimitLeft")
                {
                    movDir.x *= -1;
                    Flip();
                }
                else if (other.transform.name == "LimitRight")
                {
                    movDir.x *= -1;
                    Flip();
                }
            }
        }
    }
}
