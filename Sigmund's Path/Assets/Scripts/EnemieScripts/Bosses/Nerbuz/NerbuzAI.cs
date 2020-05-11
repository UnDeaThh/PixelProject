using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NerbuzAI : BossBase
{

    private State actualState;

    public State ActualState { get => actualState; set => actualState = value; }
    public bool IsTired { get => isTired; set => isTired = value; }
    public bool H2ChargingAnim1 { get => H2ChargingAnim; set => H2ChargingAnim = value; }
    public bool GeneratorInPlace { get => generatorInPlace; set => generatorInPlace = value; }
    public int CntAttacksH2 { get => cntAttacksH2; set => cntAttacksH2 = value; }
    public int AttacksH2 { get => attacksH2; set => attacksH2 = value; }
    public bool H2AttackAnim1 { get => H2AttackAnim; set => H2AttackAnim = value; }
    public bool IsCrazy { get => isCrazy; set => isCrazy = value; }
    public bool MakingH3 { get => makingH3; set => makingH3 = value; }
    public bool MakingH4 { get => makingH4; set => makingH4 = value; }
    public GameObject LaserH4 { get => laserH4; set => laserH4 = value; }
    public Transform PlayerPos { get => playerPos; set => playerPos = value; }

    private Vector2 movDir;
    private Transform playerPos;

    [Header("Enter")]
    [SerializeField] float timeToH1;
    private float cntTimeToH1;
    [SerializeField] GameObject cameraFight;

    [Header("H1")]
    [SerializeField] float speedH1;
    [SerializeField] int maxParticlesH1;
    private float cntParticlesH1;
    [SerializeField] float timeToSpawnH1;
    private float cntTimeToSpawnH1;
    [SerializeField] GameObject particleH1;

    [Header("Transition")]
    [SerializeField] Transform centerPos;
    [SerializeField] float transitionSpeed;
    [SerializeField] GameObject shield;
    private bool shieldActivated;
    private int transition;
    private int transitionState;
    private bool isTired;
    private Vector2 tiredPos;

    [Header("H2")]
    [SerializeField] int seriesH2;
 
    private bool H2ChargingAnim;
    private bool H2AttackAnim;
    private int cntSeriesH2;
    [SerializeField] int attacksH2;
    private int cntAttacksH2;
    private bool generatorInPlace;
    private bool canAttackH2;
    [SerializeField] int nGenerators;
    private Vector2[] generatorPos;
    [SerializeField] Collider2D roomSpace;
    private Vector2 colCenter;
    private Vector2 colSize;
    [SerializeField] GameObject groundParticle;
    [SerializeField] float timeToAttackH2;
    private float cntTimeToAttackH2;
    [SerializeField] AudioSource source;
    [SerializeField] GameObject pinchosH2;
    [Header("Cansada")]
    [SerializeField] float cansadaAltitude;
    [SerializeField] float tiredSpeed;
    [SerializeField] float tiredTime;
    private float cntTiredTime;
    private bool reachedTiredPos;
    private bool deactivateShield;
    private bool isCrazy;

    [Header("H3")]
    [SerializeField] ParticleSystem[] particlesH3;
    private bool makingH3;
    private bool h3Activated;
    [SerializeField] float timeH3;
    private float cntTimeH3;
    [SerializeField] int seriesH3;
    private int cntSeriesH3;

    [Header("H4")]
    [SerializeField] Transform h4Pos;
    private bool rechedH4Pos;
    [SerializeField] float timeStillH4;
    private float cntTimeStillH4;
    [SerializeField] float timeToH4;
    private float cntTimeToH4;
    private bool makingH4;
    [SerializeField] GameObject laserH4;
    [SerializeField] int attacksH4;
    private int cntAttacksH4;

    [Header("CAMERA SHAKE")]
    [SerializeField] float shakeAmplitude;
    [SerializeField] float shakeFrequency;
    [SerializeField] float shakeTime;

    [Header("DEAD")]
    [SerializeField] float timeFreeze;
    private float cntTimeFreeze;
    [SerializeField] float deadAmplitudeShaking;
    [SerializeField] float deadFrequencyShaking;
    [SerializeField] float deadTimeShaking;
    float cntTimeShaking;
    bool oneShake = false;
    [SerializeField] AudioSource earthquakeSound;
    private Animator anim;
    void Start()
    {
        actualState = State.Nothing;
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        mat = sprite.material;
        col = GetComponent<Collider2D>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;

        cntTimeToH1 = timeToH1;
        generatorPos = new Vector2[nGenerators];

        Transform colTrans = roomSpace.gameObject.transform;
        colCenter = colTrans.position;

        colSize.x = colTrans.localScale.x * roomSpace.bounds.size.x;
        colSize.y = colTrans.localScale.y * roomSpace.bounds.size.y;

        tiredPos = new Vector2(centerPos.position.x, centerPos.position.y - cansadaAltitude);
        cameraFight.SetActive(false);
        facingDir = 1;

        for (int i = 0; i < particlesH3.Length; i++)
        {
            ParticleSystem.EmissionModule emidMod = particlesH3[i].emission;
            emidMod.enabled = false;
        }
        anim = GetComponentInChildren<Animator>();
        cntTimeFreeze = timeFreeze;
        cntTimeShaking = deadTimeShaking;
    }
    void Update()
    {
        if(nLifes > 0)
        {
            switch (actualState)
            {
                case State.Enter:
                    if(cntTimeToH1 > 0)
                    {
                        cntTimeToH1 -= Time.deltaTime;
                        cameraFight.SetActive(true);
                    }
                    else
                    {
                        actualState = State.H1;
                        cntParticlesH1 = 0;
                        movDir = GetRandomDirection(0);
                        cntTimeToSpawnH1 = timeToSpawnH1;
                    }
                    break;
                case State.H1:
                    //LA DIRECCION DEL VUELO SE RECALCULA CON LOS COLLIDERS TRIGGERS
                    if(cntParticlesH1 < maxParticlesH1)
                    {
                        if(cntTimeToSpawnH1 > 0)
                        {
                            cntTimeToSpawnH1 -= Time.deltaTime;
                        }
                        else
                        {
                            Instantiate(particleH1, playerPos.position, Quaternion.identity);
                            cntParticlesH1++;
                            cntTimeToSpawnH1 = timeToSpawnH1;
                        }
                    }
                    else
                    {
                        transition = 1;
                        transitionState = 1;
                        actualState = State.Transition;
                    }
                    break;
                case State.H2:
                    if(cntSeriesH2 < seriesH2)
                    {
                        if(cntAttacksH2 < attacksH2)
                        {
                            if (!generatorInPlace)
                            {
                                H2ChargingAnim = true; // Ponemos la animacion a cargar
                                Debug.Log("charging anim true");
                                for (int i = 0; i < generatorPos.Length; i++)
                                {
                                    if(i == 0)
                                    {
                                        generatorPos[i] = new Vector2(playerPos.position.x + Random.Range(-0.3f, 0.3f), roomSpace.bounds.min.y);
                                    }
                                    else
                                    {
                                        generatorPos[i] = H2GeneratorRandomPos();
                                        float distancia = Vector2.Distance(generatorPos[i], generatorPos[i - 1]);
                                        while(distancia < 3)
                                        {
                                            generatorPos[i] = H2GeneratorRandomPos();
                                            distancia = Vector2.Distance(generatorPos[i], generatorPos[i - 1]);
                                        }
                                    }
                                    GameObject go = Instantiate(groundParticle, generatorPos[i], Quaternion.identity);
                                    go.GetComponent<ParticleDestroy>().timeToDestroy = timeToAttackH2;
                                }
                            
                                cntTimeToAttackH2 = timeToAttackH2;
                                canAttackH2 = true;
                                generatorInPlace = true; //EL GENERATOR IN PLACE LO QUITA EL SCRIPT DE LOS PINCHOS
                            }
                            if (canAttackH2)
                            {
                                if(cntTimeToAttackH2 > 0)
                                {
                                    cntTimeToAttackH2 -= Time.deltaTime;
                                    if (!source.isPlaying)
                                    {
                                        source.Play(); // Suena terremoto
                                    }
                                }
                                else
                                {
                                    source.Stop();
                                    for (int i = 0; i < generatorPos.Length; i++)
                                    {
                                        Instantiate(pinchosH2, generatorPos[i], Quaternion.identity);
                                    }
                                    H2AttackAnim = true;
                                    H2ChargingAnim = false;
                                    cntAttacksH2++;
                                    canAttackH2 = false;
                                    reachedTiredPos = false;
                                    deactivateShield = false;
                                }
                            }
                        }
                        else
                        {
                            if (isTired)
                            {
                                if (!deactivateShield)
                                {
                                    Shield shield = FindObjectOfType<Shield>();
                                    shield.IsDisolve = true;
                                    deactivateShield = true;
                                }
                                if (!reachedTiredPos)
                                {
                                    if(transform.position.y > tiredPos.y)
                                    {
                                        Vector2 dir;
                                        dir = new Vector2(tiredPos.x - transform.position.x, tiredPos.y - transform.position.y).normalized;
                                        movDir = dir;
                                    }
                                    else
                                    {
                                        cntSeriesH2++;
                                        cntTiredTime = tiredTime;
                                        reachedTiredPos = true;
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
                                        isCrazy = true;
                                        transition = 1;
                                        transitionState = 1;
                                        actualState = State.Transition;
                                    }
                                }
                            }
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
                            transition = 2;
                            transitionState = 1;
                            actualState = State.Transition;
                        }
                    }
                    break;
                case State.H3:
                    if(cntSeriesH3 < seriesH3)
                    {
                        if (!isTired)
                        {
                            makingH3 = true;
                            if (!h3Activated)
                            {
                                for (int i = 0; i < particlesH3.Length; i++)
                                {
                                    ParticleSystem.EmissionModule emidMod = particlesH3[i].emission;
                                    emidMod.enabled = true;
                                }
                                h3Activated = true;
                            }
                            if(cntTimeH3 > 0)
                            {
                                cntTimeH3 -= Time.deltaTime;
                            }
                            else
                            {
                                for (int i = 0; i < particlesH3.Length; i++)
                                {
                                    ParticleSystem.EmissionModule emidMod = particlesH3[i].emission;
                                    emidMod.enabled = false;
                                }
                                cntSeriesH3++;
                                makingH3 = false;
                                isTired = true;
                            }
                        }
                        else
                        {
                            if (!reachedTiredPos)
                            {
                                if(transform.position.y > tiredPos.y)
                                {
                                    Vector2 dir;
                                    dir = new Vector2(tiredPos.x - transform.position.x, tiredPos.y - transform.position.y).normalized;
                                    movDir = dir;
                                    Debug.Log("incoming");
                                }
                                else
                                {
                                    reachedTiredPos = true;
                                }
                            }
                            else
                            {
                                transition = 2;
                                transitionState = 1;
                                actualState = State.Transition;
                            }
                        }
                    }
                    else
                    {
                        if (isTired)
                        {
                            if (!reachedTiredPos)
                            {
                                if (transform.position.y > tiredPos.y)
                                {
                                    Vector2 dir;
                                    dir = new Vector2(tiredPos.x - transform.position.x, tiredPos.y - transform.position.y).normalized;
                                    movDir = dir;
                                }
                                else
                                {
                                    reachedTiredPos = true;
                                }
                            }
                            else
                            {
                                transition = 3;
                                transitionState = 1;
                                cntTiredTime = tiredTime;
                                actualState = State.Transition;
                            }
                        }
                    }
                    break;
                case State.H4:
                    if(cntAttacksH4 < attacksH4)
                    {
                        if (!makingH4)
                        {
                            if(cntTimeToH4 > 0)
                            {
                                cntTimeToH4 -= Time.deltaTime;
                            }
                            else
                            {
                                makingH4 = true;
                                cntAttacksH4++;
                                cntTimeToH4 = timeToH4;
                            }
                        }
                    }
                    else
                    {
                        cntParticlesH1 = 0;
                        movDir = GetRandomDirection(0);
                        cntTimeToSpawnH1 = timeToSpawnH1;
                        actualState = State.H1;
                    }
                    break;
                case State.Transition:
                    if(transition == 1)
                    {
                        if(transitionState == 1) // Posicionandose en el centro de la pantalla
                        {
                            if(transform.position.y > centerPos.position.y - 0.1f && transform.position.y < centerPos.position.y + 0.1f && transform.position.x > centerPos.position.x - 0.1f 
                                && transform.position.x < centerPos.position.x + 0.1f)
                            {
                                shieldActivated = false;
                                transitionState = 2;
                            }
                            else
                            {
                                Vector2 dir;
                                dir = new Vector2(centerPos.position.x - transform.position.x, centerPos.position.y - transform.position.y).normalized;
                                movDir = dir;
                            }
                        }
                        else if(transitionState == 2)
                        {
                            if (!shieldActivated)
                            {
                                Instantiate(shield, transform.position, Quaternion.identity);
                                shieldActivated = true;
                                if(cntSeriesH2 == seriesH2)
                                {
                                    cntSeriesH2 = 0;
                                }
                                cntAttacksH2 = 0;
                                generatorInPlace = false;
                                actualState = State.H2;
                            }
                        }
                    }
                    else if(transition == 2)
                    {
                        if(transitionState == 1)
                        {
                            if(cntTiredTime > 0)
                            {
                                cntTiredTime -= Time.deltaTime;
                            }
                            else
                            {
                                isCrazy = true;
                                transitionState = 2;
                            }
                        }
                        else if(transitionState == 2)
                        {
                            if (transform.position.y > centerPos.position.y - 0.1f && transform.position.y < centerPos.position.y + 0.1f && transform.position.x > centerPos.position.x - 0.1f
                                && transform.position.x < centerPos.position.x + 0.1f)
                            {
                                shieldActivated = false;
                                h3Activated = false;
                                isTired = false;
                                cntTiredTime = tiredTime;
                                cntTimeH3 = timeH3;
                                reachedTiredPos = false;
                                if(cntSeriesH3 != seriesH3)
                                {
                                    Debug.Log("cargarH3");
                                    actualState = State.H3;
                                }
                            }
                            else
                            {
                                Vector2 dir;
                                dir = new Vector2(centerPos.position.x - transform.position.x, centerPos.position.y - transform.position.y).normalized;
                                movDir = dir;
                            }
                        }
                    }
                    else if(transition == 3)
                    {
                        if(transitionState == 1)
                        {
                            if (isTired)
                            {
                                if(cntTiredTime > 0)
                                {
                                    cntTiredTime -= Time.deltaTime;
                                }
                                else
                                {
                                    IsCrazy = true;
                                    rechedH4Pos = false;
                                    Debug.Log("gotrans2");
                                    transitionState = 2;
                                }
                            }
                        }
                        else if(transitionState == 2)
                        {
                            if (!isCrazy)
                            {
                                if (!rechedH4Pos)
                                {
                                    if(transform.position.y > h4Pos.position.y - 0.05f && transform.position.y < h4Pos.position.y + 0.1f && transform.position.x > h4Pos.position.x - 0.05f
                                    && transform.position.x < h4Pos.position.x + 0.1f)
                                    {
                                        rechedH4Pos = true;
                                        Flip();
                                        cntTimeStillH4 = timeStillH4;
                                        transitionState = 3;
                                    }
                                    else
                                    {
                                        Debug.Log("Moving");
                                        Vector2 dir;
                                        dir = new Vector2(h4Pos.position.x - transform.position.x, h4Pos.position.y - transform.position.y).normalized;
                                        movDir = dir;
                                    }
                                }
                            }
                        }
                        else if (transitionState == 3)
                        {
                            if (cntTimeStillH4 > 0)
                            {
                                cntTimeStillH4 -= Time.deltaTime;
                            }
                            else
                            {
                                cntTimeToH4 = 0.1f;
                                cntAttacksH4 = 0;
                                actualState = State.H4;
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
            if (cntTimeFreeze > 0)
            {
                AudioManager.instanceAudio.EndBossSong = true;
                cntTimeFreeze -= Time.deltaTime;
                col.enabled = false;
                anim.speed = 0;
                if (cntTimeFreeze < 0.15f)
                {
                    cameraFight.SetActive(false);
                }
            }
            else
            {
                if (!oneShake)
                {

                    earthquakeSound.Play();
                    CameraController.cameraController.GenerateCamerashake(deadAmplitudeShaking, deadFrequencyShaking, deadTimeShaking);
                    oneShake = true;
                }
                if(cntTimeShaking > 0)
                {
                    cntTimeShaking -= Time.deltaTime;
                    if (cntTimeShaking < deadTimeShaking / 3)
                    {
                        if (!oneCallDead)
                        {
                            anim.speed = 1;
                            anim.SetBool("isDead", true);
                            oneCallDead = true;
                        }
                        if (earthquakeSound.volume > 0)
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
                    rb.velocity = movDir * speedH1 * Time.deltaTime;
                    break;
                case State.H2:
                    if (!isTired)
                    {
                        rb.velocity = Vector2.zero;
                    }
                    else
                    {
                        if (!reachedTiredPos)
                        {
                            rb.velocity = movDir * tiredSpeed * Time.deltaTime;
                        }
                        else
                        {
                            rb.velocity = Vector2.zero;
                        }
                    }
                    break;
                case State.H3:
                    if (!isTired)
                    {
                        rb.velocity = Vector2.zero;
                    }
                    else
                    {
                        if (!reachedTiredPos)
                        {
                            rb.velocity = movDir * tiredSpeed * Time.deltaTime;
                        }
                        else
                        {
                            rb.velocity = Vector2.zero;
                        }
                    }
                    break;
                case State.H4:
                    rb.velocity = Vector2.zero;
                    break;
                case State.Transition:
                    if(transition == 1)
                    {
                        if(transitionState == 1)
                        {
                            rb.velocity = movDir * transitionSpeed * Time.deltaTime;
                        }
                        else if(transitionState == 2)
                        {
                            rb.velocity = Vector2.zero;
                        }
                    }
                    else if(transition == 2)
                    {
                        if(transitionState == 1)
                        {
                            rb.velocity = Vector2.zero;
                        }
                        else if(transitionState == 2)
                        {
                            rb.velocity = movDir * transitionSpeed * Time.deltaTime;
                        }
                    }
                    else if(transition == 3)
                    {
                        if(transitionState == 1)
                        {

                            rb.velocity = Vector2.zero;
                        
                        }
                        else if(transitionState == 2)
                        {
                            if (!isCrazy)
                            {
                                if (rechedH4Pos)
                                {
                                    rb.velocity = Vector2.zero;
                                }
                                else
                                {
                                    rb.velocity = movDir * transitionSpeed * Time.deltaTime;
                                }
                            }
                            else
                            {
                                rb.velocity = Vector2.zero;
                            }
                        }
                        else if(transitionState == 3)
                        {
                            rb.velocity = Vector2.zero;
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
    private Vector2 GetRandomDirection(int number)
    {
        Vector2 movDir;
        movDir = new Vector2();
        switch (number)
        {
            case 0: //FULL RANDOM
                if(facingDir < 0)
                {
                    movDir = new Vector2(Random.Range(-1f, -0.2f), Random.Range(-1, 1));
                }
                else
                {
                    movDir = new Vector2(Random.Range(0.2f, 1f), Random.Range(-1, 1));
                }
                break;
            case 1: //LIMIT LEFT
                movDir = new Vector2(Random.Range(0.1f, 1f), Random.Range(-1f, 1f));
                break;
            case 2: // LIMIT RIGHT
                movDir = new Vector2(Random.Range(-1f, -0.1f), Random.Range(-1f, 1f));
                break;
            case 3: //LIMIT DOWN
                movDir = new Vector2(Random.Range(-1f, 1f), Random.Range(0.1f, 1f));
                break;
            case 4: // LIMIT UP
                movDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, -0.1f));
                break;
        }
        movDir.Normalize();
        return movDir;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Floor"))
        {
            if(actualState == State.H1)
            {
                if(other.transform.name == "LimitDown")
                {
                    movDir = GetRandomDirection(3);
                    if(movDir.x > 0 && facingDir < 0 || movDir.x < 0 && facingDir > 0)
                    {
                        Flip();
                    }
                }
                else if(other.transform.name == "LimitUp")
                {
                    movDir = GetRandomDirection(4);
                    if (movDir.x > 0 && facingDir < 0 || movDir.x < 0 && facingDir > 0)
                    {
                        Flip();
                    }
                }
                else if (other.transform.name == "LimitRight")
                {
                    movDir = GetRandomDirection(2);
                    Flip();
                }
                else if (other.transform.name == "LimitLeft")
                {
                    movDir = GetRandomDirection(1);
                    Flip();
                }
            }
        }
    }

    Vector2 H2GeneratorRandomPos()
    {
        Vector2 posicion;
        posicion = colCenter + new Vector2(Random.Range(-colSize.x / 2, colSize.x / 2), -colSize.y / 2);
        return posicion;
    }
}
