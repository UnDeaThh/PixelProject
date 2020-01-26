using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulTrail : MonoBehaviour
{
    private int moneyToAdd; 
    public float speed = 10f;
    private Transform target;
    private bool playerTouched = false;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        if(target != null && !playerTouched)
        {
            transform.LookAt(target);
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
        if(playerTouched){
            Destroy(gameObject);
        }
    }

    public void MoneyValor(EnemyClass enemyType){
        switch (enemyType){
            case EnemyClass.Changeling:
                moneyToAdd = 26;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player")
        {
            Inventory2.inventory.WinMoney(moneyToAdd);
            playerTouched = true;
            
        }
    }
}
