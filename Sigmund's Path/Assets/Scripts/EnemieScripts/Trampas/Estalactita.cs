using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Estalactita : MonoBehaviour
{
    private Rigidbody2D rb;
    public LayerMask collidesWith;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, collidesWith);
        if(hit.transform.CompareTag("Player"))
        {
            rb.gravityScale = 5;
            Debug.Log("Player");
        }
    }
}
