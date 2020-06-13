using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossNeckAnims : MonoBehaviour
{
    [SerializeField] BossNeck neckBrain;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip[] atackSounds;
    [SerializeField] AudioClip growl;

    void StopDobleAttack()
    {
        neckBrain.DoDobleAttack = false;
    }

    void StopSalpicadura()
    {
        neckBrain.DoRangeAttack = false;
    }

    void ExecuteAttack()
    {
        neckBrain.CloseAttack();
    }

    void ThrowWater()
    {
        neckBrain.ThrowWater();
    }

    void StopPunch()
    {
        neckBrain.PunchAttack = false;
    }

    void SalplicaduraSound()
    {
        source.clip = atackSounds[1];
        source.volume = 0.5f;
        source.Play();
    }

    void DobleSound()
    {
        source.clip = atackSounds[0];
        source.volume = 1f;
        source.pitch = Random.Range(0.85f, 1.15f);
        source.Play();
    }

    void CallDead()
    {
        neckBrain.isDisolve = true;
    }

    void IsStanding()
    {
        neckBrain.ActualState = State.H1;
        AudioManager.instanceAudio.StartBossSong = true;
    }

    void InitialGrowl()
    {
        source.clip = growl;
        CameraController.cameraController.IsGenerateCS = true;
        CameraController.cameraController.GenerateCamerashake(8, 3, growl.length - 0.05f);
        source.Play();
    }
}
