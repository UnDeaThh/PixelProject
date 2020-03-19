using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColectableSword : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerAttack>().haveSword = true;
            collision.GetComponent<PlayerController2>().isOnKinematic = true;
            FindObjectOfType<DialogueTrigger>().isTalking = true;
            Destroy(gameObject);
        }
    }
}
