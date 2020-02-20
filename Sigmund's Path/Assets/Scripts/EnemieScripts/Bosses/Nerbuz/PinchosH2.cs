using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchosH2 : MonoBehaviour
{
    private NerbuzBoss nerbuzBrain;
    public float speed;
    public float subida;
    private float maxHigh;
    private float minHigh;
    private bool reachedMaxY = false;
    private bool reachedMinY = false;
    public float timeToDescende;
    private float cntTimeToDescende;

    private float cntTimeNextAttack;
    private AudioSource raizSound;

    void Awake()
    {
        nerbuzBrain = GameObject.FindGameObjectWithTag("Nerbuz").GetComponent<NerbuzBoss>();
        raizSound = GetComponentInChildren<AudioSource>();
        minHigh = transform.position.y;
        maxHigh = transform.position.y + subida;
        cntTimeToDescende = timeToDescende;
        cntTimeNextAttack = 0.4f;
    }

    private void Start()
    {
        nerbuzBrain.H2ChargingAnim = false;
        nerbuzBrain.H2attackAnim = true;
        raizSound.Play();
    }
    void Update()
    {
        if(!reachedMaxY)
        {
            if(transform.position.y < maxHigh){
                reachedMaxY = false;
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, maxHigh), speed * Time.deltaTime);
            }
            else
            {
                reachedMaxY = true;
                Debug.Log("ReachedMaxY");
            }
        }

        else
        {
            if(!reachedMinY)
            {
                if(cntTimeToDescende > 0)
                {
                    cntTimeToDescende -= Time.deltaTime;
                }
                else
                {
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, minHigh), speed * Time.deltaTime);
                }
                if(transform.position.y > minHigh)
                {
                    reachedMinY = false;
                }
                else
                {
                    reachedMinY = true;
                }
            }
        }

        if(reachedMinY)
        {
            if(cntTimeNextAttack > 0)
            {
                cntTimeNextAttack -= Time.deltaTime;
            }
            else
            {
                nerbuzBrain.generatorInPlace = false;
                Destroy(gameObject);
            }
            if(nerbuzBrain.cntAttacksH2 >= nerbuzBrain.numberOfAttacksH2)
            {
                nerbuzBrain.isTired = true;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController2>().PlayerDamaged(1, transform.position);
        }
    }
}
