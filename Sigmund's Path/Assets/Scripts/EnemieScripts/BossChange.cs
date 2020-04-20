using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChange : BossBase
{
    private void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        mat = sprite.material;
        col = GetComponent<Collider2D>();
    }


    private void Update()
    {
        Dead();
    }
}
