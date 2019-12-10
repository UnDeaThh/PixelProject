using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    private Inventory inventory;
    public GameObject itemButtonToUI;

    private void Awake()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            for (int i = 0; i < inventory.slotssss.Length; i++)
            {
                if(inventory.slotssss[i].isFull == false)
                {
                    inventory.slotssss[i].isFull = true;
                    Instantiate(itemButtonToUI, inventory.slotssss[i].slotGO.transform, false);
                    Destroy(gameObject);
                    break;
                }
            }
        }
    }
}
