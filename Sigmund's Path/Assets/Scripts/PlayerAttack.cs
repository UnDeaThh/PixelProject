using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private float timeBtwttack;
    public float startTimeBtwAttack;
    private bool canAttack;

    public float attackRange;
    public Transform attackPos;
    public LayerMask whatIsEnemie;
    public int damage;

    private Animator anim;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
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
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (canAttack)
            {
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemie);
                for(int i = 0; i < enemiesToDamage.Length; i++)
                {
                    enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(damage);
                }
                anim.SetTrigger("Attack");
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
