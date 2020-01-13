using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    public List<ItemHolderButton> holderButtons = new List<ItemHolderButton>();

    public GameObject holderPrefab;
    public static ShopController shopController;

    public Transform content;

    private void Awake()
    {
        shopController = this;
        FillList();
    }

    void FillList()
    {
        for (int i = 0; i < holderButtons.Count; i++)
        {
            GameObject holder = Instantiate(holderPrefab, content);
        }
    }
}
