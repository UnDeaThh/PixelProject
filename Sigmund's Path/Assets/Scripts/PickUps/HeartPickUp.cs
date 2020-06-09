using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;
using UnityEngine.UI;

public class HeartPickUp : MonoBehaviour
{
    private PlayerController2 player;
    private bool disolve;
    private float fade = 1;
    private Material mat;
    public Light2D redLight;
    private Collider2D col;
    private AudioSource sound;
    public int numberOfHeart = 0;
    private bool alreadyPicked;
    [SerializeField] Image image;
    [SerializeField] Sprite[] deviceButtons;
    [SerializeField] GameObject canvasObject;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController2>();
        mat = GetComponent<SpriteRenderer>().material;
        col = GetComponent<Collider2D>();
        sound = GetComponent<AudioSource>();

        canvasObject.SetActive(false);
        alreadyPicked = ScenesManager.scenesManager.HeartsPickUp[numberOfHeart];
        if (alreadyPicked)
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        if (disolve)
        {
            fade -= Time.deltaTime;
            redLight.intensity -= Time.deltaTime;
            if (fade <= 0)
            {
                fade = 0;
                player.maxHealth++;
                player.health = player.maxHealth;
                SaveSystem.SaveSceneData(ScenesManager.scenesManager);
                SaveSystem.SavePlayerData(player, player.gameObject.GetComponentInChildren<Inventory2>(), player.gameObject.GetComponent<PlayerAttack>());
                Debug.Log("DatosGuardados");
                disolve = false;
            }
            if (redLight.intensity <= 0)
            {
                redLight.intensity = 0f;
            }
            mat.SetFloat("_Fade", fade);
        }

        if (!disolve && fade <= 0f && alreadyPicked && !sound.isPlaying)
        {
            Destroy(gameObject);
        }

        CanvasController();
    }

    void CanvasController()
    {
        if(player.Gamepad != null)
        {
            image.sprite = deviceButtons[1];
        }
        else
        {
            image.sprite = deviceButtons[0];
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!alreadyPicked)
        {
            if (other.CompareTag("Player"))
            {
                canvasObject.SetActive(true);
            }
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!alreadyPicked)
        {
            if (other.CompareTag("Player"))
            {
                if (player.inputs.Controls.Interact.triggered)
                {
                    disolve = true;
                    alreadyPicked = true;
                    canvasObject.SetActive(false);
                    col.enabled = false;
                    sound.Play();
                    ScenesManager.scenesManager.HeartsPickUp.SetValue(true, numberOfHeart);
                    SaveSystem.SavePlayerData(player, player.gameObject.GetComponentInChildren<Inventory2>(), player.gameObject.GetComponent<PlayerAttack>());
                    SaveSystem.SaveSceneData(ScenesManager.scenesManager);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canvasObject.SetActive(false);
        }
    }
}
