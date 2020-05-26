using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterNeck : MonoBehaviour
{
    [SerializeField] float speed;
    private void Update()
    {
        transform.localPosition += transform.right * Time.deltaTime * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController2>().PlayerDamaged(1, transform.position);
            Debug.Log("BRU");
        }
        else if (collision.CompareTag("Floor"))
        {
            //Sonido choque ola
            //particulas explosion ola
            Destroy(gameObject);   
        }
    }
}
