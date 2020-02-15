using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NerbuzBoss : MonoBehaviour
{
    public enum State
    {
        Enter, H1,H2,H3,H4,Transition
    }

    public State actualState;
    public int life = 200;
    [Header("Hechizo1")]
    public float speedH1;
    public int particlesToSpawn;
    private int cntParticlesSpawn = 0;
    public float timeToSpawn;
    private float cntTimeToSpawn;
    public GameObject particleH1Prefab;
    private Vector2 movDir;

    public Collider2D colH1Confiner;
    private Vector2 colCenter;
    private Vector2 colSize;


    private Collider2D nerbuzCol;
    private SpriteRenderer sprite;



    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        nerbuzCol = GetComponent<Collider2D>();

        actualState = State.Enter;

        movDir = GetRandomDirection(0);
        cntTimeToSpawn = timeToSpawn;

        Transform colTrans = colH1Confiner.GetComponent<Transform>();
        colCenter = colTrans.position;

        colSize.x = colTrans.localScale.x * colH1Confiner.bounds.size.x;
        colSize.y = colTrans.localScale.y * colH1Confiner.bounds.size.y;

        Debug.Log(colCenter);
    }

    private void Update()
    {
        switch (actualState)
        {
            case State.Enter:
                UpdateEnter();
                break;
            case State.H1:
                FlyH1();
                AttackH1();
                break;
            case State.H2:
                break;
            case State.H3:
                break;
            case State.H4:
                break;
            case State.Transition:
                break;
        }
    }

    private void FixedUpdate()
    {
        if(actualState == State.H1)
        {
            rb.velocity = movDir * speedH1 * Time.fixedDeltaTime;
        }
    }

    void UpdateEnter()
    {
        StartCoroutine(IsOnEnterState());
    }
    IEnumerator IsOnEnterState()
    {
        yield return new WaitForSeconds(2);
        actualState = State.H1;
    }
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
                Instantiate(particleH1Prefab, GetRandomPosition(), Quaternion.identity);
                cntTimeToSpawn = timeToSpawn;
                cntTimeToSpawn++;
            }

        }
    }

    public Vector2 GetRandomPosition()
    {
        Vector2 randomPosition = colCenter + new Vector2(Random.Range(-colSize.x/2, colSize.x/2), Random.Range(-colSize.y/2, colSize.y/2));
        return randomPosition;
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
