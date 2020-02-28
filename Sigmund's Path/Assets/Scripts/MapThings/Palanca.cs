using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Palanca : MonoBehaviour
{
    public bool isOpen = false;
    [SerializeField] GameObject door;
    [SerializeField] float openSpeed;

    // Update is called once per frame
    void Update()
    {
        if (isOpen)
        {
            door.transform.position = Vector2.MoveTowards(door.transform.position, door.transform.position - new Vector3(0, 10, 0), openSpeed);
        }
    }
}
