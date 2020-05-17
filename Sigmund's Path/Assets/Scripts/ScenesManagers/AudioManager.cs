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
    private PlayerController2 player;


    [SerializeField] float reductionSpeed = 0.01f;

    public bool FfScene { get => ffScene; set => ffScene = value; }
    public bool StartBossSong { get => startBossSong; set => startBossSong = value; }
    public bool EndBossSong { get => endBossSong; set => endBossSong = value; }
    public PlayerController2 Player { get => player; set => player = value; }
    public AudioSource[] BossSongSource { get => bossSongSource; set => bossSongSource = value; }
    public bool PlayedFirstBossSong { get => playedFirstBossSong; set => playedFirstBossSong = value; }

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


    }
    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "Boss1Scene" && SceneManager.GetActiveScene().name != "NerbuzScene" && SceneManager.GetActiveScene().name != "Boss2Scene")
        {
            if (!songSource[0].isPlaying && !songSource[1].isPlaying)
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
        if(SceneManager.GetActiveScene().name != "Boss1Scene" && SceneManager.GetActiveScene().name != "NerbuzScene" && SceneManager.GetActiveScene().name != "Boss2Scene")
        {
            if (!songSource[0].isPlaying && playedFirstSong)
            {
                if (!instanceAudio.songSource[1].isPlaying)
                {
                    instanceAudio.songSource[1].Play();
                }
            }
            else if(!songSource[0].isPlaying && !playedFirstSong)
            {
                if(!songSource[0].isPlaying && !songSource[1].isPlaying)
                {
                    instanceAudio.songSource[0].Play();
                    playedFirstSong = true;
                }
            }
        }




        else
        {
            DesconectNormalSongOnBossScene();
            FadeVolumeOnDead();
            if(startBossSong && !endBossSong)
            {
                if(!instanceAudio.bossSongSource[0].isPlaying && !instanceAudio.playedFirstBossSong)
                {
                    bossSongSource[0].Play();
                    playedFirstBossSong = true;
                }

                if(!instanceAudio.bossSongSource[0].isPlaying && instanceAudio.playedFirstBossSong)
                {
                    if (!instanceAudio.bossSongSource[1].isPlaying)
                    {
                        bossSongSource[1].Play();
                    }
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


    void FadeVolumeOnDead()
    {
        if(player != null)
        {
            if(player.health <= 0)
            {
                if(bossSongSource[0].volume > 0f)
                {
                    if (bossSongSource[0].isPlaying)
                    {
                        bossSongSource[0].volume -= reductionSpeed + 0.005f;
                        playedFirstBossSong = false;
                    }
                }
                else if (bossSongSource[1].isPlaying)
                {
                    if (bossSongSource[1].volume > 0f)
                    {
                        {
                            bossSongSource[1].volume -= reductionSpeed + 0.005f;
                            playedFirstBossSong = false;
                        }
                    }
                }
            }
        }
    }

    void DesconectNormalSongOnBossScene()
    {
        if (songSource[0].isPlaying)
        {
            songSource[0].Stop();
        }
        else if (songSource[1].isPlaying)
        {
            songSource[1].Stop();
        }

        if (playedFirstSong)
        {
            playedFirstSong = false;
        }
    }
}
