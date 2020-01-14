using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopController : MonoBehaviour
{
    public static ShopController shopController;

    public TextMeshProUGUI moneyText;

    public List<ItemInfo> itemsList = new List<ItemInfo>();
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
        Debug.Log(this.gameObject.name);

        if (shopController.enabled)
        {
            Debug.Log("shop");
        }

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
            print("q");
            for (int i = 0; i < itemsList.Count; i++)
            {
                Instantiate(holderPrefab, content);
                print("q");
            }
        }
    }

    void UpdateUI()
    {
        moneyText.SetText("" + Inventory2.inventory.actualMoney);
    }
}
