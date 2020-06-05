using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class ChangelingAI : BaseEnemy
{
    #region FollowPath
    private Rigidbody2D rb;
    public float maxSpeed = 7;
    private Transform target;
    private Vector2 force;
    public bool isChasing = false;

    private float nextWayPointDistance = 3f;

    private Path path;
    private int currentWayPoint = 0;
    private bool reachedEndOfPath = false;

    private Seeker seeker;
    #endregion
    public float damagePushForce = 100;
    public Transform spriteChangeling;
    private SpriteRenderer graphic;
    

    public Color damagedColor;
    private Color normalColor;

    private bool ffStuned;
    [SerializeField] float stunedForce;
    [SerializeField] AudioSource aleteoSource;
    [SerializeField] AudioSource screamSource;
    private bool oneScream = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        graphic = GetComponentInChildren<SpriteRenderer>();
        mat = graphic.material;
        anim = GetComponentInChildren<Animator>();
        anim.SetFloat("randomStart", Random.Range(0.00f, 1.00f));
        normalColor = graphic.color;
    }
    private void Update()
    {
        CheckMaxSpeed();
        if(nLifes > 0)
        {
            if (!isStuned)
            {
                Flip();
            }
        }
        Stuned();
        Dead();
    }

    void CheckMaxSpeed()
    {
        if(rb.velocity.x >= maxSpeed)
        {
            rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
        }
        if(rb.velocity.x <= -maxSpeed)
        {
            rb.velocity = new Vector2(-maxSpeed, rb.velocity.y);
        }
    }
    private void FixedUpdate()
    {
        if (path == null)
            return;
        if(currentWayPoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }
        if (nLifes > 0)
        {
            ApplyMovement();
        }

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);

        if(distance < nextWayPointDistance)
        {
            currentWayPoint++;
        }
    }

    void ApplyMovement()
    {
        if (!isStuned)
        {
            Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
            force = direction * movSpeed * Time.fixedDeltaTime;
            rb.AddForce(force);
        }
        else
        {
            if (!ffStuned)
            {
                rb.velocity = Vector2.zero;
                if (transform.position.x >= target.position.x)
                {
                    rb.AddForce(Vector2.right * stunedForce );
                }
                else
                {
                    rb.AddForce(Vector2.left * stunedForce);
                }
                ffStuned = true;
            }
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }


    void Flip()
    {
        if(rb.velocity.x >= 0.01f)
        {
            spriteChangeling.localScale = new Vector3(-1f, 1f, 1f);
        }

        else if(rb.velocity.x <= -0.01f)
        {
            spriteChangeling.localScale = new Vector3(1f, 1f, 1f);
        }
    }
    public override void TakeDamage(int damage, Vector2 playerPos)
    {
        base.TakeDamage(damage, playerPos);
        rb.velocity = Vector2.zero;
        if (playerPos.x <= transform.position.x)
        {
            Vector2 hitDir = new Vector2(1f, 0f);
            rb.AddForce(hitDir * damagePushForce);
        }
        else
        {
            Vector2 hitForce = new Vector2(-1f, 0f);
            rb.AddForce(hitForce * damagePushForce);
        }
        Debug.Log("Changeling");
    }    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isChasing = true;
            if (!oneScream)
            {
                screamSource.Play();
                oneScream = true;
            }
            InvokeRepeating("UpdatePath", 0f, .5f);
        }
    }     

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController2>().PlayerDamaged(damage, transform.position);
        }
    }

    public override void StartStun()
    {
        isStuned = true;
        cntTimeStuned = timeStuned;
        ffStuned = false;
        Debug.Log("Parry to Change");
    }

    public override void Dead()
    {
        base.Dead();
    }

    public void MakeAleteoSound()
    {
        aleteoSource.pitch = Random.Range(0.8f, 1.2f);
        aleteoSource.Play();
    }
}
