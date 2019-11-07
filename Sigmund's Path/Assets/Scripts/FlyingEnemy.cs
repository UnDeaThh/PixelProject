using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : BaseEnemy
{
    //REFERENCIAS
    private Rigidbody2D rb2d;
    private PlayerController thePlayer;
    private bool playerDetected = false;
    public LayerMask whatIsDetected;
    

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
        if (playerDetected)
        {
            transform.position = Vector2.MoveTowards(transform.position ,thePlayer.transform.position, speedX);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
