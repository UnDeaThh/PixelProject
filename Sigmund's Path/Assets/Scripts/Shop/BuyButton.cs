using UnityEngine;

public class BuyButton : MonoBehaviour
{
    public int itemID;
    [SerializeField] AudioSource source;

    public void BuyItem()
    {
        source.Play();
        ShopController.shopController.itemSelecteID = itemID;  
    }
}
