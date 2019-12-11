using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    public GameObject potionUI;
	public TextMeshProUGUI textPotionCounter;
    private PlayerController plContoller;
    [System.Serializable]
    public class Slot
    {
       // [HideInInspector]
        public bool isFull;
        public GameObject slotGO; //Es el gameObject, los slots en el inventario
    };
    //public bool[] isFull;
    //public GameObject[] slots;

    public Slot[] slotsForUI;
    public Slot[] slotsForPotions;

    private void Awake()
    {
        plContoller = GetComponentInParent<PlayerController>();
    }
    private void Update()
    {
        PotionSystemForUI();

    }

	void PotionSystemForUI()
	{
	/*
		for (int i = 0; i < slotsForPotions.Length; i++)
        {
            if(slotsForPotions[i].isFull == false && i < plContoller.potions)
            {
                slotsForPotions[i].isFull = true;
                Instantiate(potionUI, slotsForPotions[i].slotGO.transform, false);
            }
            if(i >= plContoller.potions && slotsForPotions[i].isFull == true)
            {
                slotsForPotions[i].isFull = false;
                foreach (RectTransform item in slotsForPotions[i].slotGO.transform)
                {
                    Destroy(item.gameObject);
                }
            }
        }
		*/

		if(plContoller.potions <= 0){
			potionUI.SetActive(false);
			textPotionCounter.SetText("");
		}

		else
		{
			potionUI.SetActive(true);
			textPotionCounter.SetText("x" + plContoller.potions);
		}
	}
}
