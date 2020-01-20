using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopController : MonoBehaviour
{
    public static ShopController shopController;

    public bool alreadyFilled = false;

    public TextMeshProUGUI moneyText;

    public List<ShopItemInfo> itemsList = new List<ShopItemInfo>();
    public GameObject holderPrefab;
    public Transform content;
    private Vendedor vendedor;

    public int itemSelecteID = 0;
    public Image descriptionImage;
    [TextArea(3, 6)]
    public string[] descriptions;
    public TextMeshProUGUI itemDescriptionText;

    private void Awake()
    {
        vendedor = GameObject.FindObjectOfType<Vendedor>().GetComponent<Vendedor>();
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
        if (!alreadyFilled)
        {
            FillList();
        }
    }

    private void OnDisable()
    {
        itemSelecteID = 0;
    }

    private void Update()
    {
        UpdateUI();

    }

    void FillList()
    {
        if (vendedor.inShop)
        {
            for (int i = 0; i < itemsList.Count; i++)
            {
                GameObject holder = Instantiate(holderPrefab, content);
                ItemHolderButton holderScript = holder.GetComponent<ItemHolderButton>();

                holderScript.itemName.text = itemsList[i].itemName;
                holderScript.itemPrice.text = itemsList[i].itemPrice.ToString();
                holderScript.itemID = itemsList[i].itemID;
                holderScript.itemSprite.sprite = itemsList[i].itemImage;

                holderScript.buyButton.GetComponent<BuyButton>().itemID = itemsList[i].itemID;
            }
            alreadyFilled = true;
        }
    }

    void UpdateUI()
    {
        moneyText.SetText("" + Inventory2.inventory.actualMoney);
        if(itemSelecteID == 0)
        {
            descriptionImage.sprite = null;
            itemDescriptionText.SetText("");
            return;
        }
        else
        {
            for (int i = 0; i < itemsList.Count; i++)
            {
                if(itemSelecteID == itemsList[i].itemID)
                {
                    descriptionImage.sprite = itemsList[i].itemImage;
                    itemDescriptionText.SetText(descriptions[i]);
                }
            }
        }
    }
}
