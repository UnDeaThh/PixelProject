using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] float percentReducction;
    private float speedReduced;
    private float normalSpeed;
    private PlayerController2 player;
    private PlayerAudio plAudio;
    [SerializeField] GameObject gotas;
    private Collider2D col;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip[] soundSteps;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController2>();
        plAudio = player.GetComponentInChildren<PlayerAudio>();
        normalSpeed = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController2>().speedMov;
        speedReduced = normalSpeed * percentReducction;
        col = GetComponent<Collider2D>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!player.GetComponent<PlayerParry>().IsParry)
            {
                Vector2 pos = new Vector2(other.transform.position.x, col.bounds.max.y);
                var salpicadura = Instantiate(gotas, pos, Quaternion.Euler(-90, 0f, 0f));
                Destroy(salpicadura, 0.5f);
                source.pitch = Random.Range(0.8f, 1.2f);
                source.Play();
                plAudio.walkSound.clip = soundSteps[0];
            }
        }
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
            if (!player.GetComponent<PlayerParry>().IsParry)
            {
                collision.GetComponent<PlayerController2>().speedMov = normalSpeed;
                plAudio.walkSound.clip = soundSteps[1];
            }

        }
    }
}
