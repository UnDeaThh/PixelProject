using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 2;

    void Update()
    {
        Dead();
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log(health);
    }

    public void Dead()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
