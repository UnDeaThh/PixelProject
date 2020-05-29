using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TrigerCameraPriority : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;

    private void Start()
    {
        virtualCamera.Priority = 0;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            virtualCamera.Priority = 15;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            virtualCamera.Priority = 0;
        }
    }
}
