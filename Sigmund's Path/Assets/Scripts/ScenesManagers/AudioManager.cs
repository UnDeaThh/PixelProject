using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instanceAudio;

    [SerializeField] AudioSource[] songSource;
    [SerializeField] AudioSource[] bossSongSource;
    [SerializeField] AudioSource[] menuSongs;

    [SerializeField] AudioSource ambientSound;
    [SerializeField] AudioSource rainSound;

    private bool playedFirstSong = false;
    private bool ffScene;
    private bool startBossSong;
    private bool endBossSong;
    private bool playedFirstBossSong = false;
    private bool playerFirstMenuSong = false;
    private PlayerController2 player;


    [SerializeField] float reductionSpeed = 0.01f;

    public bool FfScene { get => ffScene; set => ffScene = value; }
    public bool StartBossSong { get => startBossSong; set => startBossSong = value; }
    public bool EndBossSong { get => endBossSong; set => endBossSong = value; }
    public PlayerController2 Player { get => player; set => player = value; }
    public AudioSource[] BossSongSource { get => bossSongSource; set => bossSongSource = value; }
    public bool PlayedFirstBossSong { get => playedFirstBossSong; set => playedFirstBossSong = value; }
    public bool PlayerFirstMenuSong { get => playerFirstMenuSong; set => playerFirstMenuSong = value; }

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
        if(SceneManager.GetActiveScene().name != "MainMenuScene" && SceneManager.GetActiveScene().name != "SettingsScene" && SceneManager.GetActiveScene().name != "IntroScene")
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController2>();
        }
    }

    void StopMenuSongs()
    {
        if(menuSongs[0].isPlaying || menuSongs[1].isPlaying)
        {
            menuSongs[1].Stop();
            menuSongs[0].Stop();
        }
        
    }
    private void Update()
    {
        //____________________________________
        if(SceneManager.GetActiveScene().name == "MainMenuScene" || SceneManager.GetActiveScene().name == "SettingsScene") // Estas en el menu
        {
            if (!menuSongs[0].isPlaying && !playerFirstMenuSong)
            {
                menuSongs[0].Play();
                playerFirstMenuSong = true;
            }
            else if (!menuSongs[0].isPlaying && playerFirstMenuSong)
            {
                if (!menuSongs[1].isPlaying)
                {
                    menuSongs[1].Play();
                }
            }

            if(!bossSongSource[0].isPlaying || !bossSongSource[1].isPlaying)
            {
                bossSongSource[0].Stop();
                bossSongSource[1].Stop();
            }
        }
        else if ( SceneManager.GetActiveScene().name == "CreditsScene")
        {
            StopMenuSongs();
            if (!bossSongSource[1].isPlaying)
            {
                bossSongSource[1].Play();
            }
        }
        else
        {
            StopMenuSongs();
            if (SceneManager.GetActiveScene().name != "IntroScene")
            {
                if(player == null)
                {
                    player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController2>();
                }


                if(SceneManager.GetActiveScene().name != "Boss1Scene" && SceneManager.GetActiveScene().name != "NerbuzScene" && SceneManager.GetActiveScene().name != "Boss2Scene") //Gameplay Normal
                {
                    #region RainSound
                    if (SceneManager.GetActiveScene().buildIndex >= 14 && SceneManager.GetActiveScene().buildIndex <= 23)
                    {
                        if (SceneManager.GetActiveScene().buildIndex == 22)
                        {
                            rainSound.Stop();
                        }
                        else
                        {
                            if (!rainSound.isPlaying)
                            {
                                rainSound.Play();
                            }
                        }
                    }
                    else
                    {
                        rainSound.Stop();
                    }
                    #endregion

                    if (!ambientSound.isPlaying) // Soindo Ambiente
                    {
                        ambientSound.Play();
                    }

                    if (!songSource[0].isPlaying && playedFirstSong)
                    {
                        if (!instanceAudio.songSource[1].isPlaying)
                        {
                            instanceAudio.songSource[1].Play();
                        }
                    }
                    else if (!songSource[0].isPlaying && !playedFirstSong)
                    {
                        if (!songSource[0].isPlaying && !songSource[1].isPlaying)
                        {
                            instanceAudio.songSource[0].Play();
                            playedFirstSong = true;
                        }
                    }
                }
                else //Bosses
                {
                    if (ambientSound.isPlaying)
                    {
                        ambientSound.Stop();
                    }

                    DesconectNormalSongOnBossScene();
                    FadeVolumeOnDead();
                    if (startBossSong && !endBossSong)
                    {
                        if (!instanceAudio.bossSongSource[0].isPlaying && !instanceAudio.playedFirstBossSong)
                        {
                            bossSongSource[0].Play();
                            playedFirstBossSong = true;
                        }

                        if (!instanceAudio.bossSongSource[0].isPlaying && instanceAudio.playedFirstBossSong)
                        {
                            if (!instanceAudio.bossSongSource[1].isPlaying)
                            {
                                bossSongSource[1].Play();
                            }
                        }
                    }
                    else if (startBossSong && endBossSong)
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
        //_____________________________________

        /*

        if (SceneManager.GetActiveScene().name != "MainMenuScene" && SceneManager.GetActiveScene().name != "SettingsScene" && SceneManager.GetActiveScene().name != "IntroScene")
        {
            if(player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController2>();
            }
        }
        #region RainSound
        if(SceneManager.GetActiveScene().buildIndex >= 14 && SceneManager.GetActiveScene().buildIndex <= 23)
        {
            if(SceneManager.GetActiveScene().buildIndex == 22)
            {
                rainSound.Stop();
            }
            else
            {
                if (!rainSound.isPlaying)
                {
                    rainSound.Play();
                }
            }
        }
        else
        {
            rainSound.Stop();
        }
        #endregion
        if (SceneManager.GetActiveScene().name != "Boss1Scene" && SceneManager.GetActiveScene().name != "NerbuzScene" && SceneManager.GetActiveScene().name != "Boss2Scene")
        {
            if (SceneManager.GetActiveScene().name == "MainMenuScene" || SceneManager.GetActiveScene().name == "SettingsScene" )
            {
                if (!menuSongs[0].isPlaying && !playerFirstMenuSong)
                {
                    menuSongs[0].Play();
                    playerFirstMenuSong = true;
                }
                else if (!menuSongs[0].isPlaying && playerFirstMenuSong)
                {
                    if (!menuSongs[1].isPlaying)
                    {
                        menuSongs[1].Play();
                    }
                }
            }
            else
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
                if (!ambientSound.isPlaying)
                {
                    ambientSound.Play();
                }
        }


        else
        {

            if (ambientSound.isPlaying)
            {
                ambientSound.Stop();
            }

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
        */
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
