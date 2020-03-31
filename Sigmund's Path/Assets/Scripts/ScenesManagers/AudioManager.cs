using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    [SerializeField] AudioSource[] song;
    private bool playedFirstSong = false;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        if (!song[0].isPlaying && !playedFirstSong)
        {
            song[0].Play();
            playedFirstSong = true;
        }
    }

    private void Update()
    {
        if (!song[0].isPlaying && playedFirstSong)
        {
            if (!song[1].isPlaying)
            {
                song[1].Play();
            }
        }
        /*
        if (ScenesManager.scenesManager.actualScene == nerbuzFightScene)
        {
            for (int i = 0; i < song.Length; i++)
            {
                song[i].Stop();
            }
        }
        */
    }
}
