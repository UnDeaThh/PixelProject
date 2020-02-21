using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadPanelController : MonoBehaviour
{
    [SerializeField] PlayerController2 player;

    public void DeadEvent()
    {
        player.pasSceneDead = true;
    }
}
