using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public enum EnemyClass
    {
        Changeling, Bermonch, Tatzel, Nach, Neck
    }
public class BaseEnemy : MonoBehaviour
{
    public int nLifes;
    public int damage;
    [HideInInspector] public bool isAlive;
    [HideInInspector] public bool isStuned;


    public float movSpeed;
    public float timeStuned = 1f;
    [HideInInspector] public float currentTimeStuned = 0;

    [HideInInspector]public float detectionRange;
    public LayerMask whatIsDetected;

    public SoulTrail soul;
    public EnemyClass enemyType;


    public virtual void Dead()
    {
        if(nLifes <= 0)
        {
           if(soul != null)
           {
               soul.enabled = true;
               soul.MoneyValor(enemyType);
               soul.transform.parent = null;
               soul.gameObject.transform.position = transform.position;
               //Instantiate(soul, transform.position, Quaternion.identity);
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
