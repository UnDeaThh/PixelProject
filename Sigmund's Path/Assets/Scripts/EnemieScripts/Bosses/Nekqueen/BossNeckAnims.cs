using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossNeckAnims : MonoBehaviour
{
    [SerializeField] BossNeck neckBrain;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip[] atackSounds;

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
        source.Play();
    }

    void DobleSound()
    {
        source.clip = atackSounds[0];
        source.pitch = Random.Range(0.85f, 1.15f);
        source.Play();
    }
}
