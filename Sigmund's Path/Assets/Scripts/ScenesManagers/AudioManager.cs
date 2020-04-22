using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instanceAudio;

    [SerializeField] AudioSource[] songSource;
    [SerializeField] AudioSource[] bossSongSource;

    private bool playedFirstSong = false;
    private bool ffScene;
    private bool startBossSong;
    private bool endBossSong;
    private bool playedFirstBossSong = false;


    [SerializeField] float reductionSpeed = 0.01f;

    public bool FfScene { get => ffScene; set => ffScene = value; }
    public bool StartBossSong { get => startBossSong; set => startBossSong = value; }
    public bool EndBossSong { get => endBossSong; set => endBossSong = value; }

    private void Awake()
    {
        if (instanceAudio == null)
        {
            instanceAudio = this;
        }
        if (instanceAudio != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

        if(SceneManager.GetActiveScene().name != "Boss1Scene")
        {
            if(!instanceAudio.songSource[0].isPlaying && !instanceAudio.songSource[1].isPlaying)
            {
                instanceAudio.songSource[0].Play();
                playedFirstSong = true;
            }
        }
        else
        {
            instanceAudio.songSource[0].Stop();
            instanceAudio.songSource[0].Stop();
        }
    }
    private void Update()
    {

        
        if(SceneManager.GetActiveScene().name != "Boss1Scene")
        {
            if (!instanceAudio.songSource[0].isPlaying && instanceAudio.playedFirstSong)
            {
                if (!instanceAudio.songSource[1].isPlaying)
                {
                    instanceAudio.songSource[1].Play();
                }
            }
        }
        else
        {
            if(startBossSong && !endBossSong)
            {
                if(!instanceAudio.bossSongSource[0].isPlaying && !instanceAudio.playedFirstBossSong)
                {
                    bossSongSource[0].Play();
                    playedFirstBossSong = true;
                }
            }
            else if(startBossSong && endBossSong)
            {
                if (bossSongSource[0].isPlaying)
                {
                    if (bossSongSource[0].volume > 0f)
                    {
                        bossSongSource[0].volume -= reductionSpeed;
                    }
                }
                else if (bossSongSource[1].isPlaying)
                {
                    if (bossSongSource[1].volume > 0f)
                    {
                        bossSongSource[1].volume -= reductionSpeed;
                    }
                }
            }
        }
    }
}
