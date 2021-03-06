﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionBall : MonoBehaviour
{
    [SerializeField] ParticleSystem ps;
    SpriteRenderer spriteRenderer;
    [SerializeField] GameObject lightV;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
            if(other.transform.name == "LimitDown")
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
