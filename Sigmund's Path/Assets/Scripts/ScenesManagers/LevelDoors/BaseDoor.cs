using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDoor : MonoBehaviour
{
    public PlayerController2 player;
    public Inventory2 inventory;
    private PlayerAttack plAttack;
    public LevelManager levelManager;
    private Animator anim;
    private bool alreadyEntered;

    public int sceneToLoad;
    private void Start()
    {
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
                alreadyEntered = true;
            }
        }
    }

    IEnumerator LoadLevel()
    {
        player.lastScene = levelManager.levelScene;
        SaveSystem.SavePlayerData(player, inventory, plAttack);
        SaveSystem.SaveSceneData(ScenesManager.scenesManager);
        anim.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1f);
        ScenesManager.scenesManager.ChangeScene(sceneToLoad);
    }


}
