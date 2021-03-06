﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public enum State
    {
        Enter, H1,H2,H3,H4,Transition,Dead, Nothing
    }
public class NerbuzBoss : MonoBehaviour
{


    public State actualState;
    
    public bool canBeDamaged = true;
    public int life = 200;
    [Header("Hechizo1")]
    public float speedH1;
    public int particlesToSpawn;
    private int cntParticlesSpawn = 0;
    public float timeToSpawn;
    private float cntTimeToSpawn;
    public GameObject particleH1Prefab;
    private Vector2 movDir;

    [Header("Hechizo2")]
    public GameObject groundParticle;
    public GameObject pinchosPrefab;
    public bool isTired;
    public int nGenerators;
    public int numberOfAttacksH2 = 3;
    private int cntSeriesAttackH2 = 0;
    public int seriesAttackH2 = 3;
    [HideInInspector] public int cntAttacksH2 = 0;
    private Vector2[] generatorPos;
    public bool generatorInPlace = false;
    public bool canAttackH2;
    public float timeToAttackH2;
    private float cntTimeToAttackH2;
    public float tiredTime = 5f;
    private float cntTiredTime;
    public Transform cansadaPos;
    private bool reachedCansadaPos = false;
    public AudioSource earthquakeSound;

    [Header("Hechizo3")]
    [SerializeField] GameObject spellH3Prefab;
    public float timeDoingH3;
    private float cntTimeDoingH3;
    private bool alreadyActivatedH3 = false;
    [SerializeField] float timeTiredForH3;
    private float cntTimeTiredForH3;
    private bool finishSpawnH3 = false;


    [Header("Hechizo4")]
    [SerializeField] float positionH4;
    private Vector3 finalPosH4;

    [Header("Transitions")]
    public int transitions = 1;
    [Header("Transition 1")]
    public GameObject shieldPrefab;
    public bool centerReached;
    private bool shieldActivated;
    public Transform centerPosition;
    [Header("Transition3")]
    private float timeToH3 = 2f;
    private float cntTimeToH3;

    public Collider2D colH1Confiner;
    private Vector2 colCenter;
    private Vector2 colSize;


    private Collider2D nerbuzCol;
    private SpriteRenderer sprite;
    private Transform player;


    private Rigidbody2D rb;
    private Animator anim;
    [HideInInspector] public bool H2ChargingAnim;
    [HideInInspector] public bool H2attackAnim;
    public bool isCrazy = false;
    private bool ffCrazy = true;
    private void Awake()
    {
        generatorPos = new Vector2[nGenerators];    
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        nerbuzCol = GetComponent<Collider2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        actualState = State.Nothing;

        movDir = GetRandomDirection(0);
        cntTimeToSpawn = timeToSpawn;

        Transform colTrans = colH1Confiner.GetComponent<Transform>();
        colCenter = colTrans.position;

        colSize.x = colTrans.localScale.x * colH1Confiner.bounds.size.x;
        colSize.y = colTrans.localScale.y * colH1Confiner.bounds.size.y;
        spellH3Prefab.SetActive(false);

        finalPosH4 = new Vector3(colH1Confiner.bounds.min.x + positionH4, colH1Confiner.bounds.size.y/2, 0f);
    }

    private void Update()
    {
        switch (actualState)
        {
            case State.Nothing:
                break;
            case State.Enter:
                UpdateEnter();
                break;
            case State.H1:
                FlyH1();
                AttackH1();
                break;
            case State.H2:
                UpdateH2();
                break;
            case State.H3:
                UpdateH3();
                break;
            case State.H4:
                break;
            case State.Transition:
                UpdateTransitions();
                break;
            case State.Dead:
                print("DEAD");
                break;
        }
        Dead();
    }

    private void FixedUpdate()
    {
        switch (actualState)
        {
            case State.Enter:
                rb.velocity = Vector2.zero;
                break;
            case State.H1:
                rb.velocity = movDir * speedH1 * Time.fixedDeltaTime;
                break;
            case State.H2:
            rb.velocity = Vector2.zero;
                break;
            case State.H3:
                break;
            case State.H4:
                break;
            case State.Transition:
                break;
            case State.Dead:
                rb.velocity = Vector2.zero;
                break;
        }
    }

    void UpdateEnter()
    {
        bool ffEnter = false;
        if (!ffEnter)
        {
            StartCoroutine(IsOnEnterState());
            ffEnter = true;
        }
    }
    #region ENTER and TRANSITION
    IEnumerator IsOnEnterState()
    {
        //Reproducir animacion de algo
        yield return new WaitForSeconds(2);
        actualState = State.H1;
        
    }

