using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory2 : MonoBehaviour
{
    public static Inventory2 inventory;

    public int actualMoney;
    public int nBombs;
    private int maxBombs;
    public int nTP;
    private int maxTP;


    private PlayerController plController;
    private PauseManager PM;

    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI descriptions;
    [TextArea(3, 6)]
    public string[] descriptionText;

    [System.Serializable]
    public class Slot
    {
        public Transform slotPosition;
        public bool isFull;
        public int nItems;
        public TextMeshProUGUI textCounter;
        public Image emptySlot;
        public GameObject filledSlot;
    }

    public enum ItemType
    {
        Potions, Bombs, TelePort, Nothing
    }

    public ItemType itemDescription;

    //public Slot[] utilityItems;
    public List<Slot> utilityItems;

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


        plController = GetComponentInParent<PlayerController>();
        itemDescription = ItemType.Nothing;
    }
    private void Update()
    {
        CheckIsFill();
        UpdateText();
        ImageDisplayed();
        DescriptionText();
    }

    void UpdateText()
    {
        moneyText.SetText("" + actualMoney);
        utilityItems[0].textCounter.SetText("" + plController.potions);
        utilityItems[1].textCounter.SetText("" + nBombs);
        utilityItems[2].textCounter.SetText("" + nTP);
    }

    public void WinMoney(int moneyObtained)
    {
        actualMoney += moneyObtained;
    }
    public void LoseMoney(int moneyLosed)
    {
        actualMoney -= moneyLosed;
    }

    void CheckIsFill()
    {
        if(plController.potions > 0)
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
            case ItemType.Nothing:
                descriptions.SetText("");
                break;
        }
        if (!PauseManager.pauseManager.isPaused)
        {
            itemDescription = ItemType.Nothing;
        }
    }
}
