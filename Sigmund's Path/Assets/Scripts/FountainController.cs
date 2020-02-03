using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FountainController : MonoBehaviour
{
    private bool canHeal = true;
    private bool playerIn = false;
    public int reserveHeal = 4;
    private int currentHeal;
    public float timeHealOne;
    private float currentTimeHealOne;

    private PlayerController2 plController2;

    private void Start()
    {
        plController2 = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController2>();
    }

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
                if(reserveHeal > 0 && plController2.health < plController2.maxHealth)
                {
                    plController2.health++;
                    reserveHeal--;
                    currentTimeHealOne = timeHealOne;
                }
                if(reserveHeal <= 0)
                {
                    reserveHeal = 0;
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
