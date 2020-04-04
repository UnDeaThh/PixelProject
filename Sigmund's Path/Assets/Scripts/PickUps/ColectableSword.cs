using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColectableSword : MonoBehaviour
{
    private void Start()
    {
        if (ScenesManager.scenesManager.SwordPicked)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObject player = collision.gameObject;
            collision.GetComponent<PlayerAttack>().haveSword = true;
            collision.GetComponent<PlayerController2>().isOnKinematic = true;
            FindObjectOfType<NpcDialogue>().isTalking = true;
            ScenesManager.scenesManager.SwordPicked = true;
            SaveSystem.SavePlayerData(player.GetComponent<PlayerController2>(), player.GetComponentInChildren<Inventory2>(), player.GetComponent<PlayerAttack>());
            Destroy(gameObject);
        }
    }
}
