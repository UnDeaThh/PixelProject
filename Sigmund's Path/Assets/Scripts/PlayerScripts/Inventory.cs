using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Inventory : MonoBehaviour
{
    public GameObject potionUI;
    private PlayerController plContoller;
    private PauseManager pauseManager;
    public enum ItemType
    {
        Potions, Ring, Eye, Nothing
    }
    public ItemType itemDescription;
    public bool oneClick = false;
    public GameObject[] descriptionPanels; //0 = Potis, 1 = ring, 2 = Eye

    [HideInInspector] public int totalNumEyes;
    [HideInInspector] public int totalNumRings;

    [System.Serializable]
    public class Slot
    {

        public bool isFull;
        public int nItems;
        public string itemType;
        public int slotNumber;
        public Transform slotGO; //Es el gameObject, los slots en el inventario
        public TextMeshProUGUI textCounter; //Texto para saber cuantos objetos del mismo item hay estaqueados en cada slot
        
    };


    public Slot[] slotsForUtility;
    public Slot[] slotsForGoods;

    private void Awake()
    {
        SetSlotNumber();
        plContoller = GetComponentInParent<PlayerController>();
        pauseManager = GameObject.FindGameObjectWithTag("PauseManager").GetComponent<PauseManager>();
        for (int i = 0; i < slotsForGoods.Length; i++)
        {
            slotsForGoods[i].textCounter.SetText("");
        }
        for (int i = 1; i < slotsForUtility.Length; i++)
        {
            slotsForUtility[i].textCounter.SetText("");
        }

    }
    /*
   void SetEventTrigger()
   {

       EventTrigger.Entry entry = new EventTrigger.Entry(); // No entiendo que hace esto
       entry.eventID = EventTriggerType.PointerClick;  //Esto decide que EventType se añade al EventTrigger
       entry.callback.AddListener((eventData) => { ButtonDisplayDescription(); }); //Añade la funcion que se ejecutara con el evento
       for (int i = 0; i < slotsForUtility.Length; i++)
       {
           if(slotsForUtility[i].eventTrigger != null)
           {
               slotsForUtility[i].eventTrigger.triggers.Add(entry); // Añade todo lo anterior al EventTrigger y ready para usarlo
           }
       }
       
}
*/
    void SetSlotNumber()
    {
        for (int i = 0; i < slotsForUtility.Length; i++)
        {
            slotsForUtility[i].slotNumber = i;
        }
        for (int i = 0; i < slotsForGoods.Length; i++)
        {
            slotsForGoods[i].slotNumber = i;
        }
    }

    private void Update()
    {
        PotionSystemForUI();
        UpdateText();
        ShowDescriptionText();
    }

	void PotionSystemForUI()
	{
        slotsForUtility[0].isFull = true;
        slotsForUtility[0].nItems = plContoller.potions;
        slotsForUtility[0].itemType = "Potions";
        if (slotsForUtility[0].nItems <= 0)
        {
            potionUI.SetActive(false);
        }
        else
            potionUI.SetActive(true);
    }

    void UpdateText()
    {
        for (int i = 0; i < slotsForGoods.Length; i++)
        { 
            if(slotsForGoods[i].nItems <= 0)
            {
                slotsForGoods[i].textCounter.SetText("");
            }
            else
            {
                slotsForGoods[i].textCounter.SetText("x" + slotsForGoods[i].nItems);
            }
        }
        for (int i = 0; i < slotsForUtility.Length; i++)
        {
            if(slotsForUtility[i].nItems <= 0)
            {
                slotsForUtility[i].textCounter.SetText("");
            }
            else
            {
                slotsForUtility[i].textCounter.SetText("x" + slotsForUtility[i].nItems);
            }
        }
    }

    //Called when click on any item on inventory
    private void ShowDescriptionText()
    {
        if(pauseManager.isOnInventory && oneClick)
        {
            switch (itemDescription)
            {
                case ItemType.Potions:
                    descriptionPanels[0].SetActive(true);
                    descriptionPanels[1].SetActive(false);
                    descriptionPanels[2].SetActive(false);
                    break;
                case ItemType.Ring:
                    descriptionPanels[0].SetActive(false);
                    descriptionPanels[1].SetActive(true);
                    descriptionPanels[2].SetActive(false);
                    break;
                case ItemType.Eye:
                    descriptionPanels[0].SetActive(false);
                    descriptionPanels[1].SetActive(false);
                    descriptionPanels[2].SetActive(true);
                    break;
                default:
                    break;
            }
            oneClick = false;
        }
    }

    //This Method is on PauseManager
    public void HideDescriptionText()
    {
        itemDescription = ItemType.Nothing;
        for (int i = 0; i < descriptionPanels.Length; i++)
        {
            descriptionPanels[i].SetActive(false);
        }
    }
}
