﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bombs : MonoBehaviour
{
    public float timeToExplode;
    public float explosionRadius;
    private float cntTimeToExplode;
    private bool exploding = false;
    private bool alreadyExploted;

    public LayerMask whatIsHitting;
    public GameObject particlePref;

    public SpriteRenderer spriteBomb;
    private Collider2D col;
    private Rigidbody2D rb;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip[] clips = new AudioClip[2];
    private void Awake()
    {
        Physics2D.IgnoreLayerCollision(13, 10);
        cntTimeToExplode = timeToExplode;
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        source.clip = clips[0];
        source.volume = 0.2f;
        source.Play();
    }
    private void Update()
    {
        if(cntTimeToExplode  > 0)
        {
            cntTimeToExplode -= Time.deltaTime;
            exploding = false;
        }
        else
        {
            exploding = true;
        }

        if (exploding && !alreadyExploted)
        {
            Collider2D explosCol = Physics2D.OverlapCircle(transform.position, explosionRadius, whatIsHitting);
            if (explosCol != null)
            {
                explosCol.gameObject.GetComponent<DestructibleWalls>().WallDestroyed();
            }
            else
                print("NO WALL");

            Instantiate(particlePref,transform.position, Quaternion.identity);
            spriteBomb.enabled = false;
            col.enabled = false;
            Destroy(rb);

            exploding = false;
            alreadyExploted = true;
            source.clip = clips[1];
            source.Play();
            source.volume = 1f;
            Destroy(gameObject, 0.5f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
