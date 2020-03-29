using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleWalls : MonoBehaviour
{
    private bool isDestroyed = false;

    public ParticleSystem psHit;
    public ParticleSystem psDestroy;

    public SpriteRenderer wallSprite;
    public SpriteRenderer caveSprite;
    private Animator anim;

    private Collider2D col;

    public bool IsDestroyed { get => isDestroyed; set => isDestroyed = value; }

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        
    }
    public void WallDestroyed()
    {
        IsDestroyed = true;
       // psDestroy.Play();
        col.enabled = false;
        wallSprite.enabled = false;
        anim.SetTrigger("FadeOut");
    }

    void WallHitted()
    {
        //psHit.Play();
    }
}
