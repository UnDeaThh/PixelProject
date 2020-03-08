using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadPanelController : MonoBehaviour
{
    [SerializeField] PlayerController2 player;

    private void Start()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController2>();
        }
    }
    public void DeadEvent()
    {
        player.pasSceneDead = true;
    }
}
