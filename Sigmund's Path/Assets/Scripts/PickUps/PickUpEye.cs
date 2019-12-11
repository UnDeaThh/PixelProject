using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpEye : MonoBehaviour
{
    private Inventory inventory;
    public GameObject itemButtonToUI;
    private int nEyes = 2;

    private void Awake()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Inventory>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            for (int i = 0; i < inventory.slotsForGoods.Length; i++)
            {
                inventory.totalNumEyes += nEyes;
                if (inventory.slotsForGoods[i].isFull == false)
                {
                    inventory.slotsForGoods[i].isFull = true;
                    inventory.slotsForGoods[i].nEyes += nEyes;
                    Instantiate(itemButtonToUI, inventory.slotsForGoods[i].slotGO.transform, false);
                    inventory.slotsForGoods[i].textCounter.SetText("x" + inventory.slotsForGoods[i].nEyes);
                    Destroy(gameObject);
                    break;
                }
                else if(inventory.slotsForGoods[i].isFull == true && inventory.slotsForGoods[i].nEyes != 0)
                {
                    inventory.slotsForGoods[i].nEyes += nEyes;
                    inventory.slotsForGoods[i].textCounter.SetText("x" + inventory.slotsForGoods[i].nEyes);
                    Destroy(gameObject);
                    break;
                }
            }
        }
    }
}
