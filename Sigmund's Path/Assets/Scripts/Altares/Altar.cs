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
    [SerializeField] Sprite[] abilitieDeviceButtonSprite;


    private bool clickedForDash = false;
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
        if (playerClose)
        {
            if(image.transform.localScale != Vector3.one)
            {
                image.transform.localScale = Vector3.one;
            }

            image.SetNativeSize();
            if(player.Gamepad != null)
            {
                image.sprite = deviceButtonSprite[1];
            }
            else
            {
                image.sprite = deviceButtonSprite[0];
            }

            if (clickedForDash)
            {
                imageInstructions.transform.localScale = Vector3.one;
                imageInstructions.SetNativeSize();
                if (player.Gamepad != null)
                {
                    imageInstructions.sprite = abilitieDeviceButtonSprite[1];
                }
                else
                {
                    imageInstructions.sprite = abilitieDeviceButtonSprite[0];
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
                            clickedForDash = true;
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
                            pressEText.SetActive(false);
                            abilitiesInstructions.SetText(textos[1]);
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
            clickedForDash = false;
        }
    }

    IEnumerator UnlockAbilitie()
    {
        player.isOnKinematic = true;
        player.heedArrows = false;
        player.rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(kinematicDuration);
        switch (altarType)
        {
            case AltarType.Dash:
                player.dashUnlocked = true;
                break;
            case AltarType.DoubleJump:
                player.dobleJumpUnlocked = true;
                break;
            case AltarType.WallJump:
                player.wallJumpUnlocked = true;
                break;
        }
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
