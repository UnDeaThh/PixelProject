using UnityEngine;

public class BuyButton : MonoBehaviour
{
    public int itemID;

    public void BuyItem()
    {
         ShopController.shopController.itemSelecteID = itemID;  
    }
}
