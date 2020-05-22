using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BossNeck : BossBase
{
    private State actualState;

    private Animator anim;
    private Transform playePos;
    [SerializeField] LayerMask playerLayer;

    private float distancePlayer;

    private float distanceAtack;
    [SerializeField] float movSpeed;
    [SerializeField] float minDistanceDobleAttack;
    [SerializeField] float maxDistanceDobleAttack;
    [SerializeField] float timeToAttack;
    private float cntTimeToAttack;
    [SerializeField] float timeStill;
    private float cntTimeStill;

    [SerializeField] Vector2 atackArea;
    [SerializeField] Transform atackPos;

    private bool playerInFront;
    private bool doDobleAttack = false;
    private bool canWalk;
    public State ActualState { get => actualState; set => actualState = value; }
    public bool DoDobleAttack { get => doDobleAttack; set => doDobleAttack = value; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        mat = sprite.material;
        col = GetComponent<Collider2D>();
        anim = GetComponentInChildren<Animator>();
        actualState = State.Nothing;
        playePos = GameObject.FindGameObjectWithTag("Player").transform;

        facingDir = -1;
        cntTimeToAttack = timeToAttack;
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), playePos.GetComponent<Collider2D>());
    }

    // Update is called once per frame
    void Update()
    {
        switch (actualState)
        {
            case State.Enter: // La animacion al entrar en la sala
                actualState = State.H1;
                break;
            case State.H1: // Aqui meteremos todo el comportamiento del NeckBoss ya que no va por fases, sino por situaciones

                if(facingDir < 0 && playePos.position.x > transform.position.x)
                {
                    Flip();
                }
                else if(facingDir > 0 && playePos.position.x < transform.position.x)
                {
                    Flip();
                }

                distancePlayer = Vector2.Distance(transform.position, playePos.position);
                if (facingDir > 0)
                {
                    distanceAtack = Vector2.Distance(transform.position, new Vector3(atackPos.position.x + atackArea.x / 2, transform.position.y, atackPos.position.z));
                }
                else if (facingDir < 0)
                {
                    distanceAtack = Vector2.Distance(transform.position, new Vector3(atackPos.position.x - atackArea.x / 2, transform.position.y, atackPos.position.z));
                }

                if (distancePlayer <= distanceAtack)
                {
                    canWalk = false;
                    if(cntTimeStill != timeStill)
                    {
                        cntTimeStill = timeStill;
                    }
                    playerInFront = true;
                    if (!doDobleAttack)
                    {
                        if (cntTimeToAttack > 0)
                        {
                            cntTimeToAttack -= Time.deltaTime;
                        }
                        else
                        {
                            doDobleAttack = true;
                            cntTimeToAttack = timeToAttack;
                        }
                    }
                }
                else
                {
                    playerInFront = false;
                    if (!doDobleAttack) // El player esta lejos y no estamos atacando
                    {
                        if(cntTimeStill > 0)
                        {
                            cntTimeStill -= Time.deltaTime;
                        }
                        else
                        {
                             canWalk = true;
                        }
                    }
                }
                break;
        }
        UpdateAnims();
    }

    private void FixedUpdate()
    {
        if(actualState == State.H1)
        {
            if (canWalk)
            {
                rb.velocity = new Vector2(movSpeed * Time.deltaTime * facingDir, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }
    }
    public void CloseAttack()
    {
        Collider2D col = Physics2D.OverlapBox(atackPos.position, atackArea, 0, playerLayer);
        if (col != null)
        {
            if (playePos.GetComponent<PlayerParry>().IsParry)
            {
                //StartStun();
            }
            else
            {
                col.gameObject.GetComponent<PlayerController2>().PlayerDamaged(1, transform.position);
            }
        }
    }

    private void UpdateAnims()
    {
        anim.SetBool("dobleAttack", doDobleAttack);
        anim.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(atackPos.position, atackArea); //Collider ataque doble

        if(facingDir > 0)
        {
            Gizmos.DrawLine(transform.position, new Vector3(atackPos.position.x + atackArea.x/2, transform.position.y, atackPos.position.z));
        }
        else if(facingDir < 0)
        {
            Gizmos.DrawLine(transform.position, new Vector3(atackPos.position.x - atackArea.x/2, transform.position.y, atackPos.position.z));
        }
    }
}
