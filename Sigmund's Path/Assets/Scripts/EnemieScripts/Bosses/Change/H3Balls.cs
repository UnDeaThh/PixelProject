using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H3Balls : MonoBehaviour
{
    [SerializeField] int speed;
    SpriteRenderer spriteRenderer;
    [SerializeField] ParticleSystem ps;
    [SerializeField] GameObject lightV;
    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb.AddForce(transform.right * speed, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController2>().PlayerDamaged(1, transform.position);
            spriteRenderer.enabled = false;
            GetComponent<Collider2D>().enabled = false;
            lightV.SetActive(false);
            Destroy( gameObject, 0.1f);
        }
        else if (other.CompareTag("Floor"))
        {
            if (other.transform.name == "LimitDown")
            {
                GetComponent<Collider2D>().enabled = false;
                spriteRenderer.enabled = false;
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                rb.velocity = Vector2.zero;
                rb.gravityScale = 0;
                ps.Play();
                lightV.SetActive(false);
                Destroy(gameObject, 0.7f);
            }
            else
            {
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                rb.velocity = new Vector2(0f, rb.velocity.y);
            }
        }
    }
}
