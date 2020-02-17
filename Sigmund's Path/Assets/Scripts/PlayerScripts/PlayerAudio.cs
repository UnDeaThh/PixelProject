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

    private void Awake()
    {
        plController = GetComponentInParent<PlayerController2>();
        plAttack = GetComponentInParent<PlayerAttack>();
        plParry = GetComponentInParent<PlayerParry>();
    }

    private void Update()
    {
        //WALKSOUND
        if(Mathf.Abs(plController.rb.velocity.x) > 0.1f && plController.isGrounded)
        {
            if (!walkSound.isPlaying)
            {
                walkSound.Play();
            }
        }
        else
        {
            walkSound.Stop();
        }
        

    }
}
