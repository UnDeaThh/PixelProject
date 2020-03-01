using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    private float speedReduced;
    [SerializeField] float percentReducction;
    private float normalSpeed;

    private void Start()
    {
        normalSpeed = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController2>().speedMov;
        speedReduced = normalSpeed * percentReducction;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            if (!collision.GetComponent<Inventory2>().waterPasive)
            {
                collision.GetComponent<PlayerController2>().speedMov = speedReduced;
            }
            else
                return;
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
