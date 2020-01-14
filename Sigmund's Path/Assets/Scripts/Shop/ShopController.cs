using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopController : MonoBehaviour
{
    public static ShopController shopController;

    public TextMeshProUGUI moneyText;

    public List<ShopItemInfo> itemsList = new List<ShopItemInfo>();
    public GameObject holderPrefab;
    public Transform content;

    private void Awake()
    {
        if(shopController == null)
        {
            shopController = this;
        }
        else if(shopController != this)
        {
            Destroy(gameObject);
        }      
    }

    private void OnEnable()
    {
        FillList();
    }

    private void Update()
    {
        UpdateUI();

    }

    void FillList()
    {
        if(Vendedor.seller.inShop )
        {
            for (int i = 0; i < itemsList.Count; i++)
            {
                GameObject holder = Instantiate(holderPrefab, content);
                ItemHolderButton holderScript = holder.GetComponent<ItemHolderButton>();

                holderScript.itemName.text = itemsList[i].itemName;
                holderScript.itemPrice.text = itemsList[i].itemPrice.ToString();
                holderScript.itemID = itemsList[i].itemID;
                holderScript.itemSprite = itemsList[i].itemImage;
            }
        }
    }

    void UpdateUI()
    {
        moneyText.SetText("" + Inventory2.inventory.actualMoney);
    }
}
