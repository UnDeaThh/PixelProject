using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class waterNeck : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] GameObject waterExplos;
    [SerializeField] AudioSource soruce;
    private SpriteRenderer sr;
    private bool destroy = false;
    private Collider2D col;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }
    private void Update()
    {
        if (!destroy)
        {
            transform.localPosition += transform.right * Time.deltaTime * speed;
        }
        else
        {
            if (!soruce.isPlaying)
            {
                Destroy(gameObject);
            }
        }
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

            soruce.pitch = Random.Range(0.8f, 1.2f);
            soruce.Play();
            Instantiate(waterExplos, transform.position, Quaternion.identity);
            sr.enabled = false;
            col.enabled = false;
            destroy = true;
        }
    }
}
