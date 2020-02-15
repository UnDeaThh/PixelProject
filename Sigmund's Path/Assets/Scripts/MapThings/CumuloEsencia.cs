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
    public GameObject souls;
    private Collider2D col;

    private Vector2 colSize;
    private Vector2 colCenter;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        crashSound = GetComponent<AudioSource>();
        col = GetComponent<Collider2D>();

        Transform colTrans = col.GetComponent<Transform>();
        colCenter = colTrans.position;
        colSize.x = colTrans.localScale.x * col.bounds.size.x;
        colSize.y = colTrans.localScale.y * col.bounds.size.y;
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
            RandomInstantiateSouls();
            Destroy(gameObject, 0.7f);
        }
    }

    void RandomInstantiateSouls()
    {
        for (int i = 0; i < 5; i++)
        {
            Vector2 randomPosition = colCenter + new Vector2(Random.Range(-colSize.x/2, colSize.x/2), Random.Range(-colSize.y/2, colSize.y/2));
            Instantiate(souls, randomPosition, Quaternion.identity);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(colCenter, colSize);
    }
}
