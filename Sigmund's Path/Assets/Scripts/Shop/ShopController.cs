using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopController : MonoBehaviour
{
    public static ShopController shopController;
    private PlayerController2 plController2;

    public bool alreadyFilled = false;
    private bool buyingItem;
    private bool itemBought = false;

    public int itemSelecteID = 0;
    private int itemsToBuy = 1;
    private int moneyToSpend; 

    

    public List<ShopItemInfo> itemsList = new List<ShopItemInfo>();
    
    public Transform content;
    private Vendedor vendedor;

    public Image descriptionImage;

    [TextArea(3, 6)]
    public string[] descriptions;
    public string[] questions;

    public TextMeshProUGUI itemDescriptionText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI cantidadText;

    public GameObject buyButton;
    public GameObject holderPrefab;
    public GameObject questionFadeBG;

    private void Awake()
    {
        questionFadeBG.SetActive(false);
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
    private void Start()
    {
        plController2 = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController2>();
    }

    private void OnEnable()
    {
        if (!alreadyFilled)
        {
            buyingItem = false;
            itemsToBuy = 1;
            FillList();
        }
    }

    private void OnDisable()
    {
        itemSelecteID = 0;
        buyingItem = false;
    }

    private void Update()
    {
        UpdateUI();
        ConfirmationQuestion();
        MoneyToSpend();
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

    void MoneyToSpend()
    {
        for (int i = 0; i < itemsList.Count; i++)
        {
            if(itemSelecteID == itemsList[i].itemID)
            {
                moneyToSpend = itemsList[i].itemPrice * itemsToBuy;
            }
        }
    }


    void ConfirmationQuestion()
    {
        if (buyingItem)
        {
            questionFadeBG.SetActive(true);
            for (int i = 0; i < itemsList.Count; i++)
            {
                if(itemSelecteID == itemsList[i].itemID)
                {
                    questionText.SetText(questions[i]);
                }
            }
        }
        else
        {
            questionFadeBG.SetActive(false);
        }

        cantidadText.SetText(itemsToBuy.ToString());
    }


    void UpdateUI()
    {
        moneyText.SetText("" + Inventory2.inventory.actualMoney);
        if(itemSelecteID == 0)
        {
            descriptionImage.gameObject.SetActive(false);
            buyButton.SetActive(false);
            itemDescriptionText.SetText("");
            return;
        }
        else
        {
            descriptionImage.gameObject.SetActive(true);
            buyButton.SetActive(true);

            for (int i = 0; i < itemsList.Count; i++)
            {  
                if (itemSelecteID == itemsList[i].itemID)
                {
                    descriptionImage.sprite = itemsList[i].itemImage;
                    descriptionImage.SetNativeSize();
                    itemDescriptionText.SetText(descriptions[i]);
                }
            }
        }
    }

    #region BuyingButtonsMethods
    public void ClickOnBuy()
    {
        buyingItem = true;
    }

    public void IncreaseItemsToBuy()
    {
        if(itemsToBuy < 99)
        {
            itemsToBuy++;
        }
    }

    public void DecreaseItemsToBuy()
    {
        if(itemsToBuy > 1)
        {
            itemsToBuy--;
        }
    }

    public void CancelPurchase()
    {
        itemsToBuy = 1;
        buyingItem = false;
    }

    public void AcceptPurchase()
    {
        if (Inventory2.inventory.RequestMoney(moneyToSpend))
        {
            switch (itemSelecteID)
            {
                case 1:
                    plController2.potions += itemsToBuy;
                    break;
                case 2:
                    Inventory2.inventory.nBombs += itemsToBuy;
                    break;
                case 3:
                    Inventory2.inventory.nTP += itemsToBuy;
                    break;
            }
            Inventory2.inventory.LoseMoney(moneyToSpend);
            //Sonido de Compra
            itemsToBuy = 1;
            buyingItem = false;
        }
        else
        {
            Debug.Log("PUTO POBRE");
            //Sonido de que falta dinero
        }

    }

    #endregion
}
