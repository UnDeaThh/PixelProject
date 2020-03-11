using System.Collections;
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
    [SerializeField] float maxWalkSpeed = 30f;

    private bool playerFound = false;
    //Se construye a traves del Script que controla el evento
    public bool bermBuild = false;
    private bool isAttacking = false;

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
            distance = Vector2.Distance(transform.position, targetPlayer.position);
            //Desactivar el Collider que detecta al player para que no moleste mas adelante
            if (playerFoundCollider != null){
                playerFoundCollider.enabled = false;
            }

            if(distance <= maxRangeDistance) //El player esta a una distancia atacable
            {
                Attack();
            }
            else
            {
                CheckEnvironment();
            }
            Dead();
        }

        UpdateAnimations();
    }

    private void FixedUpdate()
    {
        if (bermBuild)
        {
            if(distance > maxRangeDistance)
            {
                if (groundInFront && !wallInFront)
                {
                    rb.AddForce(new Vector2(facingDir * movSpeed * Time.fixedDeltaTime, 0f), ForceMode2D.Force);
                }
                else
                {
                    Flip();
                }
            }
            else
            {
                rb.velocity = Vector2.zero;
            }

            if(rb.velocity.x >= maxWalkSpeed)
            {
                rb.velocity = new Vector2(maxWalkSpeed, rb.velocity.y);
            }
        }
    }

    void CheckEnvironment()
    {
        groundInFront = Physics2D.Raycast(edgeLocatorPos.position, Vector2.down, toEdgeDistance, floorLayer);
        wallInFront = Physics2D.Raycast(edgeLocatorPos.position, transform.right, toEdgeDistance, floorLayer);
    }

    void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
        facingDir *= -1;
    }
    void Attack(){
        //Calculamos a que distancia se encuentra el player

        if(distance >= 0f && distance < highRangeDistance){
            Collider2D col = Physics2D.OverlapBox(transform.position, attackRange, 0, whatIsDetected);
            if(col != null)
            {
                if(cntTimeBtwAttack <= 0f)
                {
                    isAttacking = true;
                    col.gameObject.GetComponent<PlayerController2>().PlayerDamaged(damage, gameObject.transform.position); //Pongo los vectores al reves ya que en el metodo le doy la vuelta
                    cntTimeBtwAttack = timeBtwAttack;
                }
                else if(cntTimeBtwAttack > 0f)
                {
                    cntTimeBtwAttack -= Time.deltaTime;
                    isAttacking = false;
                }
            }
        }
        
        #region HighAttackRange
        if(distance >= highRangeDistance && distance <= maxRangeDistance){
            if(throwRockPrefab != null){
                if(cntTimeBtwAttack <= 0f){
                    Instantiate(throwRockPrefab, transform.position, Quaternion.identity);
                    cntTimeBtwAttack = timeBtwAttack;
                }
                else{
                    cntTimeBtwAttack -= Time.deltaTime;
                }
            }
        }
        #endregion
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
            cntTimeBtwAttack = timeBtwAttack;
        }
    }

    public override void Dead()
    {
        base.Dead();
    }

    void UpdateAnimations(){
        anim.SetBool("playerFound", playerFound);
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
