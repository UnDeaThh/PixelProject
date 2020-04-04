using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AddingPickUp : MonoBehaviour
{
    [SerializeField] Image pickUpImage;
    [SerializeField] TextMeshProUGUI textCantidad;
    private int printQuantity = 0;

    private bool showPanel;
    private Animator anim;

    void Start()
    {
        showPanel = false;
        anim = GetComponent<Animator>();
    }

    public void PrintPickUpInfo(Sprite image, int cantidad)
    {
        if (!showPanel)
        {
            printQuantity = cantidad;
            pickUpImage.sprite = image;
            textCantidad.SetText("+" + printQuantity.ToString());
            pickUpImage.SetNativeSize();
            StartCoroutine(TimeShowingInfo());
        }
        else
        {
            printQuantity += cantidad;
            textCantidad.SetText("+" + printQuantity.ToString());
        }

        
    }

    IEnumerator TimeShowingInfo()
    {
        anim.SetTrigger("appears");
        showPanel = true;
        yield return new WaitForSeconds(1f);
        showPanel = false;
        anim.SetTrigger("disappears");
    }
}
