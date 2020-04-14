using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class Torch : MonoBehaviour
{
    [SerializeField] UnityEngine.Experimental.Rendering.Universal.Light2D orangeLight;
    [SerializeField] float maxLight;
    [SerializeField] float minLight;
    [SerializeField] float lightSpeed;
    private float startIntensity;
    private bool go;

    private void Start()
    {
        startIntensity = orangeLight.intensity;
    }
    private void Update()
    {
        if (go)
        {
            if(orangeLight.intensity < startIntensity + maxLight)
            {
                orangeLight.intensity += lightSpeed;
            }
            else
            {
                go = false;
            }
        }
        else
        {
            if (orangeLight.intensity > startIntensity - minLight)
            {
                orangeLight.intensity -= lightSpeed;
            }
            else
            {
                go = true;
            }
        }
    }
}
