﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class Vendedor : MonoBehaviour
{
    [SerializeField] int vendedorNumber;
    private bool alreadyTalk = false;
    [SerializeField] NpcDialogue npcDialogue;
    private PlayerInputs inputs;

    public Canvas canvasVendedor;
    public GameObject pressText;

    [SerializeField] Image buttonImage;
    [SerializeField] Sprite[] buttonDeviceSprite;


    public GameObject UIShop;
	private PauseManager  pauseManager;
    private PlayerController2 player;

    public bool inShop = false;
    private bool playerClose = false;

    [SerializeField] GameObject talkIcon;

    //Audio
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip exitClipSound;
    [SerializeField] AudioClip enterSound;

    #region EventSystem Variables
    [SerializeField] EventSystem eventSystem;
    [SerializeField] GameObject fatherContent;
    private GameObject firstSelected;
    private bool ffShop = false;

    public GameObject FirstSelected { get => firstSelected; set => firstSelected = value; }
    public bool AlreadyTalk { get => alreadyTalk; set => alreadyTalk = value; }
    #endregion
    private void Awake()
    {
        pressText.SetActive(false);
        canvasVendedor.gameObject.SetActive(true);
        canvasVendedor.enabled = true;
        
    }
    private void Start()
    {
        alreadyTalk = ScenesManager.scenesManager.ShopUnlocked[vendedorNumber];

        inputs = new PlayerInputs();
        if(eventSystem == null)
        {
            eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        }
	    pauseManager = GameObject.FindGameObjectWithTag("PauseManager").GetComponent<PauseManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController2>();
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
            talkIcon.SetActive(true);
            buttonImage.SetNativeSize();
            pressText.SetActive(true);
            if(player.Gamepad != null)
            {
                buttonImage.sprite = buttonDeviceSprite[1];
            }
            else
            {
                buttonImage.sprite = buttonDeviceSprite[0];
            }
        }
        else
        {
            talkIcon.SetActive(false);
            pressText.SetActive(false);
        }
    }
    void ActiveShopUi()
    {
        if (inShop)
        {
            inputs.Shop.Enable();
            UIShop.SetActive(true);
            pressText.SetActive(false);
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
           
        }
    }

    void ExitShop()
    {
        if (inShop)
        {
            if (inputs.Shop.Exit.triggered)
            {
                player.isOnKinematic = false;
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
                    if (alreadyTalk)
                    {
                        EnterShop();
                    }
                    else
                    {
                        npcDialogue.TriggerDialogue();
                        ScenesManager.scenesManager.ShopUnlocked[vendedorNumber] = true;
                        SaveSystem.SaveSceneData(ScenesManager.scenesManager);
                    }
                }
            }
        }
    }

    public void EnterShop()
    {
        inShop = true;
        source.clip = enterSound;
        source.Play();
        ffShop = true;
        pauseManager.inShop = true;
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
        player.isOnKinematic = false;
        source.clip = exitClipSound;
        source.Play();
        inShop = false;
    }
}
