using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroy : MonoBehaviour
{
    public float timeToDestroy;
    private void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }
}
