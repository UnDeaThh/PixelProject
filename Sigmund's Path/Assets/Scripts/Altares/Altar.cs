using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Altar : MonoBehaviour
{
    private enum AltarType
    {
        Dash, DoubleJump, WallJump
    }
    private SpriteRenderer sr;
    [SerializeField] AltarType altarType;
    private bool playerClose;
    [SerializeField] GameObject pressEText;
    [SerializeField] TextMeshProUGUI abilitiesInstructions;
    [SerializeField] string[] textos = new string[3];
    private PlayerController2 player;
    [SerializeField] float kinematicDuration;
    [SerializeField] GameObject canvasObject;
    [SerializeField] Image image;
    [SerializeField] Sprite[] deviceButtonSprite;

    [SerializeField] Image imageInstructions;
    [SerializeField] Sprite[] dashDeviceSprite;
    [SerializeField] Sprite[] jumpDeviceSprite;

    [SerializeField] Material[] materialPiedra;

    [SerializeField] GameObject particulaSayan;
    [SerializeField] Material[] materialesParticula;

    private bool clicked = false;
    private bool onKinematic = false;
    [SerializeField] AudioSource source;
    [SerializeField] ParticleSystem ps;
    [SerializeField] int nParticlesEnter;
    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        canvasObject.SetActive(true);
        pressEText.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController2>();
        abilitiesInstructions.SetText("");
        imageInstructions.enabled = false;

        switch (altarType)
        {
            case AltarType.Dash:
                sr.material = materialPiedra[0];
                if (player.dashUnlocked)
                {
                    Destroy(ps);
                }
                break;
            case AltarType.DoubleJump:
                sr.material = materialPiedra[1];
                if (player.dobleJumpUnlocked)
                {
                    Destroy(ps);
                }
                break;
            case AltarType.WallJump:
                sr.material = materialPiedra[2];
                if (player.wallJumpUnlocked)
                {
                    Destroy(ps);
                }
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        if (onKinematic)
        {
            player.rb.velocity = Vector2.zero;
        }

        if (playerClose)
        {
            if(image.transform.localScale != Vector3.one)
            {
                image.transform.localScale = Vector3.one;
            }

            image.SetNativeSize();
            if(player.Gamepad != null) // Esto es el button al que le has de dar para abrir el altar
            {
                image.sprite = deviceButtonSprite[1];
            }
            else
            {
                image.sprite = deviceButtonSprite[0];
            }

            if (clicked)
            {
                switch (altarType)
                {
                    case AltarType.Dash:
                        imageInstructions.transform.localScale = Vector3.one;
                        imageInstructions.SetNativeSize();
                        if (player.Gamepad != null)
                        {
                            imageInstructions.sprite = dashDeviceSprite[1];
                        }
                        else
                        {
                            imageInstructions.sprite = dashDeviceSprite[0];
                        }
                        break;
                    case AltarType.DoubleJump:
                        imageInstructions.transform.localScale = Vector3.one;
                        imageInstructions.SetNativeSize();
                        if (player.Gamepad != null)
                        {
                            imageInstructions.sprite = jumpDeviceSprite[1];
                        }
                        else
                        {
                            imageInstructions.sprite = jumpDeviceSprite[0];
                        }
                        break;
                    case AltarType.WallJump:
                        imageInstructions.transform.localScale = Vector3.one;
                        imageInstructions.SetNativeSize();
                        if (player.Gamepad != null)
                        {
                            imageInstructions.sprite = jumpDeviceSprite[1];
                        }
                        else
                        {
                            imageInstructions.sprite = jumpDeviceSprite[0];
                        }
                        break;
                    default:
                        break;
                }
            }


            if (player.inputs.Controls.Interact.triggered)
            {
                switch (altarType)
                {
                    case AltarType.Dash:
                        if (!player.dashUnlocked)
                        {
                            pressEText.SetActive(false);
                            StartCoroutine(UnlockAbilitie());
                        }
                        else
                        {
                            clicked = true;
                            pressEText.SetActive(false);
                            abilitiesInstructions.SetText(textos[0]);

                            imageInstructions.enabled = true;
 
                        }
                        break;

                    case AltarType.DoubleJump:
                        if (!player.dobleJumpUnlocked)
                        {
                            pressEText.SetActive(false);
                            StartCoroutine(UnlockAbilitie());
                        }
                        else
                        {
                            clicked = true;
                            pressEText.SetActive(false);
                            abilitiesInstructions.SetText(textos[1]);
                            imageInstructions.enabled = true;
                        }
                        break;

                    case AltarType.WallJump:
                        if (!player.wallJumpUnlocked)
                        {
                            pressEText.SetActive(false);
                            StartCoroutine(UnlockAbilitie());
                        }
                        else
                        {
                            pressEText.SetActive(false);
                            abilitiesInstructions.SetText(textos[2]);
                        }
                        break;
                }

            }
        }
        else
        {
            pressEText.SetActive(false);
            abilitiesInstructions.SetText("");
            imageInstructions.enabled = false;
            clicked = false;
        }
    }

    IEnumerator UnlockAbilitie()
    {
        player.isOnKinematic = true;
        ParticleSystem.EmissionModule emidMod = ps.emission;
        emidMod.rateOverTime = nParticlesEnter;
        player.heedArrows = false;
        onKinematic = true;
        source.Play();
        GameObject particle = Instantiate(particulaSayan, player.transform.position, Quaternion.identity);
        switch (altarType)
        {
            case AltarType.Dash:
                particle.GetComponent<SpriteRenderer>().material = materialesParticula[0];
                break;
            case AltarType.DoubleJump:
                particle.GetComponent<SpriteRenderer>().material = materialesParticula[1];
                break;
            case AltarType.WallJump:
                particle.GetComponent<SpriteRenderer>().material = materialesParticula[2];
                break;
            default:
                break;
        }
        yield return new WaitForSeconds(kinematicDuration);
        switch (altarType)
        {
            case AltarType.Dash:
                player.dashUnlocked = true;
                imageInstructions.enabled = true;
                abilitiesInstructions.SetText(textos[0]);
                clicked = true;
                break;
            case AltarType.DoubleJump:
                player.dobleJumpUnlocked = true;
                imageInstructions.enabled = true;
                abilitiesInstructions.SetText(textos[1]);
                clicked = true;
                break;
            case AltarType.WallJump:
                player.wallJumpUnlocked = true;
                imageInstructions.enabled = true;
                abilitiesInstructions.SetText(textos[2]);
                clicked = true;
                break;
        }
        emidMod.rateOverTime = 0;
        onKinematic = false;
        SaveSystem.SavePlayerData(player, player.gameObject.transform.GetComponentInChildren<Inventory2>(), player.gameObject.GetComponent<PlayerAttack>());
        SaveSystem.SaveSceneData(ScenesManager.scenesManager);
        player.isOnKinematic = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerClose = true;
            pressEText.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerClose = false;
            pressEText.SetActive(false);
        }
    }
}
