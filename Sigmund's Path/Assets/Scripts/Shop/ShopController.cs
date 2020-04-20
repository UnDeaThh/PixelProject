using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class ShopController : MonoBehaviour
{
    public static ShopController shopController;
    private PlayerController2 player;

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
    [SerializeField] GameObject[] flechas;

    [SerializeField] EventSystem eventSystem;
    private bool ffShop = false;
    [SerializeField] GameObject firstSelected;
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
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController2>();
        if(eventSystem == null)
        {
            eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        }
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
                if(i != 3 && i != 4)
                {
                    GameObject holder = Instantiate(holderPrefab, content);
                    ItemHolderButton holderScript = holder.GetComponent<ItemHolderButton>();

                    holderScript.itemName.text = itemsList[i].itemName;
                    holderScript.itemPrice.text = itemsList[i].itemPrice.ToString();
                    holderScript.itemID = itemsList[i].itemID;
                    holderScript.itemSprite.sprite = itemsList[i].itemImage;

                    holderScript.buyButton.GetComponent<BuyButton>().itemID = itemsList[i].itemID;
                }
                else
                {
                    if(i == 3)
                    {
                        if (Inventory2.inventory.swordPasive)
                        {
                            //Esta vendido
                        }
                        else
                        {
                            GameObject holder = Instantiate(holderPrefab, content);
                            ItemHolderButton holderScript = holder.GetComponent<ItemHolderButton>();

                            holderScript.itemName.text = itemsList[i].itemName;
                            holderScript.itemPrice.text = itemsList[i].itemPrice.ToString();
                            holderScript.itemID = itemsList[i].itemID;
                            holderScript.itemSprite.sprite = itemsList[i].itemImage;

                            holderScript.buyButton.GetComponent<BuyButton>().itemID = itemsList[i].itemID;
                        }
                    }
                    else if(i == 4)
                    {
                        if (Inventory2.inventory.waterPasive)
                        {
                            
                        }
                        else
                        {
                            GameObject holder = Instantiate(holderPrefab, content);
                            ItemHolderButton holderScript = holder.GetComponent<ItemHolderButton>();

                            holderScript.itemName.text = itemsList[i].itemName;
                            holderScript.itemPrice.text = itemsList[i].itemPrice.ToString();
                            holderScript.itemID = itemsList[i].itemID;
                            holderScript.itemSprite.sprite = itemsList[i].itemImage;

                            holderScript.buyButton.GetComponent<BuyButton>().itemID = itemsList[i].itemID;
                        }
                    }
                }
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
            if (ffShop)
            {
                eventSystem.SetSelectedGameObject(firstSelected);
                ffShop = false;
            }

            if (itemSelecteID != 4 && itemSelecteID != 5)
            {
                for (int i = 0; i < flechas.Length; i++)
                {
                    flechas[i].SetActive(true);
                }
            }
            else
            {
                for (int i = 0; i < flechas.Length; i++)
                {
                    flechas[i].SetActive(false);
                }
            }

        }
        else
        {
            questionFadeBG.SetActive(false);
        }

        if(itemSelecteID != 4 && itemSelecteID != 5)
        {
            cantidadText.SetText(itemsToBuy.ToString());
        }
        else
        {
            cantidadText.SetText("");
        }
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
            if(itemSelecteID == 4 && Inventory2.inventory.swordPasive || itemSelecteID == 5 && Inventory2.inventory.waterPasive)
            {
                 buyButton.SetActive(false);
            }
            else
            {
                buyButton.SetActive(true);
            }

            descriptionImage.gameObject.SetActive(true);

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
        if(itemSelecteID == 1)
        {
            if(player.potions < 5)
            {
                buyingItem = true;
                ffShop = true;
            }
            else
            {
                buyingItem = false;
                Debug.Log("YA TIENES MUCHAS POTIS");
            }
        }
        else
        {
            buyingItem = true;
            ffShop = true;
        }
    }

    public void IncreaseItemsToBuy()
    {
        if(itemSelecteID != 4 && itemSelecteID != 5)
        {
            if(itemSelecteID == 1)
            {
                if(player.potions + itemsToBuy < 5)
                {
                    itemsToBuy++;
                }
                else
                {
                    Debug.Log("TE PASAS BRO");
                }
            }
            else
            {
                if(itemsToBuy < 99)
                {
                    itemsToBuy++;
                }
            }
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
        eventSystem.SetSelectedGameObject(vendedor.FirstSelected);
        buyingItem = false;
    }

    public void AcceptPurchase()
    {
        if (Inventory2.inventory.RequestMoney(moneyToSpend))
        {
            switch (itemSelecteID)
            {
                case 1:
                    player.potions += itemsToBuy;
                    break;
                case 2:
                    Inventory2.inventory.nBombs += itemsToBuy;
                    break;
                case 3:
                    Inventory2.inventory.nTP += itemsToBuy;
                    break;
                case 4:
                    Inventory2.inventory.swordPasive = true;
                    break;
            }
            Inventory2.inventory.LoseMoney(moneyToSpend);
            //Sonido de Compra
            itemsToBuy = 1;
            eventSystem.SetSelectedGameObject(vendedor.FirstSelected);
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
