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

    public bool groundFound;
    private bool isGrounded;
    public bool wallFound;
    private bool makeJump;

    private Rigidbody2D rb;
    public Transform edgeLocatorPos;
    public Transform wallLocatorPos;
    public Transform groundCheckerPos;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        CheckEnvironment();
        MovementLogic();

        base.Stuned();
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
        if (makeJump)
        {
            if(facingDir == 1)
            {
                rb.AddForce(Vector2.one * movSpeed, ForceMode2D.Impulse);
            }
            else if(facingDir == -1)
            {
                rb.AddForce(new Vector2(-1f, 1f) * movSpeed, ForceMode2D.Impulse);
            }
        }
    }
    public override void Dead()
    {
        base.Dead();
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
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
