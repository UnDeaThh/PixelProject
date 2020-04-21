using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    private PlayerController2 plController;
    private PlayerAttack plAttack;
    private PlayerParry plParry;
    public AudioSource jumpSound;
    public AudioSource walkSound;
    public AudioSource attackSound;
    public AudioClip[] attackClips;
    public AudioSource dashSound;
    public AudioSource[] healingSound;
    private void Awake()
    {
        plController = GetComponentInParent<PlayerController2>();
        plAttack = GetComponentInParent<PlayerAttack>();
        plParry = GetComponentInParent<PlayerParry>();
    }

    private void Update()
    {
        //WALKSOUND
        if(Mathf.Abs(plController.rb.velocity.x) > 0.1f && plController.IsGrounded)
        {
            if (!walkSound.isPlaying)
            {
                walkSound.pitch = Random.Range(0.75f, 1.3f);
                walkSound.Play();
            }
        }
        else
        {
            walkSound.Stop();
        }
        

    }
}
