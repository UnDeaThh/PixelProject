using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUIController : MonoBehaviour
{
    private Inventory inventory;

    private void Awake()
    {
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
    }

    public void PointerClick()
    {
        if(inventory != null)
        {
            switch (gameObject.tag)
            {
                case "PotionUI":
                    inventory.itemDescription = Inventory.ItemType.Potions;
                    break;
                case "RingUI":
                    inventory.itemDescription = Inventory.ItemType.Ring;
                    break;
                case "EyeUI":
                    inventory.itemDescription = Inventory.ItemType.Eye;
                    break;

            }
            inventory.oneClick = true;
        }
    }

    public void PointerEnter()
    {
        Image sprite = GetComponent<Image>();
        sprite.color = Color.grey;
    }

    public void PointerExit()
    {
        Image sprite = GetComponent<Image>();
        sprite.color = Color.white;
    }

}
