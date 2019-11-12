using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : BaseEnemy
{
    //REFERENCIAS
    private Rigidbody2D rb2d;
    private PlayerController thePlayer;

    //ATACK
    private bool playerDetected = false;
    public LayerMask whatIsDetected;

    private bool hasAttacked;
    private float startTimeStill = 1f;
    private float timeStill;

    private int damage = 1;
    

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        thePlayer = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        CheckPlayer();
        Chase();
        Dead();
        StayForMoment();
    }
    void CheckPlayer()
    {
        if(Physics2D.OverlapCircle(transform.position, detectionRange, whatIsDetected))
        {
            playerDetected = true; ;
        }
    }

    void Chase()
    {
        if (playerDetected && !hasAttacked)
        {
            transform.position = Vector2.MoveTowards(transform.position ,thePlayer.transform.position, speedX * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Player")
        {
            Vector2 normal = other.contacts[0].normal;
            other.transform.GetComponent<PlayerController>().Damaged(damage, normal);
            timeStill = startTimeStill;
            hasAttacked = true;

        }
    }

    private void StayForMoment()
    {
        if (hasAttacked)
        {
            timeStill -= Time.deltaTime;
            if(timeStill > 0f)
            {
                rb2d.velocity = Vector2.zero;
            }
            else if(timeStill <= 0)
            {
                hasAttacked = false;
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
