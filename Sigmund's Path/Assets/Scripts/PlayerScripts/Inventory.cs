using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    public GameObject potionUI;
    private PlayerController plContoller;

    [HideInInspector] public int totalNumEyes;
    [HideInInspector] public int totalNumRings;

    [System.Serializable]
    public class Slot
    {

        public bool isFull;
        public GameObject slotGO; //Es el gameObject, los slots en el inventario
        public TextMeshProUGUI textCounter; //Texto para saber cuantos objetos del mismo item hay estaqueados en cada slot
        [HideInInspector] public int nEyes;
        [HideInInspector] public int nRings;

    };


    public Slot[] slotsForGoods;
    public Slot[] slotsForUtility;

    private void Awake()
    {
        plContoller = GetComponentInParent<PlayerController>();
        for (int i = 0; i < slotsForGoods.Length; i++)
        {
            slotsForGoods[i].textCounter.SetText("");
        }
        for (int i = 1; i < slotsForUtility.Length; i++)
        {
            slotsForUtility[i].textCounter.SetText("");
        }

    }
    private void Update()
    {
        PotionSystemForUI();

    }

	void PotionSystemForUI()
	{
        #region PotisSlot

        if (plContoller.potions <= 0){
			potionUI.SetActive(false);
			slotsForUtility[0].textCounter.SetText("");
		}

		else
		{
			potionUI.SetActive(true);
			slotsForUtility[0].textCounter.SetText("x" + plContoller.potions);
		}

        #endregion
    }

    void UtilityItemStack()
    {
    
        for (int i = 1; i < slotsForUtility.Length; i++)
        {
            if(slotsForUtility[i].isFull == false)
            {
                slotsForUtility[i].isFull = true;
                Instantiate(potionUI, slotsForUtility[i].slotGO.transform, false);
                slotsForUtility[i].textCounter.SetText("x");
            }
            if(i >= plContoller.potions && slotsForUtility[i].isFull == true)
            {
                slotsForUtility[i].isFull = false;
                foreach (RectTransform item in slotsForUtility[i].slotGO.transform)
                {
                    Destroy(item.gameObject);
                }
            }
        }
    }

    //HACE FALTA QUE ACTUALICE TODO EL RATO?, SOLO CUANDO CONSIGO UN ITEM Y CUANDO LO VENDO,NO?
    void UpdateText()
    {

    }
}
