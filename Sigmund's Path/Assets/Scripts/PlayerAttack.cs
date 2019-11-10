using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int damage;

    private float timeBtwttack;
    public float startTimeBtwAttack;
    public float attackRange;

    private bool canAttack;

    private Vector3 frontAttackPos = new Vector3(1.5f, 0.0f, 0.0f);
    private Vector3 upAttackPos = new Vector3(0.0f, 2f, 0.0f);

    public Transform attackPos;
    private Transform playerPos;

    public LayerMask whatIsEnemie;

    private PlayerController plController;
    

    void Awake()
    {
    
        playerPos = GetComponent<Transform>();
        plController = GetComponent<PlayerController>();
    }
    void Update()
    {
        CheckIfCanAttack();
        Attack();
    }

    void CheckIfCanAttack()
    {
        if(timeBtwttack <= 0)
        {
            canAttack = true;
            
        }
        else
        {
            timeBtwttack -= Time.deltaTime;
        }

    }

    void Attack()
    {
        if (canAttack)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && !Input.GetKey(KeyCode.W))
            {
                if (plController.facingRight == 1)
                {
                    Vector3 finalPos = playerPos.position + frontAttackPos;
                    attackPos.position = finalPos;
                }
                else if(plController.facingRight == -1)
                {
                    Vector3 finalPos = playerPos.position + (-frontAttackPos);
                    attackPos.position = finalPos;
                }
                
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemie);
                for(int i = 0; i < enemiesToDamage.Length; i++)
                {
                    if(enemiesToDamage[i].tag == "Enemy")
                    {
                    enemiesToDamage[i].GetComponent<BaseEnemy>().TakeDamage(damage);
                    }
                }
                canAttack = false;
                timeBtwttack = startTimeBtwAttack;
            }
            else if(Input.GetKeyDown(KeyCode.Mouse0) && Input.GetKey(KeyCode.W))
            {
                Debug.Log("UpAttack");
                Vector3 finalPos = playerPos.position + upAttackPos;

                attackPos.position = finalPos;
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemie);
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    enemiesToDamage[i].GetComponent<BaseEnemy>().TakeDamage(damage);
                }

                canAttack = false;
                timeBtwttack = startTimeBtwAttack;
            }
        }
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);

    }
}
