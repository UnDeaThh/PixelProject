using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BermonchAI : BaseEnemy
{
    private bool playerFound = false;
    //Se construye a traves del Script que controla el evento
    public bool bermBuild = false;

    public float highRangeDistance = 3f;
    public float maxRangeDistance = 25f;
    public float timeBtwThrow = 4f;
    private float currentTimeBtwThrow = 4f;
    
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
        if(bermBuild){
            //Desactivar el Collider que detecta al player para que no moleste mas adelante
            if(playerFoundCollider != null){
                playerFoundCollider.enabled = false;
            }

            Attack();
        }

        UpdateAnimations();
    }

    void Attack(){
        //Calculamos a que distancia se encuentra el player
        float distance = Vector2.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);

        if(distance >= 0f && distance < highRangeDistance){
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
        base.TakeDamage(damage);
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            playerFound = true;
        }
    }

    void UpdateAnimations(){
        anim.SetBool("playerFound", playerFound);
    }
}
