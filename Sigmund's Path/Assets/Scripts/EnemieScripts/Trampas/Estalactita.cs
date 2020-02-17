using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Estalactita : MonoBehaviour
{
    private Rigidbody2D rb;
    public LayerMask collidesWith;
    private Collider2D col;
    public GameObject ps;
    private SpriteRenderer sprite;
    private bool launched;
    private AudioSource sound;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        col.enabled = false;
        rb.gravityScale = 0;
        sound = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (!launched)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, collidesWith);
            if(hit.transform.CompareTag("Player"))
            {
                rb.gravityScale = 5;
                col.enabled = true;
                launched = true;
                Debug.Log("Player");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController2>().PlayerDamaged(1, transform.position);
        }
        else if (collision.CompareTag("Floor"))
        {
            if (launched)
            {
                sound.Play();
                col.enabled = false;
                sprite.enabled = false;
                rb.velocity = Vector2.zero;
                Vector2 posicion = new Vector2(col.transform.position.x, col.transform.position.y - col.bounds.size.y/2 - 0.2f);
                Instantiate(ps, posicion, Quaternion.identity);
                Destroy(gameObject, 0.3f);
            }
        }
    }
}
