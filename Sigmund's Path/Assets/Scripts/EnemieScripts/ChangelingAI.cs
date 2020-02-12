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

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        soul.enabled = false;
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        graphic = GetComponentInChildren<SpriteRenderer>();
        normalColor = graphic.color;
    }
    private void Update()
    {
        CheckMaxSpeed();
        Flip();
        base.Stuned();
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

        ApplyMovement();

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
            rb.velocity = Vector2.zero;
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

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if(rb.velocity.x <= -0.01)
        {
            Vector2 hitDir = new Vector2(1f, 0f);
            rb.AddForce(hitDir * damagePushForce);
            StartCoroutine(Blinking());
        }
        else if(rb.velocity.x >= 0.01f)
        {
            Vector2 hitForce = new Vector2(-1f, 0f);
            rb.AddForce(hitForce * damagePushForce);
            StartCoroutine(Blinking());
        }
        Debug.Log("Changeling");

    }

    IEnumerator Blinking()
    {
        for (int i = 0; i < 2; i++)
        {
            graphic.color = damagedColor;
            yield return new WaitForSeconds(0.1f);
            graphic.color = normalColor;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
             isChasing = true;
             InvokeRepeating("UpdatePath", 0f, .5f);
        }
    }     

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            //Vector2 normal = collision.contacts[0].normal;
            collision.gameObject.GetComponent<PlayerController2>().PlayerDamaged(damage, transform.position);
        }
    }

}
