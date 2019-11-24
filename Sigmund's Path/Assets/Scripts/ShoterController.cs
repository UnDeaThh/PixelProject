using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoterController : MonoBehaviour
{
    public float timeBtwShots = 2f;
    private float currentTimeBtwShots;

    public GameObject arrows;

    private void Update()
    {
        if(currentTimeBtwShots <= 0)
        {
            Instantiate(arrows, this.transform.position, Quaternion.Euler(0f, 0f, 180f));
            currentTimeBtwShots = timeBtwShots;
        }
        else if(currentTimeBtwShots > 0)
        {
            currentTimeBtwShots -= Time.deltaTime;
        }
    }
}
