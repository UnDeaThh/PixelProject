using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUIController : MonoBehaviour
{
    private Inventory2 inventory;

    private void Awake()
    {
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory2>();
    }

    public void PointerClick()
    {
        if(inventory != null)
        {
            switch (gameObject.tag)
            {
                case "PotionUI":
                    inventory.itemDescription = Inventory2.ItemType.Potions;
                    break;
                case "BombUI":
                    inventory.itemDescription = Inventory2.ItemType.Bombs;
                    break;
                case "TelePortUI":
                    inventory.itemDescription = Inventory2.ItemType.TelePort;
                    break;
            }
        }
    }
}
