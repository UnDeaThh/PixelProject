using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionBall : MonoBehaviour
{
    Animator anim;

    private void Start()
    {
        //anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<Collider2D>().enabled = false;
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            other.GetComponent<PlayerController2>().PlayerDamaged(1, transform.position);
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
            Destroy(gameObject);
        }
        else if (other.CompareTag("Floor"))
        {
            GetComponent<Collider2D>().enabled = false;
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
            Destroy(gameObject);
        }
    }
}
