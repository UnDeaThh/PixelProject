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
    private bool playerDetected;
    [SerializeField] AudioSource sound;
    [SerializeField] AudioSource balanceSound;
    [SerializeField] float timeToLaunch;
    float cntTimeToLaunch;
    [SerializeField] ParticleSystem polvo;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        col.enabled = false;
        rb.gravityScale = 0;
        cntTimeToLaunch = timeToLaunch;
        polvo.gameObject.transform.parent = null;
    }
    private void Update()
    {
        if (!launched)
        {
            if (!playerDetected)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1000, collidesWith);
                if(hit.transform.CompareTag("Player"))
                {
                    playerDetected = true;
                    balanceSound.pitch = Random.Range(0.8f, 1.5f);
                    balanceSound.Play();
                    polvo.Play();
                }
            }
            else
            {
                if(cntTimeToLaunch > 0)
                {
                    cntTimeToLaunch -= Time.deltaTime;
                }
                else
                {
                    rb.gravityScale = 5;
                    col.enabled = true;
                    launched = true;
                }
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
                sound.pitch = Random.Range(0.85f, 1.15f);
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
