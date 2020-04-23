using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NachAI : BaseEnemy
{
    private int facingDir = 1;

    public float timeToJump;
    private float cntTimeToJump;
    public float edgeDistance;
    public float wallDistance;
    public float groundDistance;
    [SerializeField] float stunedForce;

    public bool groundFound;
    private bool isGrounded;
    public bool wallFound;
    private bool makeJump;
    private bool ffStuned;

    public Vector2 jumpDirection;

    private Rigidbody2D rb;
    public Transform edgeLocatorPos;
    public Transform wallLocatorPos;
    public Transform groundCheckerPos;
    private Transform player;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreLayerCollision(9, 9);
        cntTimeToJump = timeToJump + Random.Range(0f, 1f);
        jumpDirection.Normalize();
        anim = GetComponentInChildren<Animator>();
        mat = GetComponentInChildren<SpriteRenderer>().material;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void Update()
    {
        CheckEnvironment();
        MovementLogic();

        Stuned();
        Dead();
    }


    private void FixedUpdate()
    {
        ApplyMovement();
    }
    void CheckEnvironment()
    {
        groundFound = Physics2D.Raycast(edgeLocatorPos.position, Vector2.down, edgeDistance, whatIsDetected);
        wallFound = Physics2D.Raycast(wallLocatorPos.position, transform.right, wallDistance, whatIsDetected);
        isGrounded = Physics2D.Raycast(groundCheckerPos.position, Vector2.down, groundDistance, whatIsDetected);
    }

    void MovementLogic()
    {
        if (groundFound && !wallFound)
        {
            if(cntTimeToJump > 0)
            {
                cntTimeToJump -= Time.deltaTime;
                makeJump = false;
            }
            else
            {
                makeJump = true;
                cntTimeToJump = timeToJump;
            }
        }
        else
        {
            if (isGrounded)
            {
                Flip();
            }
        }
    }
    void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
        facingDir *= -1;
    }

    void ApplyMovement()
    {
        if (!isStuned)
        {
            if (makeJump)
            {
                anim.SetTrigger("isJump");
                if(facingDir == 1)
                {
                    rb.AddForce(jumpDirection * movSpeed, ForceMode2D.Impulse);
                }
                else if(facingDir == -1)
                {
                    rb.AddForce(new Vector2(-jumpDirection.x, jumpDirection.y) * movSpeed, ForceMode2D.Impulse);
                }
            }
        }
        else
        {
            if (!ffStuned)
            {
                rb.velocity = Vector2.zero;
                if(transform.position.x >= player.position.x)
                {
                    rb.AddForce(Vector2.right * stunedForce);
                }
                else
                {
                    rb.AddForce(Vector2.left * stunedForce);
                }
                ffStuned = true;
            }
        }
    }
    public override void TakeDamage(int damage, Vector2 playerPos)
    {
        base.TakeDamage(damage, playerPos);
    }



    public override void Dead()
    {
        if(nLifes <= 0 && !oneCallDead)
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;    
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
        base.Dead();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            if (!collision.gameObject.GetComponent<PlayerParry>().IsParry)
            {
                collision.gameObject.GetComponent<PlayerController2>().PlayerDamaged(damage, transform.position);
            }
            else
            {
                ffStuned = false;
                cntTimeStuned = timeStuned;
            }
        }
    }
    public override void StartStun()
    {
        isStuned = true;
        cntTimeStuned = timeStuned;
        ffStuned = false;
        Debug.Log("Parry to Nach");
    }

    public override void Stuned()
    {
        if (isStuned)
        {
            if (cntTimeStuned > 0)
            {
                cntTimeStuned -= Time.deltaTime;
            }
            else
            {
                isStuned = false;
            }
        }
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(edgeLocatorPos.position, new Vector3(edgeLocatorPos.position.x, edgeLocatorPos.position.y - edgeDistance, transform.position.z));
        Gizmos.DrawLine(groundCheckerPos.position, new Vector3(groundCheckerPos.position.x, groundCheckerPos.position.y - groundDistance, transform.position.z));

        if(facingDir == 1)
        {
            Gizmos.DrawLine(wallLocatorPos.position, new Vector3(wallLocatorPos.position.x + wallDistance, wallLocatorPos.position.y, transform.position.z));
        }
        else
        {
            Gizmos.DrawLine(wallLocatorPos.position, new Vector3(wallLocatorPos.position.x - wallDistance, wallLocatorPos.position.y, transform.position.z));
        }
    }
}
