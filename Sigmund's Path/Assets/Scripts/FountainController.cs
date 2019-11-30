using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FountainController : MonoBehaviour
{
    private bool canHeal = true;
    private bool playerIn = false;
    public int maxHeal = 4;
    private int currentHeal;
    public float timeHealOne;
    private float currentTimeHealOne;
    private PlayerController player;

    private void Update()
    {
        if (playerIn && canHeal)
        {
            if(currentTimeHealOne > 0)
            {
                currentTimeHealOne -= Time.deltaTime;
            }
            else if(currentTimeHealOne <= 0)
            {
                if(maxHeal > 0 && player.health < player.maxHealth)
                {
                    player.health++;
                    maxHeal--;
                    currentTimeHealOne = timeHealOne;
                }
                else if(maxHeal > 0 && player.health >= player.maxHealth)
                {
                    player.potions++;
                    maxHeal--;
                    currentTimeHealOne = timeHealOne;
                }

                if(maxHeal <= 0)
                {
                    maxHeal = 0;
                    canHeal = false;
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.tag == "Player")
        {
            playerIn = true;
            currentTimeHealOne = timeHealOne;
            player = other.GetComponent<PlayerController>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            playerIn = false;
        }
    }
}
