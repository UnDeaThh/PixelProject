using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDoor : MonoBehaviour
{
    [HideInInspector] public GameManager GM;
    public PlayerController2 player;
    public Inventory2 inventory;
    private PlayerAttack plAttack;
    public LevelManager levelManager;

    public int sceneToLoad;
    private void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController2>();
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory2>();
        plAttack = player.gameObject.GetComponent<PlayerAttack>();
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            player.lastScene = levelManager.levelScene;
            SaveSystem.SavePlayerData(player, inventory, plAttack);
            ScenesManager.scenesManager.ChangeScene(sceneToLoad);
        }
    }


}
