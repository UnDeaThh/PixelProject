using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BermonchAI : BaseEnemy
{
    private bool playerFound = false;
    //Se construye a traves del Script que controla el evento
    public bool bermBuild = false;
    private bool firstFrame = true;
    private bool isAttacking = false;

    [Range(1.0f, 10.0f)]
    public float highRangeDistance = 3f;
    [Range(10.0f, 25f)]
    public float maxRangeDistance = 25f;
    public float timeBtwThrow = 4f;
    private float currentTimeBtwThrow = 4f;
    public float timeBtwPunch = 3f;
    [SerializeField]
    private float currentTimeBtwPunch = 0;

    public Vector2 attackRange;
    private RaycastHit2D leftRay;
    private RaycastHit2D rightRay;


    public GameObject throwRockPrefab;
    public Collider2D playerFoundCollider;
    private Animator anim;

    private void Awake() {
        anim = GetComponentInChildren<Animator>();
        //El Collider de Collision del bermounch no interacciona con el del player
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), 
         GameObject.FindGameObjectWithTag("Player").GetComponent<CapsuleCollider2D>());
    }

    void Update(){
        rightRay = Physics2D.Raycast(transform.position, Vector2.right, attackRange.x/2, whatIsDetected);
        leftRay = Physics2D.Raycast(transform.position, Vector2.left, attackRange.x/2, whatIsDetected);

        if (bermBuild){
            //Desactivar el Collider que detecta al player para que no moleste mas adelante
            if(playerFoundCollider != null){
                playerFoundCollider.enabled = false;
            }
            //Poner tiempos el primer frame despues de construirse
            if (firstFrame)
            {
                currentTimeBtwPunch = timeBtwPunch;
                firstFrame = false;
            }
            Attack();
            Dead();
        }

        UpdateAnimations();
    }

    void Attack(){
        //Calculamos a que distancia se encuentra el player
        float distance = Vector2.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);

        if(distance >= 0f && distance < highRangeDistance){
            Collider2D col = Physics2D.OverlapBox(transform.position, attackRange, 0, whatIsDetected);
            if(col != null)
            {
                if(currentTimeBtwPunch <= 0f)
                {
                    isAttacking = true;
                    if(rightRay)
                    {
                        Debug.Log("isRight");
                        col.gameObject.GetComponent<PlayerController2>().PlayerDamaged(damage, gameObject.transform.position); //Pongo los vectores al reves ya que en el metodo le doy la vuelta
                    }
                    else if (leftRay)
                    {
                        Debug.Log("isLeft");
                        col.gameObject.GetComponent<PlayerController>().PlayerDamaged(damage, gameObject.transform.position); //Pongo los vectores al reves ya que en el metodo le doy la vuelta
                    }
                    currentTimeBtwPunch = timeBtwPunch;
                }
                else if(currentTimeBtwPunch > 0f)
                {
                    currentTimeBtwPunch -= Time.deltaTime;
                    isAttacking = false;
                }
            }
        }
        
        #region HighAttackRange
        if(distance >= highRangeDistance && distance <= maxRangeDistance){
            if(throwRockPrefab != null){
                if(currentTimeBtwThrow <= 0f){
                    Instantiate(throwRockPrefab, transform.position, Quaternion.identity);
                    currentTimeBtwThrow = timeBtwThrow;
                }
                else{
                    currentTimeBtwThrow -= Time.deltaTime;
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
        if(other.CompareTag("Player")){
            playerFound = true;
        }
    }

    void UpdateAnimations(){
        anim.SetBool("playerFound", playerFound);
    }

    private void OnDrawGizmosSelected()
    {
        //if (bermBuild)
        //{
            Gizmos.DrawWireCube(transform.position, attackRange);
            Gizmos.DrawWireSphere(transform.position, highRangeDistance);
            Gizmos.DrawWireSphere(transform.position, maxRangeDistance);
            //RightRaycast
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + (attackRange.x/2), transform.position.y, transform.position.z));
            //LeftRayCast
            Gizmos.DrawLine(transform.position, new Vector3(transform.position.x - (attackRange.x/2), transform.position.y, transform.position.z));
        //}
    }
}
