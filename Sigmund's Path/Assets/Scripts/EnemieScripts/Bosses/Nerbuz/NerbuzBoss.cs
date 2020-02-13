using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NerbuzBoss : MonoBehaviour
{
    public int life = 200;
    public float speedH1;
    public float maxSpeedH1;
    private Vector2 movDir;
    public Collider2D colH1Confiner;
    private Collider2D nerbuzCol;
    private SpriteRenderer sprite;



    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        nerbuzCol = GetComponent<Collider2D>();
        movDir = RandomVector(0);
    }

    private void Update()
    {
        if(transform.position.x <= colH1Confiner.bounds.min.x)
        {
            movDir = RandomVector(1);
        }
        else if(transform.position.x >= colH1Confiner.bounds.max.x)
        {
            movDir = RandomVector(2);
        }
        if(transform.position.y <= colH1Confiner.bounds.min.y)
        {
            movDir = RandomVector(3);
        }
        else if(transform.position.y >= colH1Confiner.bounds.max.y)
        {
            movDir = RandomVector(4);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = movDir * speedH1 * Time.fixedDeltaTime;
    }

    private Vector2 RandomVector(int number)
    {
        Vector2 movDir;
        movDir = new Vector2();
        switch (number)
        {
            case 0:
                movDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1, 1));
                break;
            case 1:
                movDir = new Vector2(Random.Range(0.1f, 1f), Random.Range(-1f, 1f));
                break;
            case 2:
                movDir = new Vector2(Random.Range(-1f, -0.1f), Random.Range(-1f, 1f));
                break;
            case 3:
                movDir = new Vector2(Random.Range(-1f, 1f), Random.Range(0.1f, 1f));
                break;
            case 4:
                movDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, -0.1f));
                break;
        }
        return movDir;
    }

    public void TakeDamge(int damage)
    {
        life -= damage;
        Debug.Log(life);
        StartCoroutine(Blinking());
    }

    IEnumerator Blinking()
    {
        for (int i = 0; i < 3; i++)
        {
            sprite.color = Color.gray;
            yield return new WaitForSeconds(0.15f);
            sprite.color = Color.white;
            yield return new WaitForSeconds(0.15f);
        }
    }
}
