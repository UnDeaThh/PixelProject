using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ShopItemInfo 
{
    public string itemName;
    public int itemID;

    public Sprite itemImage;
    public int itemPrice;
    public bool isBought;
}
