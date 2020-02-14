using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CumuloEsencia : MonoBehaviour
{
    public int lifes = 3;
    private SpriteRenderer sr;
    private bool destroyed;
    public Sprite[] brokenSprites = new Sprite[3];
    public ParticleSystem[] ps = new ParticleSystem[3];

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
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
                destroyed = true;
                break;
        }

        if (destroyed)
        {
            for (int i = 0; i < ps.Length; i++)
            {
                ps[i].Play();
            }
            for (int i = 0; i < ps.Length; i++)
            {
                
            }
            destroyed = false;
            Destroy(gameObject, 0.3f);
        }
    }

    public void TakeDamage()
    {
        lifes--;
        ps[0].Play();
    }
}
