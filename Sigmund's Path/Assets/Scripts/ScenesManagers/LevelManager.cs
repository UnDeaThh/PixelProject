using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private PlayerController2 player;
    public Transform[] apearsPos;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController2>();

        if (player.lastScene == 1)
        {
            player.gameObject.transform.position = apearsPos[0].position;
        }
        else if (player.lastScene == 2)
        {
            player.gameObject.transform.position = apearsPos[0].position;
        }
    }

}
