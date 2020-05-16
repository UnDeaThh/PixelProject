using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] float percentReducction;
    private float speedReduced;
    private float normalSpeed;
    private PlayerController2 player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController2>();
        normalSpeed = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController2>().speedMov;
        speedReduced = normalSpeed * percentReducction;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            if (player.MovDir < -0.1f || player.MovDir > 0.1f)
            {
                if (!player.GetComponentInChildren<Inventory2>().waterPasive)
                {
                    player.speedMov = speedReduced;
                }
                if (player.GetComponent<PlayerAttack>().isAttacking)
                {
                    player.rb.velocity = Vector2.zero;
                }
            }
            else
            {
                if (!player.IsGrounded)
                {
                    player.rb.velocity = new Vector2(0f, player.rb.velocity.y);
                }
                else
                {
                    player.rb.velocity = Vector2.zero;
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController2>().speedMov = normalSpeed;
        }
    }
}
