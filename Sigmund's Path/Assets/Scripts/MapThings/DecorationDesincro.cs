using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class DecorationDesincro : MonoBehaviour
{

    private Animator anim;
   
    void Start()
    {
        anim = GetComponent<Animator>();

        float randomStart = Random.Range(0, 1f);
        anim.SetFloat("randomStart", randomStart);
    }

}
