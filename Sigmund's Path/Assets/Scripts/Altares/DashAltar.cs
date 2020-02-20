using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAltar : MonoBehaviour
{
    private bool playerClose;
    public GameObject pressEText;
    public GameObject dashInstructions;
    private PlayerController2 player;
    public float kinematicDuration;

    private void Start()
    {
        pressEText.SetActive(false);pressEText.SetActive(false);
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
                    dashInstructions.SetActive(true);
                }
                pressEText.SetActive(false);
            }
        }
        else
        {
            pressEText.SetActive(false);
            dashInstructions.SetActive(false);
        }
    }

    IEnumerator UnlockingDash()
    {
        player.isOnKinematic = true;
        player.heedArrows = false;
        yield return new WaitForSeconds(kinematicDuration);
        player.dashUnlocked = true;
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
