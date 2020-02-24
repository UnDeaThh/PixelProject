using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vendedor : MonoBehaviour
{
    public Canvas canvasVendedor;
    public GameObject pressEText;
    public GameObject UIShop;
	private PauseManager  pauseManager;
    [SerializeField] PlayerController2 player;

    public bool inShop = false;
    private bool playerClose = false;
    private void Awake()
    {
        pressEText.SetActive(false);
        canvasVendedor.enabled = true;
        
    }
    private void Start()
    {
      //  ShopController.shopController.enabled = false;
	  pauseManager = GameObject.FindGameObjectWithTag("PauseManager").GetComponent<PauseManager>();
    }

    private void Update()
    {
        if (pauseManager.isPaused)
        {
            canvasVendedor.enabled = false;
        }
        else
            canvasVendedor.enabled = true;
        PressEControll();
        ActiveShopUi();
        ExitShop();
    }

    void PressEControll() 
    {
        if (playerClose)
        {
            pressEText.SetActive(true);
        }
        else
            pressEText.SetActive(false);
    }
    void ActiveShopUi()
    {
        if (inShop)
        {
            UIShop.SetActive(true);
            pressEText.SetActive(false);
            if(player != null)
            {
                player.isOnKinematic = true;
            }
        }
        else
        {
            UIShop.SetActive(false);
            player.isOnKinematic = false;
        }
    }

    void ExitShop()
    {
        if (inShop)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseManager.inShop = false;
                inShop = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerClose = true;
        }
    }



    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(!pauseManager.isPaused)
            {
                playerClose = true;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    inShop = true;
                    pauseManager.inShop = true;
                    
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerClose = false;
        }
    }
}
