using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

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
    private void Start()
    {
        mat = GetComponent<SpriteRenderer>().material;
        col = GetComponent<Collider2D>();
        sound = GetComponent<AudioSource>();

        alreadyPicked = ScenesManager.scenesManager.heartsPickUp[numberOfHeart];
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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!alreadyPicked)
        {
            if (other.CompareTag("Player"))
            {
                player = other.GetComponent<PlayerController2>();
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
                    col.enabled = false;
                    sound.Play();
                    ScenesManager.scenesManager.heartsPickUp.SetValue(true, numberOfHeart);
                }
            }
        }
    }
}
