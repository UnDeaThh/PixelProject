using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehaviour : MonoBehaviour
{
    public float movSpeed;
    private int damage = 1;
    private void Update()
    {
        transform.Translate(movSpeed * Time.deltaTime, 0f, 0f);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Vector2 normal = Vector2.right;
            if(other.GetComponent<PlayerParry>().isParry == false)
            {
                other.GetComponent<PlayerController>().Damaged(damage, normal);
            }
        }
    }
}
