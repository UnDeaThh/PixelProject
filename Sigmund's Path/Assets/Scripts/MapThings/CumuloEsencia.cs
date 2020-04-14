using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

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
    private Animator anim;
    public UnityEngine.Experimental.Rendering.Universal.Light2D purpleLight;

    private Vector2 colSize;
    private Vector2 colCenter;
    [SerializeField] DestructibleWalls destructibleWalls;
    [SerializeField] int numberOfCumulo;
    private bool isDestroyed = false;
    private GameObject player;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        crashSound = GetComponent<AudioSource>();
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();

        Transform colTrans = col.GetComponent<Transform>();
        colCenter = colTrans.position;
        colSize.x = colTrans.localScale.x * col.bounds.size.x;
        colSize.y = colTrans.localScale.y * col.bounds.size.y;

        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        isDestroyed = ScenesManager.scenesManager.cumuloState[numberOfCumulo];

        if (isDestroyed)
        {
            Destroy(gameObject);
        }
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

        if(lifes <= 0)
        {
            if (!crashSound.isPlaying)
            {
                Destroy(gameObject);
            }
        }
        ActiveLightBehindWall();
    }

    void ActiveLightBehindWall()
    {
        if (destructibleWalls != null)
        {
            if (destructibleWalls.IsDestroyed)
            {
                purpleLight.enabled = true;
            }
            else
            {
                purpleLight.enabled = false;
            }
        }
        else
            return;
    }

    public void DesPegado()
    {
        anim.SetBool("hit", false);
        if(lifes <= 0)
        {
            purpleLight.enabled = false;
        }
    }

    public void TakeDamage()
    {
        lifes--;
        anim.SetBool("hit", true);
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

            Debug.Log("Save");
            ScenesManager.scenesManager.cumuloState[numberOfCumulo] = true;

            SaveSystem.SavePlayerData(player.GetComponent<PlayerController2>(), player.GetComponentInChildren<Inventory2>(), player.GetComponent<PlayerAttack>());
            SaveSystem.SaveSceneData(ScenesManager.scenesManager);
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
