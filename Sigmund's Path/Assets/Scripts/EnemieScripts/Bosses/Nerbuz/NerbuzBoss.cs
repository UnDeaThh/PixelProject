using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public enum State
    {
        Enter, H1,H2,H3,H4,Transition,Nothing
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
    public int nGenerators;
    public int nAttackH2 = 3;
    private int cntAttacksH2 = 0;
    private Vector2[] generatorPos;
    public bool generatorInPlace = false;
    public bool canAttackH2;
    public float timeToAttackH2;
    private float cntTimeToAttackH2;

    [Header("Transitions")]
    public int transitions = 1;
    [Header("Transition 1")]
    public GameObject shieldPrefab;
    public bool centerReached;
    private bool shieldActivated;
    public Transform centerPosition;

    public Collider2D colH1Confiner;
    private Vector2 colCenter;
    private Vector2 colSize;


    private Collider2D nerbuzCol;
    private SpriteRenderer sprite;
    private Transform player;



    private Rigidbody2D rb;
    private void Awake()
    {
        generatorPos = new Vector2[nGenerators];
        rb = GetComponent<Rigidbody2D>();
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
                break;
            case State.H4:
                break;
            case State.Transition:
                UpdateTransitions();
                break;
        }
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
        }
    }

    void UpdateEnter()
    {
        StartCoroutine(IsOnEnterState());
    }
    #region ENTER and TRANSITION
    IEnumerator IsOnEnterState()
    {
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
                    }
                    else
                        return;
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
        if(cntAttacksH2 <= nAttackH2){
            if(!generatorInPlace)
            {
                for(int i = 0; i < generatorPos.Length; i++)
                {
                    if(i == 0)
                    {
                        generatorPos[i] = new Vector2(player.position.x + Random.Range(-0.3f, 0.3f), colCenter.y - colSize.y/2);
                    }
                    else
                    {
                        generatorPos[i] = H2GeneratorRandomPos();
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

        if(canAttackH2)
        {
            if(cntTimeToAttackH2 > 0)
            {
                cntTimeToAttackH2 -= Time.deltaTime;
            }
            else
            {
                for(int i = 0; i < generatorPos.Length; i++)
                {
                    Instantiate(pinchosPrefab, generatorPos[i], Quaternion.identity);
                }
                canAttackH2 = false;
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

    #region DAMAGED
    public void TakeDamge(int damage)
    {
        life -= damage;
        Debug.Log(life);
        StartCoroutine(Blinking());
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
}
