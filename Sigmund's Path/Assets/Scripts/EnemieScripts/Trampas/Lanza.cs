using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lanza : MonoBehaviour
{
    public SpriteRenderer sprite;
    public float speed;


    private void Update()
    {
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
