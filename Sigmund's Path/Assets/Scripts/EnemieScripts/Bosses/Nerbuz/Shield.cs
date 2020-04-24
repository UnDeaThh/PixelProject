using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private NerbuzAI nerbuzBrain;
    private Collider2D col;
    private Animator anim;
    Material material;
    public bool shieldBuild;
    private bool isDisolve;
    private float fade;

    public bool IsDisolve { get => isDisolve; set => isDisolve = value; }

    private void Start()
    {
        nerbuzBrain = GameObject.FindGameObjectWithTag("Nerbuz").GetComponent<NerbuzAI>();
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        material = GetComponent<SpriteRenderer>().material;

        fade = 1;
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

        if (nerbuzBrain.IsTired)
        {
            DisolveShield();
        }
    }
    void ActivateShield()
    {
        shieldBuild = true;
    }

    void DisolveShield()
    {
        if (isDisolve)
        {
            if(fade <= 0)
            {
                fade = 0;
                Destroy(gameObject);
            }
            else
            {
                fade -= Time.deltaTime;
            }
            material.SetFloat("_Fade", fade);
        }
    }
}
