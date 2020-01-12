using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulPickUp : MonoBehaviour
{
    private int amountOfMoney;
    private AudioSource sound;

    private void Awake()
    {
        amountOfMoney = Random.Range(2 , 10);
        sound = GetComponent<AudioSource>();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Collider2D col = GetComponent<Collider2D>();
            col.enabled = false;
            SpriteRenderer GFX = GetComponentInChildren<SpriteRenderer>();
            GFX.enabled = false;

            Inventory2 inventory = collision.transform.GetComponentInChildren<Inventory2>();
            inventory.WinMoney(amountOfMoney);
            //Lanzar Sonido
            sound.Play();
            Destroy(gameObject, 0.4f);
        }
    }
}
