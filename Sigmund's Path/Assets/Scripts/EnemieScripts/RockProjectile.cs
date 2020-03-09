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
    [SerializeField] GameObject ps;
    [SerializeField] AudioSource destroySound;
    private SpriteRenderer sprite;
    void Start()
    {
        Physics2D.IgnoreLayerCollision(9, 9);
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        direction = (target.position - transform.position).normalized;
        sprite = GetComponent<SpriteRenderer>();
        rb.AddForce(direction * speed, ForceMode2D.Impulse);
    }

    void Update(){
        transform.Rotate(new Vector3(0f, 0f, angularSpeed));
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Player"){
          
            collision.gameObject.GetComponent<PlayerController2>().PlayerDamaged(damage, transform.position);
            DeactivateObject();
        }
        else if (collision.transform.CompareTag("Floor"))
        {
            DeactivateObject();
        }
    }

    void DeactivateObject()
    {
        Collider2D col = GetComponent<Collider2D>();
        col.enabled = false;
        sprite.enabled = false;
        destroySound.Play();
        Instantiate(ps, transform.position, Quaternion.identity);
        Destroy(gameObject, 0.1f);
    }
}
