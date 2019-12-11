using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpRing : MonoBehaviour
{
    private Inventory inventory;
    public GameObject itemButtonToUI;
    private int nRings = 1;

    private void Awake()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Inventory>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            for (int i = 0; i < inventory.slotsForGoods.Length; i++)
            {
                inventory.totalNumRings += nRings;
                if (inventory.slotsForGoods[i].isFull == false)
                {
                    inventory.slotsForGoods[i].isFull = true;
                    inventory.slotsForGoods[i].nRings += nRings;
                    Instantiate(itemButtonToUI, inventory.slotsForGoods[i].slotGO.transform, false);
                    inventory.slotsForGoods[i].textCounter.SetText("x" + inventory.slotsForGoods[i].nRings);
                    Destroy(gameObject);
                    break;
                }
                else if (inventory.slotsForGoods[i].isFull == true && inventory.slotsForGoods[i].nRings != 0)
                {
                    inventory.slotsForGoods[i].nRings += nRings;
                    inventory.slotsForGoods[i].textCounter.SetText("x" + inventory.slotsForGoods[i].nRings);
                    Destroy(gameObject);
                    break;
                }
            }
        }
    }
}
