using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDoor : MonoBehaviour
{
    [HideInInspector] public GameManager GM;
    public PlayerController2 player;
    public Inventory2 inventory;
    public int sceneToLoad;
    public int actualScene;
    private void Start()
    {
        actualScene += 2;
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController2>();
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory2>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            player.lastScene = actualScene;
            SaveSystem.SavePlayerData(player, inventory);
            ScenesManager.scenesManager.ChangeScene(sceneToLoad);
        }
    }


}
