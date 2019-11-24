using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 2;
    private bool isStuned = false;

    //ATACK
    public Collider2D col;
    public Transform attackPos;
    public float attackRange;
    public LayerMask whatIsAttack;

    private void Awake()
    {
        col.enabled = false;
    }
    void Update()
    {
        Attack();
        Dead();
    }

    void Attack()
    {
        if (!isStuned)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                col.enabled = true;
                
            }
        }
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log(health);
    }

    public void Stuned()
    {
        isStuned = true;
        col.enabled = false;
        Debug.Log("Estuneado");
        isStuned = false;
    }

    public void Dead()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
