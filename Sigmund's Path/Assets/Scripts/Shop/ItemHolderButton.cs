﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemHolderButton : MonoBehaviour
{
    public int itemID;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemPrice;
    public Image itemSprite;
    public GameObject buyButton;

    void Update()
    {
        itemSprite.SetNativeSize();
    }

}
