using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CumuloEsencia : MonoBehaviour
{
    public int lifes = 3;
    private SpriteRenderer sr;
    private AudioSource crashSound;
    public AudioClip bigCrashSound;
    public Sprite[] brokenSprites = new Sprite[3];
    public ParticleSystem[] ps = new ParticleSystem[3];

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        crashSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        switch (lifes)
        {
            case 3:
                sr.sprite = brokenSprites[0];
                break;
            case 2:
                sr.sprite = brokenSprites[1];
                break;
            case 1:
                sr.sprite = brokenSprites[2];
                break;
            case 0:
                sr.enabled = false;
                break;
        }
    }

    public void TakeDamage()
    {
        lifes--;
        
        if(lifes > 0)
        {
            crashSound.Play();
            ps[0].Emit(5);
        }
        else
        {
            crashSound.clip = bigCrashSound;
            crashSound.Play();
            for (int i = 0; i < ps.Length; i++)
            {
                ps[i].Emit(7);
            }
            Destroy(gameObject, 0.7f);
        }
    }
}
