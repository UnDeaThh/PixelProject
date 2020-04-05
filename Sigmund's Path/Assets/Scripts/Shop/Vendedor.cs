using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class Vendedor : MonoBehaviour
{
    private PlayerInputs inputs;

    public Canvas canvasVendedor;
    public GameObject pressEText;
    public GameObject UIShop;
	private PauseManager  pauseManager;
    private PlayerController2 player;

    public bool inShop = false;
    private bool playerClose = false;

    #region EventSystem Variables
    [SerializeField] EventSystem eventSystem;
    [SerializeField] GameObject fatherContent;
    private GameObject firstSelected;
    private bool ffShop = false;

    public GameObject FirstSelected { get => firstSelected; set => firstSelected = value; }
    #endregion
    private void Awake()
    {
        pressEText.SetActive(false);
        canvasVendedor.enabled = true;
        
    }
    private void Start()
    {
        inputs = new PlayerInputs();
        if(eventSystem == null)
        {
            eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        }
	    pauseManager = GameObject.FindGameObjectWithTag("PauseManager").GetComponent<PauseManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController2>();
        canvasVendedor.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (pauseManager.isPaused)
        {
            canvasVendedor.enabled = false;
        }
        else
            canvasVendedor.enabled = true;

        PressETextControll();
        ActiveShopUi();
        ExitShop();
    }

    void PressETextControll()
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
            inputs.Shop.Enable();
            UIShop.SetActive(true);
            pressEText.SetActive(false);
            if(player != null)
            {
                player.isOnKinematic = true;
            }
            if (ffShop)
            {
                FirstSelected = fatherContent.transform.GetChild(1).gameObject;
                eventSystem.SetSelectedGameObject(FirstSelected);
                ffShop = false;
            }
        }
        else
        {
            inputs.Shop.Disable();
            UIShop.SetActive(false);
            player.isOnKinematic = false;
        }
    }

    void ExitShop()
    {
        if (inShop)
        {
            if (inputs.Shop.Exit.triggered)
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
                if (player.inputs.Controls.Interact.triggered)
                {
                    inShop = true;
                    ffShop = true;
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

    public void ExitButton()
    {
        pauseManager.inShop = false;
        inShop = false;
    }
}
