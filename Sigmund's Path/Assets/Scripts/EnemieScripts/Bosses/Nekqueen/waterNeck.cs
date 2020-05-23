using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterNeck : MonoBehaviour
{
    [SerializeField] float speed;
    private void Update()
    {
        transform.localPosition += transform.right * Time.deltaTime * speed;
    }
}
