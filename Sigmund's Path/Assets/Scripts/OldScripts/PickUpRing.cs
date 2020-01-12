using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpRing : PickUpBase
{
    private Inventory inventory;

    private void Awake()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Inventory>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (inventory != null)
            {
                for (int i = 0; i < inventory.slotsForGoods.Length; i++)
                {
                    if (!inventory.slotsForGoods[i].isFull)
                    {
                        inventory.slotsForGoods[i].isFull = true;
                        inventory.slotsForGoods[i].nItems += itemsToCollect; //sumamos la cantidad de items recogidos
                        inventory.slotsForGoods[i].itemType = itemType; //Le damos el string para que sepa que objeto tiene el slot

                        Instantiate(itemIcon, inventory.slotsForGoods[i].slotGO, false); //Dibujamos el item en el slot
                        Destroy(gameObject);
                        break;

                    }
                    else if (inventory.slotsForGoods[i].isFull && inventory.slotsForGoods[i].itemType == itemType)
                    {
                        inventory.slotsForGoods[i].nItems += itemsToCollect; // solo he de sumar los items recolectados
                        Destroy(gameObject);
                        break;
                    }
                }
            }
            else
                print("NoInventory");
        }
    }
}
