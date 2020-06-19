using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulPickUp : MonoBehaviour
{
    private int moneyToAdd;
    private AudioSource sound;
    private bool picked;
    private AddingPickUp addingPickUp;
    float alphaReduce = 1;
    bool touchPinchos;

    SpriteRenderer GFX;
    private void Awake()
    {
        sound = GetComponent<AudioSource>();
        Physics2D.IgnoreLayerCollision(11, 11);
        Physics2D.IgnoreLayerCollision(11, 9);
        GFX = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        addingPickUp = GameObject.Find("BGMoneyAdd").gameObject.GetComponent<AddingPickUp>();
    }
    private void Update()
    {
        if (picked)
        {
            if (sound.isPlaying)
            {
                return;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        if (touchPinchos)
        {
            alphaReduce -= 0.02f;
            GFX.material.SetFloat("_SmoothFade", alphaReduce);
            if(alphaReduce <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
    public void MoneyValor(EnemyClass enemyType)
    {
        switch (enemyType)
        {
            case EnemyClass.Changeling:
                moneyToAdd = Random.Range(2, 7);
                break;
            case EnemyClass.Nach:
                moneyToAdd = Random.Range(2, 6);
                break;
            case EnemyClass.Bermonch:
                moneyToAdd = Random.Range(4, 11);
                break;
            case EnemyClass.Tatzel:
                moneyToAdd = Random.Range(3, 10);
                break;
            case EnemyClass.Neck:
                moneyToAdd = Random.Range(4, 12);
                break;
            default:
                moneyToAdd = Random.Range(15,20);
                break;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            picked = true;
            Collider2D col = GetComponent<Collider2D>();
            col.enabled = false;
            GFX.enabled = false;

            Inventory2 inventory = collision.transform.GetComponentInChildren<Inventory2>();
            inventory.WinMoney(moneyToAdd);
            sound.Play();


            addingPickUp.PrintPickUpInfo(GFX.sprite, moneyToAdd);
        }
        else if (collision.transform.CompareTag("Pinchos"))
        {
            touchPinchos = true;
        }
    }
}
