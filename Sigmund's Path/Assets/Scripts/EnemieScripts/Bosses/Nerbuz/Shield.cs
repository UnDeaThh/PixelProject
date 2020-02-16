using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private NerbuzBoss nerbuzBrain;
    private Collider2D col;

    public bool shieldBuild;

     private void Awake()
    {
        nerbuzBrain = GameObject.FindGameObjectWithTag("Nerbuz").GetComponent<NerbuzBoss>();
        col = GetComponent<Collider2D>();
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
