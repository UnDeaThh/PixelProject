using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockProjectile : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform target; 
    private Vector2 direction; 
    public float speed;
    public int damage = 1;
    public float angularSpeed;
    void Start()
    {
        Physics2D.IgnoreLayerCollision(9, 9);
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        direction = (target.position - transform.position).normalized;
        rb.AddForce(direction * speed, ForceMode2D.Impulse);
    }

    void Update(){
        transform.Rotate(new Vector3(0f, 0f, angularSpeed));
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Player"){
            Vector2 normal = collision.contacts[0].normal;
            collision.gameObject.GetComponent<PlayerController2>().PlayerDamaged(damage, normal);
            //Particulas de piedra destrullendose o animacion
            //Sonido de golpe
            DeactivateObject();

        }
    }

    void DeactivateObject(){
            Collider2D collider = gameObject.GetComponent<CircleCollider2D>();
            collider.enabled = false;
            Destroy(rb);
            SpriteRenderer spr = gameObject.GetComponentInChildren<SpriteRenderer>();
            Destroy(spr);
            Destroy(gameObject, 0.5f);
    }
}
