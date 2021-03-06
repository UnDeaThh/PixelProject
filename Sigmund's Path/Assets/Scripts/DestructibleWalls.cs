﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleWalls : MonoBehaviour
{
    private bool isDestroyed = false;

    public ParticleSystem psDestroy;

    public SpriteRenderer wallSprite;
    public SpriteRenderer caveSprite;
    private Animator anim;

    private Collider2D col;
    [SerializeField] SpriteRenderer[] tiles;

    public bool IsDestroyed { get => isDestroyed; set => isDestroyed = value; }
    [SerializeField] int numberOfWall;
    private void Awake()
    {
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        
    }
    private void Start()
    {
        if (ScenesManager.scenesManager.DestruibleWall[numberOfWall] == true)
        {
            Destroy(gameObject);
        }
    }
    public void WallDestroyed()
    {
        IsDestroyed = true;
       // psDestroy.Play();
        col.enabled = false;
        wallSprite.enabled = false;
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].enabled = false;
        }
        anim.SetTrigger("FadeOut");
        ScenesManager.scenesManager.DestruibleWall[numberOfWall] = true;
        SaveSystem.SaveSceneData(ScenesManager.scenesManager);
    }
}
