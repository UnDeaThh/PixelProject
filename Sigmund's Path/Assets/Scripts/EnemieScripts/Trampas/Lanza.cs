﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lanza : MonoBehaviour
{
    public SpriteRenderer sprite;
    public float speed;
    private float timeIgnoring = 0.2f;
    private float cntTimeIgnoring;

    private void Start()
    {
        Physics2D.IgnoreLayerCollision(12, 8);
        cntTimeIgnoring = timeIgnoring;
    }
    private void Update()
    {
        if (cntTimeIgnoring > 0)
        {
            cntTimeIgnoring -= Time.deltaTime;
        }
        else
            Physics2D.IgnoreLayerCollision(12, 8, false);

        transform.position += transform.right * speed * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController2>().PlayerDamaged(1, transform.position);
            sprite.enabled = false;
            Destroy(gameObject, 1f);
        }
        else if (collision.CompareTag("Floor"))
        {
            sprite.enabled = false;
            speed = 0;
            Destroy(gameObject, 0.5f);
        }
        
    }
}