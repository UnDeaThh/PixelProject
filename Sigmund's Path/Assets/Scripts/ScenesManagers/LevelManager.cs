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

        if (player.lastScene == 3)
        {
            player.gameObject.transform.position = apearsPos[0].position;
        }
        else if (player.lastScene == 4)
        {
            player.gameObject.transform.position = apearsPos[0].position;
        }
    }

}
