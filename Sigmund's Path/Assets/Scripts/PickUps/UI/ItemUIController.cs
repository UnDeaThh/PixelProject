using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUIController : MonoBehaviour
{
    private Inventory2 inventory;
    public ItemType itemType;

    private void Awake()
    {
        inventory = Inventory2.inventory;
    }

    public void PointerClick()
    {
        if(inventory != null)
        {
            switch (itemType)
            {
                case ItemType.Potions:
                    Debug.Log("poti");
                    inventory.itemDescription = ItemType.Potions;
                    break;
                case ItemType.Bombs:
                    Debug.Log("bom");
                    inventory.itemDescription = ItemType.Bombs;
                    break;
                case ItemType.TelePort:
                    Debug.Log("tp");
                    inventory.itemDescription = ItemType.TelePort;
                    break;
            }
        }
    }
}
