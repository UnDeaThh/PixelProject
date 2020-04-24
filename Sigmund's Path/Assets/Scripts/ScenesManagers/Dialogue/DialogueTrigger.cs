using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] NpcDialogue npc;
    private PlayerController2 player;
    private Collider2D col;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController2>();
        col = GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (npc)
            {
                npc.TriggerDialogue();
            }
            else
            {
                FindObjectOfType<NpcDialogue>().TriggerDialogue();
            }
            col.enabled = false;
        }
    }
}
