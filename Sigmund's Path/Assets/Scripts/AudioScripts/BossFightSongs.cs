using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightSongs : MonoBehaviour
{
    public AudioSource[] songs;
    public NerbuzDoorTrigger door;
    private bool playedFirstSong = false;

    private void Update()
    {
        if (door.playerIn)
        {
            if(!songs[0].isPlaying && !playedFirstSong)
            {
                songs[0].Play();
                playedFirstSong = true;
            } 
            if (!songs[0].isPlaying && playedFirstSong)
            {
                if (!songs[1].isPlaying)
                {
                    songs[1].Play();
                }
            }
        }
    }
}
