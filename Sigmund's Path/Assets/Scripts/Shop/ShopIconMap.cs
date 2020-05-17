using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopIconMap : MonoBehaviour
{
    [SerializeField] int iconNumber;

    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();

        if (ScenesManager.scenesManager.ShopUnlocked[iconNumber])
        {
            image.enabled = true;
        }
        else
        {
            image.enabled = false;
        }
    }

    private void Update()
    {
        if (!image.enabled)
        {
            if (ScenesManager.scenesManager.ShopUnlocked[iconNumber])
            {
                image.enabled = true;
            }
        }
    }
}
