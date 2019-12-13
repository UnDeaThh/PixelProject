using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    public int nLifes;
    [HideInInspector] public bool isAlive;
    [HideInInspector] public bool isStuned;

    public float movSpeed;

    public float detectionRange;
    public LayerMask whatIsDetected;

    public virtual void Dead()
    {
        if(nLifes <= 0)
        {
            Destroy(gameObject);
        }
    }

    public virtual void TakeDamage(int damage)
    {
        nLifes -= damage;
        Debug.Log(nLifes);
    }


    public void Stuned()
    {
        isStuned = true;
    }
}
