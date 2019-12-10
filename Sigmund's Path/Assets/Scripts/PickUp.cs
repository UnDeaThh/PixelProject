using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    private Inventory inventory;
    public GameObject itemButtonToUI;

    private void Awake()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Inventory>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            for (int i = 0; i < inventory.slotsForUI.Length; i++)
            {
                if(inventory.slotsForUI[i].isFull == false)
                {
                    inventory.slotsForUI[i].isFull = true;
                    Instantiate(itemButtonToUI, inventory.slotsForUI[i].slotGO.transform, false);
                    Destroy(gameObject);
                    break;
                }
            }
        }
    }
}