    void UpdateTransitions()
    {
        switch(transitions)
        {
            case 1: //TRANSICION DE H1 A H2
                if(transform.position != centerPosition.position)
                {
                    centerReached = false;
                    transform.position = Vector2.MoveTowards(transform.position, centerPosition.position, 12 * Time.deltaTime);
                }
                else
                {
                    centerReached = true;
                    canBeDamaged = false;
                    if(!shieldActivated)
                    {
                        Instantiate(shieldPrefab, transform.position, Quaternion.identity);
                        shieldActivated = true;
                        actualState = State.H2;
                    }
                    else
                        return;
                }
                break;
            case 2:
                if (ffCrazy)
                {
                    isCrazy = true;
                    ffCrazy = false;
                }
                if (!isCrazy)
                {
                    //LE QUITAMOS EL CRAZY DESDE EL ANIMCONTROLLER
                    if(cntSeriesAttackH2 < seriesAttackH2)
                    {
                        if (transform.position != centerPosition.position) //Subimos hasta el centro para continuar atacando H2
                        {
                            transform.position = Vector2.MoveTowards(transform.position, centerPosition.position, 20 * Time.deltaTime);
                        }
                        else
                        {
                            canBeDamaged = false;
                            Instantiate(shieldPrefab, transform.position, Quaternion.identity);
                            //el isTired lo quita el animController
                            reachedCansadaPos = false;
                            cntTiredTime = tiredTime;
                            cntAttacksH2 = 0;
                            actualState = State.H2;
                        }
                    }
                    else // Si ya ha acabado las series de ataques
                    {
                        //el isTired lo quita el animController
                        reachedCansadaPos = false;
                        cntTiredTime = tiredTime;
                        cntAttacksH2 = 0;
                        cntSeriesAttackH2 = 0;
                        transitions = 3; //hemos acabado h2 y empezamos h3
                    }
                }
                break;
            case 3:
                if(transform.position != centerPosition.position)
                {
                    transform.position = Vector2.MoveTowards(transform.position, centerPosition.position, 15 * Time.deltaTime);
                    cntTimeToH3 = timeToH3;
                }
                else
                {
                    if(cntTimeToH3 > 0)
                    {
                        cntTimeToH3 -= Time.deltaTime;
                    }
                    else
                    {
                        actualState = State.H3;
                    }
                }
                break;
            case 4:
                if(transform.position != finalPosH4)
                {
                    transform.position = Vector2.MoveTowards(transform.position, finalPosH4, 15 * Time.deltaTime);
                }
                else
                {
                    
                }
                break;
        }
    }
    #endregion
    #region H1
    public void FlyH1()
    {
        if (transform.position.x <= colH1Confiner.bounds.min.x)
        {
            movDir = GetRandomDirection(1);
        }
        else if (transform.position.x >= colH1Confiner.bounds.max.x)
        {
            movDir = GetRandomDirection(2);
        }
        if (transform.position.y <= colH1Confiner.bounds.min.y)
        {
            movDir = GetRandomDirection(3);
        }
        else if (transform.position.y >= colH1Confiner.bounds.max.y)
        {
            movDir = GetRandomDirection(4);
        }
    }

