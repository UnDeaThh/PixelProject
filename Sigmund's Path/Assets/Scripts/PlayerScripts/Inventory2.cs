using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum ItemType
{
    Potions, Bombs, TelePort, WaterPasive, SwordPasive, Nothing
}
public class Inventory2 : MonoBehaviour
{

    public static Inventory2 inventory;
    private PlayerController2 plController2;
	private PauseManager pauseManager;

    

    public int actualMoney;
    public int nBombs;
    public int nTP;
    private int maxBombs;
    private int maxTP;
    public int abilitiesUnlocked = 0;

    public bool waterPasive = false;
    public bool swordPasive = false;

    public GameObject waterPasiveItem;
    public GameObject swordPasiveItem;

    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI descriptions;
    [TextArea(3, 6)]
    public string[] descriptionText;

    public Image bigHeartImage;
    public Image abilitiesImage;
    public Sprite[] bigHeartSprite;
    public Sprite[] abilitiesSprite;

    [System.Serializable]
    public class Slot
    {
        public Transform slotPosition;
        public bool isFull;
        public TextMeshProUGUI textCounter;
        //public Image emptySlot;
        public GameObject filledSlot;
    }



    public ItemType itemDescription;

    public List<Slot> utilityItems;


    //-------------------
    

    //-------------------


    private void Awake()
    {
        if(inventory == null)
        {
            inventory = this;
        }
        else if(inventory != this)
        {
            Destroy(gameObject);
        }


        
        itemDescription = ItemType.Nothing;
    }

    private void Start()
    {
        plController2 = GetComponentInParent<PlayerController2>();
		pauseManager = GameObject.FindGameObjectWithTag("PauseManager").GetComponent<PauseManager>();
    }
    private void Update()
    {
        CheckIsFill();
        UpdateText();
        ImageDisplayed();
        DescriptionText();

        LimitObjectToZero();


    }

    void LimitObjectToZero()
    {
        if (actualMoney <= 0)
        {
            actualMoney = 0;
        }
        if (nBombs <= 0)
        {
            nBombs = 0;
        }
        if(nTP <= 0)
        {
            nTP = 0;
        }
    }

    void UpdateText()
    {
        moneyText.SetText("" + actualMoney);
        utilityItems[0].textCounter.SetText("" + plController2.potions);
        utilityItems[1].textCounter.SetText("" + nBombs);
        utilityItems[2].textCounter.SetText("" + nTP);
    }

    #region MoneyMethods
    public void WinMoney(int moneyObtained)
    {
        actualMoney += moneyObtained;
    }
    public void LoseMoney(int moneyLosed)
    {
        actualMoney -= moneyLosed;
    }

    public bool RequestMoney(int amount)
    {
        if (amount <= actualMoney)
        {
            return true;
        }
        else
            return false;
    }
    #endregion
    void CheckIsFill()
    {
        if(plController2.potions > 0)
        {
            utilityItems[0].isFull = true;
        }
        else
            utilityItems[0].isFull = false;
        if (nBombs > 0)
        {
            utilityItems[1].isFull = true;
        }
        else
            utilityItems[1].isFull = false;
        if (nTP > 0)
        {
            utilityItems[2].isFull = true;
        }
        else
            utilityItems[2].isFull = false;
    }

    void ImageDisplayed()
    {
        for (int i = 0; i < utilityItems.Count; i++)
        {
            if (!utilityItems[i].isFull)
            {
                utilityItems[i].filledSlot.SetActive(false);
            }
            else
            {
                utilityItems[i].filledSlot.SetActive(true);
            }
        }
        //BIG HEART
        switch (plController2.maxHealth)
        {
            case 5:
                bigHeartImage.sprite = bigHeartSprite[0];
                break;
            case 6:
                bigHeartImage.sprite = bigHeartSprite[1];
                break;
            case 7:
                bigHeartImage.sprite = bigHeartSprite[2];
                break;
            case 8:
                bigHeartImage.sprite = bigHeartSprite[3];
                break;
            case 9:
                bigHeartImage.sprite = bigHeartSprite[4];
                break;
            case 10:
                bigHeartImage.sprite = bigHeartSprite[5];
                break;
        }
        //WATER PASIVE
        if (waterPasive)
        {
            waterPasiveItem.SetActive(true);
        }
        else
            waterPasiveItem.SetActive(false);
        //DAMAGE PASIVE
        if (swordPasive)
        {
            swordPasiveItem.SetActive(true);
        }
        else
            swordPasiveItem.SetActive(false);
        //ABILITIES CIRCLE
        if(!plController2.dashUnlocked && !plController2.dobleJumpUnlocked && !plController2.wallJumpUnlocked)
        {
            abilitiesUnlocked = 0;
        }
        if (plController2.dashUnlocked)
        {
            abilitiesUnlocked = 1;
        }
        if (plController2.dobleJumpUnlocked)
        {
            abilitiesUnlocked = 2;
        }
        if (plController2.wallJumpUnlocked)
        {
            abilitiesUnlocked = 3;
        }

        if(abilitiesUnlocked <= 0)
        {
            abilitiesUnlocked = 0;
        }
        else if(abilitiesUnlocked >= 3)
        {
            abilitiesUnlocked = 3;
        }
        switch (abilitiesUnlocked)
        {
            case 0:
                abilitiesImage.sprite = abilitiesSprite[0];
                break;
            case 1:
                abilitiesImage.sprite = abilitiesSprite[1];
                break;
            case 2:
                abilitiesImage.sprite = abilitiesSprite[2];
                break;
            case 3:
                abilitiesImage.sprite = abilitiesSprite[3];
                break;
        }
        //
    }

    void DescriptionText()
    {
        switch (itemDescription)
        {
            case ItemType.Potions:
                descriptions.SetText(descriptionText[0]);
                break;
            case ItemType.Bombs:
                descriptions.SetText(descriptionText[1]);
                break;
            case ItemType.TelePort:
                descriptions.SetText(descriptionText[2]);
                break;
            case ItemType.WaterPasive:
                descriptions.SetText(descriptionText[3]);
                break;
            case ItemType.SwordPasive:
                descriptions.SetText(descriptionText[4]);
                break;
            default:
                descriptions.SetText("");
                break;
        }
        if (!pauseManager.isPaused)
        {
            itemDescription = ItemType.Nothing;
        }
    }
}
