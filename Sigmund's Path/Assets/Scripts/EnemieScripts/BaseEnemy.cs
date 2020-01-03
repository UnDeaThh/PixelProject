using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    public int nLifes;
    [HideInInspector] public bool isAlive;
    [HideInInspector] public bool isStuned;


    public float movSpeed;
    public float timeStuned = 1f;
    [HideInInspector] public float currentTimeStuned = 0;

    [HideInInspector]public float detectionRange;
    public LayerMask whatIsDetected;

    public GameObject pickUp;

    public virtual void Dead()
    {
        if(nLifes <= 0)
        {
            if(pickUp != null){
                Instantiate(pickUp, transform.position, Quaternion.identity);
            }
            
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
        if (isStuned)
        {
            if (currentTimeStuned < timeStuned)
            {
                currentTimeStuned += Time.deltaTime;
            }
            else
            {
                isStuned = false;
                currentTimeStuned = 0;
            }
        }
    }
}
