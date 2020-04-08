﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BermonchAI : BaseEnemy
{
    //MOVEMENT FOR PATROL
    private bool groundInFront;
    private bool wallInFront;
    private int facingDir = 1;
    [SerializeField] LayerMask floorLayer;
    [SerializeField] Transform edgeLocatorPos;
    [SerializeField] float toEdgeDistance;

    private bool playerFound = false;
    //Se construye a traves del Script que controla el evento
    public bool bermBuild = false;
    private bool closeAttack = false;
    private bool rangeAttack = false;

    [Range(1.0f, 10.0f)]
    [SerializeField] float highRangeDistance = 3f;
    [Range(10.0f, 25f)]
    [SerializeField] float maxRangeDistance = 25f;
    [SerializeField] float timeBtwAttack;
    private float cntTimeBtwAttack;

    public Vector2 attackRange;
    public GameObject throwRockPrefab;
    public Collider2D playerFoundCollider;

    private Transform targetPlayer;
    private float distance;

    private Rigidbody2D rb;

    public float CntTimeBtwAttack { get => cntTimeBtwAttack; set => cntTimeBtwAttack = value; }
    public bool RangeAttack1 { get => rangeAttack; set => rangeAttack = value; }
    public bool CloseAttack1 { get => closeAttack; set => closeAttack = value; }

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        //El Collider de Collision del bermounch no interacciona con el del player
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), 
        GameObject.FindGameObjectWithTag("Player").GetComponent<CapsuleCollider2D>());
        targetPlayer = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        mat = GetComponentInChildren<SpriteRenderer>().material;
    }

    void Update(){
        if (bermBuild)
        {
            if(nLifes > 0)
            {
                distance = Vector2.Distance(transform.position, targetPlayer.position);
                //Desactivar el Collider que detecta al player para que no moleste mas adelante
                if (playerFoundCollider != null){
                    playerFoundCollider.enabled = false;
                }

                if(distance <= maxRangeDistance) //El player esta a una distancia atacable
                {
                    Attack();
                    CloseAttack();
                    RangeAttack();
                }
                else
                {
                    CheckEnvironment();
                }
      
            }

            Dead();
        }

        UpdateAnimations();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    void CheckEnvironment()
    {
        groundInFront = Physics2D.Raycast(edgeLocatorPos.position, Vector2.down, toEdgeDistance, floorLayer);
        wallInFront = Physics2D.Raycast(edgeLocatorPos.position, transform.right, toEdgeDistance, floorLayer);
    }

    void ApplyMovement()
    {
        if (bermBuild)
        {
            if(distance > maxRangeDistance)
            {
                if(groundInFront && !wallInFront)
                {
                    rb.velocity = new Vector2(facingDir * movSpeed * Time.fixedDeltaTime, 0f);
                }
                else
                {
                    Flip();
                }
            }
            else
            {
                rb.velocity = Vector2.zero;
                if (targetPlayer.position.x < transform.position.x)
                {
                    transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
                    facingDir = -1;
                }
                else
                {
                    transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
                    facingDir = 1;
                }
            }
        }
    }

    void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
        facingDir *= -1;
    }
    void Attack()
    {
        //Calculamos a que distancia se encuentra el player
        #region CloseAttack
        if (distance >= 0f && distance < highRangeDistance)
        {
            if(CntTimeBtwAttack <= 0f)
            {
                anim.SetBool("closeAttack", true);

            }
            else if(CntTimeBtwAttack > 0f)
            {
                CntTimeBtwAttack -= Time.deltaTime;
            }
        }
        #endregion

        #region HighAttackRange
        if (distance >= highRangeDistance && distance <= maxRangeDistance){
            if(throwRockPrefab != null)
            {
                if(CntTimeBtwAttack <= 0f)
                {
                    anim.SetBool("rangeAttack", true);
                }
                else
                {
                    CntTimeBtwAttack -= Time.deltaTime;
                }
            }
        }
        #endregion
    }

    void CloseAttack()
    {
        if (closeAttack)
        {
            Collider2D col = Physics2D.OverlapBox(transform.position, attackRange, 0, whatIsDetected);
            if(col != null)
            {
                col.gameObject.GetComponent<PlayerController2>().PlayerDamaged(damage, gameObject.transform.position); //Pongo los vectores al reves ya que en el metodo le doy la vuelta
            }
            anim.SetBool("closeAttack", false);
            cntTimeBtwAttack = timeBtwAttack;
            closeAttack = false;
        }

    }
    void RangeAttack()
    {
        if (rangeAttack)
        {
            Instantiate(throwRockPrefab, transform.position, Quaternion.identity);
            anim.SetBool("rangeAttack", false);
            cntTimeBtwAttack = timeBtwAttack;
            rangeAttack = false;
        }
    }
    public override void TakeDamage(int damage){
        if (bermBuild)
        {
            base.TakeDamage(damage);
        }
        else
            return;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player"))
        {
            playerFound = true;
            CntTimeBtwAttack = timeBtwAttack;
        }
    }

    public override void Dead()
    {
        //base.Dead();
        if (nLifes <= 0 && !oneCallDead)
        {
            //Instantiate Soul desde el Animation Event
            anim.SetTrigger("callDead");
            deadSound.Play();
            Collider2D col = GetComponent<Collider2D>();
            col.enabled = false;
            oneCallDead = true;
        }
        if (IsDisolving)
        {
            fade -= Time.deltaTime;
            if (fade <= 0)
            {
                fade = 0;
                IsDisolving = false;
            }
            mat.SetFloat("_Fade", fade);
        }
        if (callDead)
        {
            if (deadSound)
            {
                if (!deadSound.isPlaying && fade <= 0f)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    void UpdateAnimations(){
        anim.SetBool("playerFound", playerFound);
        anim.SetFloat("movSpeed", Mathf.Abs(rb.velocity.x));
    }

    private void OnDrawGizmosSelected()
    {

        Gizmos.DrawWireCube(transform.position, attackRange);
        Gizmos.DrawWireSphere(transform.position, highRangeDistance);
        Gizmos.DrawWireSphere(transform.position, maxRangeDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(edgeLocatorPos.position, new Vector2(edgeLocatorPos.position.x, edgeLocatorPos.position.y - toEdgeDistance));
        if (facingDir == 1)
        {
            Gizmos.DrawLine(edgeLocatorPos.position, new Vector3(edgeLocatorPos.position.x + toEdgeDistance, edgeLocatorPos.position.y, transform.position.z));
        }
        else
        {
            Gizmos.DrawLine(edgeLocatorPos.position, new Vector3(edgeLocatorPos.position.x - toEdgeDistance, edgeLocatorPos.position.y, transform.position.z));
        }

    }
}
