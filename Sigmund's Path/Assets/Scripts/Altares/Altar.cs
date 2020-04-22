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


    private bool clicked = false;
    private bool onKinematic = false;

    private void Start()
    {
        canvasObject.SetActive(true);
        pressEText.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController2>();
        abilitiesInstructions.SetText("");
        imageInstructions.enabled = false;
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
        player.heedArrows = false;
        onKinematic = true;
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
        onKinematic = false;
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
