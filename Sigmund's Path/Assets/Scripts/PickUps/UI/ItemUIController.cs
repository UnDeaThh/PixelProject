using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUIController : MonoBehaviour
{
    public ItemType itemType;

    public void PointerClick()
    {
        if(Inventory2.inventory.itemDescription != itemType)
        {
            Inventory2.inventory.itemDescription = itemType;
        }
        
    }
}
