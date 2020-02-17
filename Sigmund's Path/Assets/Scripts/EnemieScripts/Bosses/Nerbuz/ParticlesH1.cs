using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesH1 : MonoBehaviour
{
    private Collider2D col;
    public AudioSource explosSound;
    private bool destroy = false;
    private SpriteRenderer sprite;
    private void Awake()
    {
        col = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();

    }
    void Start()
    {
        col.enabled = false;
    }

    void StartExplosion()
    {
        col.enabled = true;
        explosSound.Play();
    }

    void DestroyParticle()
    {
        sprite.enabled = false;
        destroy = true;
    }

    private void Update()
    {
        if (destroy)
        {
            if (!explosSound.isPlaying)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController2>().PlayerDamaged(1, transform.position);
        }
    }
}
