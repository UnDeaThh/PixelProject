using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    public float launchTime;
    private float cntLaunchTime;
    public GameObject prefabLanza;
    public Vector3 rotation;

    private void Update()
    {
        if(cntLaunchTime < launchTime)
        {
            cntLaunchTime += Time.deltaTime;
        }
        else
        {
            Instantiate(prefabLanza, transform.position, Quaternion.Euler(rotation));
            cntLaunchTime = 0;
        }
    }
}
