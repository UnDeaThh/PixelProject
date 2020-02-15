using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesH1 : MonoBehaviour
{
    private Collider2D col;

    void Start()
    {
        col.enabled = false;
    }

    void Explosion()
    {
        col.enabled = true;
    }

    void DestroyParticle()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController2>().PlayerDamaged(1, transform.position);
        }
    }
}
