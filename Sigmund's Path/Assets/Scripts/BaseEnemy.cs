﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    public int nLifes;
    [HideInInspector] public bool isAlive;
    [HideInInspector] public bool isStuned;

    public float speedX;

    public float detectionRange;

    public void Dead()
    {
        if(nLifes <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        nLifes -= damage;
        Debug.Log(nLifes);
    }

    public void Stuned()
    {
        isStuned = true;
    }
}
