using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private NerbuzBoss nerbuzBrain;
    private Collider2D col;
    private Animator anim;
    private bool ffDestroy;
    public bool shieldBuild;


    private void Awake()
    {
        nerbuzBrain = GameObject.FindGameObjectWithTag("Nerbuz").GetComponent<NerbuzBoss>();
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if(!shieldBuild)
        {
            col.enabled = false;
        }
        else
        {
            col.enabled = true;
        }

        if (nerbuzBrain.isTired && !ffDestroy)
        {
            anim.SetTrigger("destroyTrigger");
            ffDestroy = true;
        }
    }
    void ActivateShield()
    {
        shieldBuild = true;
    }

    void DeactivateShield()
    {
        Destroy(gameObject);
    }
}