    public void AttackH1()
    {
        if(cntParticlesSpawn < particlesToSpawn)
        {
            if(cntTimeToSpawn > 0)
            {
                cntTimeToSpawn -= Time.deltaTime;
            }
            else
            {
                Instantiate(particleH1Prefab, player.position, Quaternion.identity);
                cntTimeToSpawn = timeToSpawn;
                cntParticlesSpawn++;
            }

        }
        else
        {
            transitions = 1;
            cntParticlesSpawn = 0;
            actualState = State.Transition;
        }
    }
    private Vector2 GetRandomDirection(int number)
    {
        Vector2 movDir;
        movDir = new Vector2();
        switch (number)
        {
            case 0:
                movDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1, 1));
                break;
            case 1:
                movDir = new Vector2(Random.Range(0.1f, 1f), Random.Range(-1f, 1f));
                break;
            case 2:
                movDir = new Vector2(Random.Range(-1f, -0.1f), Random.Range(-1f, 1f));
                break;
            case 3:
                movDir = new Vector2(Random.Range(-1f, 1f), Random.Range(0.1f, 1f));
                break;
            case 4:
                movDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, -0.1f));
                break;
        }
        movDir.Normalize();
        return movDir;
    }
    #endregion

    #region H2
    void UpdateH2()
    {
        //AQUI ESTA CARGANDO EL ATAQUE
        if(cntSeriesAttackH2 < seriesAttackH2)
        {
            if(cntAttacksH2 < numberOfAttacksH2)
            {
                isTired = false;
                if(!generatorInPlace)
                {
                    H2ChargingAnim = true;
                    for(int i = 0; i < generatorPos.Length; i++)
                    {
                        if(i == 0)
                        {
                            generatorPos[i] = new Vector2(player.position.x + Random.Range(-0.3f, 0.3f), colCenter.y - colSize.y/2);
                        }
                        else
                        {
                            generatorPos[i] = H2GeneratorRandomPos();
                            float distancia = Vector2.Distance(generatorPos[i], generatorPos[i - 1]);
                            if(distancia < 3f)
                            {
                                generatorPos[i] = H2GeneratorRandomPos();
                            }
                        }
                        GameObject go = Instantiate(groundParticle, generatorPos[i], Quaternion.identity);
                        go.GetComponent<ParticleDestroy>().timeToDestroy = timeToAttackH2;
                    }
                    cntAttacksH2++;
                    cntTimeToAttackH2 = timeToAttackH2;
                    canAttackH2 = true;
                    generatorInPlace = true;
                }
            }
        }
        if(canAttackH2)
        {
            if(cntTimeToAttackH2 > 0)
            {
                cntTimeToAttackH2 -= Time.deltaTime;
                if (!earthquakeSound.isPlaying)
                {
                    earthquakeSound.Play();
                }
            }
            else
            {
                earthquakeSound.Stop();
                for(int i = 0; i < generatorPos.Length; i++)
                {
                    Instantiate(pinchosPrefab, generatorPos[i], Quaternion.identity);
                }
                canAttackH2 = false;
            }
        }
        if(isTired)
        {
            canBeDamaged = true;
            if(!reachedCansadaPos)
            {
                if(transform.position != cansadaPos.position)
                {
                    transform.position = Vector2.MoveTowards(transform.position, cansadaPos.position, 5 * Time.deltaTime);
                    cntTiredTime = tiredTime;
                }
                else
                {
                    cntSeriesAttackH2++;
                    reachedCansadaPos = true;
                }
            }
            else
            {
                if(cntTiredTime > 0)
                {
                    cntTiredTime -= Time.deltaTime;
                }
                else
                {
                    ffCrazy = true;
                    print("GO CRAZY GIRL");
                    transitions = 2;
                    actualState = State.Transition;
                }
            }
        }
    }

    Vector2 H2GeneratorRandomPos()
    {
        Vector2 posicion;
        posicion = colCenter + new Vector2(Random.Range(-colSize.x/2, colSize.x/2), -colSize.y/2);
        return posicion;
    }
    #endregion

    #region H3
    void UpdateH3()
    {
        if (!alreadyActivatedH3)
        {
            spellH3Prefab.SetActive(true);
            alreadyActivatedH3 = true;
            cntTimeDoingH3 = timeDoingH3;
        }
        else
        {
            if(cntTimeDoingH3 > 0)
            {
                cntTimeDoingH3 -= Time.deltaTime;
            }
            else
            {
                if (!finishSpawnH3)
                {
                    spellH3Prefab.SetActive(false);
                    cntTimeTiredForH3 = timeTiredForH3;
                    finishSpawnH3 = true;
                }
                else
                {
                    if (cntTimeTiredForH3 > 0)
                    {
                        cntTimeTiredForH3 -= Time.deltaTime;
                    }
                    else
                    {
                        alreadyActivatedH3 = false;
                    }
                }
            }
        }
    }
    #endregion

    #region DAMAGED
    public void TakeDamge(int damage)
    {
        if (life > 0)
        {
            life -= damage;
            Debug.Log(life);
            StartCoroutine(Blinking());
        }
        else
            return;
    }

    IEnumerator Blinking()
    {
        for (int i = 0; i < 3; i++)
        {
            sprite.color = Color.gray;
            yield return new WaitForSeconds(0.15f);
            sprite.color = Color.white;
            yield return new WaitForSeconds(0.15f);
        }
    }
    #endregion

    void Dead()
    {
        if(life <= 0)
        {
            actualState = State.Dead;
        }
    }
}
