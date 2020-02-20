using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesH3 : MonoBehaviour
{
    public float rotationSpeed = 1;
    void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}
