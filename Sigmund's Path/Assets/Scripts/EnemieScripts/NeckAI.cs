using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeckAI : BaseEnemy
{
    private int facingDir = 1;
    [SerializeField] float environmentCheckDistance;
    [SerializeField] float underWaterDistance;
    [SerializeField] float leaveWaterSpeed;
    [SerializeField] float timeBtwAttacks;
    [SerializeField] float detectionDistance;
    private float cntTimeBtwAttacks;

    [SerializeField] Vector2 attackRange;
    [SerializeField] Vector2 firstAttackRange;
    [SerializeField] Transform firstAttackPos;
    [SerializeField] Transform environmentLocatorPos;
    [SerializeField] Transform attackPos;
    private Transform firstSon;
    [SerializeField] Collider2D colTrigger;
    [SerializeField] LayerMask floorMask;

    public bool firstAttack = false;
    private bool firstAttackFinished;
    private bool ffAtack = false;
    private bool groundInFront;
    private bool wallInFront;
    private bool playerInFront = false;
    public bool isAttacking;
    public bool makeAnAttack;

    private Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        mat = GetComponentInChildren<SpriteRenderer>().material;
        sprite = GetComponentInChildren<SpriteRenderer>();
        sprite.enabled = false;
        firstSon = transform.GetChild(0).gameObject.transform;
        firstSon.position = new Vector2(transform.position.x, transform.position.y - underWaterDistance);
        Physics2D.IgnoreCollision(transform.GetComponent<Collider2D>(), GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>());
    }

    private void Update()
    {
        FirstAttack();
        CheckEnvironment();
        CanAttack();
        Attack();
        Dead();
    }
    private void FixedUpdate()
    {
        ApplyMovement();
    }
    void FirstAttack()
    {
        if (firstAttack && !ffAtack)
        {
            Collider2D col = Physics2D.OverlapBox(firstAttackPos.position, firstAttackRange, 0, whatIsDetected);
            if (col != null)
            {
                if (col.CompareTag("Player"))
                {
                    col.GetComponent<PlayerController2>().PlayerDamaged(damage, transform.position);
                }
            }
            ffAtack = true;
            firstAttackFinished = true;
        }
    }
    void CheckEnvironment()
    {
        if (firstAttackFinished)
        {
            groundInFront = Physics2D.Raycast(environmentLocatorPos.position, Vector2.down, environmentCheckDistance, floorMask);
            wallInFront = Physics2D.Raycast(environmentLocatorPos.position, transform.right, environmentCheckDistance, floorMask);
            playerInFront = Physics2D.Raycast(new Vector2(transform.position.x, attackPos.position.y), transform.right, detectionDistance, whatIsDetected);
        }
    }

    void ApplyMovement()
    {
        if (firstAttackFinished)
        {
            if (!playerInFront)
            {
                if (groundInFront && !wallInFront)
                {
                    rb.velocity = new Vector2(facingDir * movSpeed * Time.deltaTime, rb.velocity.y);
                }
                else
                {
                    Flip();
                }
            }
        }
    }
    void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
        facingDir *= -1;
    }

    void CanAttack()
    {
        if (playerInFront)
        {
            if(cntTimeBtwAttacks > 0f)
            {
                cntTimeBtwAttacks -= Time.deltaTime;
            }
            else
            {
                isAttacking = true;
                cntTimeBtwAttacks = timeBtwAttacks;
            }
        }
    }
    void Attack()
    {
        if (makeAnAttack)
        {
            Collider2D col = Physics2D.OverlapBox(attackPos.position, attackRange, 0, whatIsDetected);
            if(col != null)
            {
                col.gameObject.GetComponent<PlayerController2>().PlayerDamaged(damage, transform.position);
            }
            Destroy(col);
            makeAnAttack = false;
        }
    }

    //El player pasa por encima del agua
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            sprite.enabled = true;
            anim.SetTrigger("playerAbove");
            colTrigger.enabled = false;
        }
    }

    public override void Dead()
    {
        base.Dead();
    }

    public override void TakeDamage(int damage)
    {
        if (firstAttackFinished)
        {
            base.TakeDamage(damage);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (!firstAttackFinished)
        {
            Gizmos.DrawWireCube(firstAttackPos.position, firstAttackRange);
        }

        Gizmos.DrawWireCube(attackPos.position, attackRange);
        Gizmos.color = Color.black;
        if (facingDir == 1)
        {
            Gizmos.DrawLine(environmentLocatorPos.position, new Vector3(environmentLocatorPos.position.x + environmentCheckDistance, environmentLocatorPos.position.y, transform.position.z));
            Gizmos.DrawLine(new Vector2(transform.position.x, attackPos.position.y), new Vector2(transform.position.x + detectionDistance, attackPos.position.y ));
        }
        else
        {
            Gizmos.DrawLine(environmentLocatorPos.position, new Vector3(environmentLocatorPos.position.x - environmentCheckDistance, environmentLocatorPos.position.y, transform.position.z));
            Gizmos.DrawLine(new Vector2(transform.position.x, attackPos.position.y), new Vector2(transform.position.x - detectionDistance, attackPos.position.y ));
        }
        Gizmos.DrawLine(environmentLocatorPos.position, new Vector3(environmentLocatorPos.position.x, environmentLocatorPos.position.y - environmentCheckDistance, transform.position.z));
        
    }
}
