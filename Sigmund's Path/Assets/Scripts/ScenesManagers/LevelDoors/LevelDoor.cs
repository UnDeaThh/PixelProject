﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelDoor : MonoBehaviour
{
    public PlayerController2 player;
    public Inventory2 inventory;
    private PlayerAttack plAttack;
    public LevelManager levelManager;
    private Animator anim;
    [SerializeField] GameObject canvasDoor;
    [SerializeField] SpriteRenderer spriteRenderer;
    private bool alreadyEntered;

    public int sceneToLoad;

    public Animator Anim { get => anim; set => anim = value; }

    private void Start()
    {
        spriteRenderer.enabled = false;
        if(canvasDoor == null)
        {
            canvasDoor = transform.Find("CanvasDoor").gameObject;
        }
        canvasDoor.SetActive(true);

        Physics2D.IgnoreLayerCollision(9, 10, false);
        sceneToLoad += 3;
        alreadyEntered = false;
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController2>();
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory2>();
        plAttack = player.gameObject.GetComponent<PlayerAttack>();
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (!alreadyEntered)
            {
                StartCoroutine(LoadLevel());
                other.GetComponent<PlayerController2>().isOnKinematic = true;
                alreadyEntered = true;
            }
        }
    }

    IEnumerator LoadLevel()
    {
        Physics2D.IgnoreLayerCollision(9, 10, true);
        player.lastScene = levelManager.levelScene;
        SaveSystem.SavePlayerData(player, inventory, plAttack);
        SaveSystem.SaveSceneData(ScenesManager.scenesManager);
        anim.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneToLoad);
    }


}
