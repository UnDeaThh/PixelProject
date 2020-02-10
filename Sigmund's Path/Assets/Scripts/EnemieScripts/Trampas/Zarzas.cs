using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zarzas : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController2>().PlayerDamaged(1, transform.position);
        }
    }
}
