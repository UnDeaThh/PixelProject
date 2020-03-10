using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeckAI : BaseEnemy
{
    private int facingDir = 1;
    [SerializeField] float maxWalkSpeed;
    [SerializeField] float environmentCheckDistance;
    [SerializeField] Vector2 attackRange;
    [SerializeField] Transform firstAttackPos;
    [SerializeField] Transform environmentLocatorPos;

    private bool isSunken = true;
    public bool firstAttack = false;
    private bool ffAtack = false;
    private bool groundInFront;
    private bool wallInFront;

    private Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        mat = GetComponentInChildren<SpriteRenderer>().material;
        
    }

    private void Update()
    {
        FirstAttack();
        CheckEnvironment();
        Dead();
    }
    private void FixedUpdate()
    {
        if (!isSunken)
        {
            if(groundInFront && !wallInFront)
            {
                rb.AddForce(new Vector2(facingDir * movSpeed * Time.fixedDeltaTime, 0f), ForceMode2D.Force);
            }
            else
            {
                Flip();
            }

            if (rb.velocity.x >= maxWalkSpeed)
            {
                rb.velocity = new Vector2(maxWalkSpeed, rb.velocity.y);
            }
        }
    }
    void FirstAttack()
    {
        if (firstAttack && !ffAtack)
        {
            Collider2D col = Physics2D.OverlapBox(firstAttackPos.position, attackRange, 0, whatIsDetected);
            if (col != null)
            {
                if (col.CompareTag("Player"))
                {
                    col.GetComponent<PlayerController2>().PlayerDamaged(damage, transform.position);
                }
            }
            ffAtack = true;
        }
    }
    void CheckEnvironment()
    {
        if (!isSunken)
        {
            groundInFront = Physics2D.Raycast(environmentLocatorPos.position, Vector2.down, environmentCheckDistance, whatIsDetected);
            wallInFront = Physics2D.Raycast(environmentLocatorPos.position, transform.right, environmentCheckDistance, whatIsDetected);
        }
    }
    void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
        facingDir *= -1;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isSunken = false;
            //anim.SetTrigger("firstAttack");
        }
    }

    public override void Dead()
    {
        base.Dead();
    }

    public override void TakeDamage(int damage)
    {
        if (!isSunken)
        {
            base.TakeDamage(damage);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(firstAttackPos.position, attackRange);
        Gizmos.color = Color.green;
        if (facingDir == 1)
        {
            Gizmos.DrawLine(firstAttackPos.position, new Vector3(firstAttackPos.position.x + environmentCheckDistance, firstAttackPos.position.y, transform.position.z));
        }
        else
        {
            Gizmos.DrawLine(firstAttackPos.position, new Vector3(firstAttackPos.position.x - environmentCheckDistance, firstAttackPos.position.y, transform.position.z));
        }
    }
}
