﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulPickUp : MonoBehaviour
{
    private int moneyToAdd;
    private AudioSource sound;

    private void Awake()
    {
        moneyToAdd = Random.Range(10, 20);
        sound = GetComponent<AudioSource>();
    }

    public void MoneyValor(EnemyClass enemyType)
    {
        switch (enemyType)
        {
            case EnemyClass.Changeling:
                moneyToAdd = 50;
                break;
        }
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
            inventory.WinMoney(moneyToAdd);
            //Lanzar Sonido
            sound.Play();
            Destroy(gameObject, 0.4f);
        }
    }
}
