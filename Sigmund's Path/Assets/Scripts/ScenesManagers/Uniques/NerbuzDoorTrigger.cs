using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class NerbuzDoorTrigger : MonoBehaviour
{
    public CinemachineVirtualCamera vCamBoss;
    private bool playerIn = false;
    public GameObject lockedDoorCollider;
    private NerbuzBoss nerbuzBrain;
    void Awake()
    {
        nerbuzBrain = GameObject.FindGameObjectWithTag("Nerbuz").GetComponent<NerbuzBoss>();
    }      
    private void Start()
    {
        vCamBoss.Priority = 0;
        lockedDoorCollider.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")){
            lockedDoorCollider.SetActive(true);
            nerbuzBrain.actualState = State.Enter;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIn = true;
            vCamBoss.Priority = 20;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIn = false;
            vCamBoss.Priority = 0;
        }
    }
}
