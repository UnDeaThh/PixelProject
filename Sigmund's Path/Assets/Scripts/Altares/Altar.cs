using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Altar : MonoBehaviour
{
    private enum AltarType
    {
        Dash, DoubleJump, WallJump
    }

    [SerializeField] AltarType altarType;
    private bool playerClose;
    [SerializeField] GameObject pressEText;
    [SerializeField] TextMeshProUGUI dashInstructions;
    private PlayerController2 player;
    [SerializeField] float kinematicDuration;

    private void Start()
    {
        pressEText.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController2>();
    }

    private void Update()
    {
        if (playerClose)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!player.dashUnlocked)
                {
                    StartCoroutine(UnlockingDash());
                }
                else
                {
                    //dashInstructions.SetActive(true);
                }
                pressEText.SetActive(false);
            }
        }
        else
        {
            pressEText.SetActive(false);
           // dashInstructions.SetActive(false);
        }
    }

    IEnumerator UnlockingDash()
    {
        player.isOnKinematic = true;
        player.heedArrows = false;
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
    private void OnTriggerStay2D(Collider2D collision)
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
        }
        pressEText.SetActive(false);
    }
}
