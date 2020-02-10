using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;
public class CameraController : MonoBehaviour
{
    public static CameraController cameraController;
    private Transform player;

    public float shakeDuration = 0.3f;          // Time the Camera Shake effect will last
    public float shakeAmplitude = 1.2f;         // Cinemachine Noise Profile Parameter
    public float shakeFrequency = 2.0f;

    private float shakeCurrentTime = 0f;
    public bool letsShake = false;

    // Cinemachine Shake
    public CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;
    private void Awake()
    {
        if(cameraController == null)
        {
            cameraController = this;
        }
        if(cameraController != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (virtualCamera != null)
            virtualCameraNoise = virtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
        SetFollowTarget();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (letsShake)
        {
            shakeCurrentTime = shakeDuration;
            letsShake = false;
        }
        if (virtualCamera != null && virtualCameraNoise != null)
        {
            // If Camera Shake effect is still playing
            if (shakeCurrentTime > 0)
            {
                // Set Cinemachine Camera Noise parameters
                virtualCameraNoise.m_AmplitudeGain = shakeAmplitude;
                virtualCameraNoise.m_FrequencyGain = shakeFrequency;

                // Update Shake Timer
                shakeCurrentTime -= Time.deltaTime;
            }
            else
            {
                // If Camera Shake effect is over, reset variables
                virtualCameraNoise.m_AmplitudeGain = 0f;
                virtualCameraNoise.m_FrequencyGain = shakeFrequency;
                shakeCurrentTime = 0f;
                
            }
        }

    }

    void SetFollowTarget()
    {
        virtualCamera.Follow = player;
    }
}
