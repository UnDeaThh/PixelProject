using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;
public class CameraController : MonoBehaviour
{
    public static CameraController cameraController;
    private Transform player;
    private PlayerController2 plController;

    public float shakeDuration = 0.3f;          // Time the Camera Shake effect will last
    public float shakeAmplitude = 1.2f;         // Cinemachine Noise Profile Parameter
    public float shakeFrequency = 2.0f;

    private float shakeCurrentTime = 0f;
    public bool letsShake = false;

    public bool isOnBossFight = false;

    // Cinemachine Shake
    public CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;
    private CinemachineConfiner confiner;
    private Collider2D bounds;

    [SerializeField] PostProcessVolume volume;
    [SerializeField] float damagedBloomIntensity;
    Bloom bloom;
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

        //CONFINER CINEMACINE
        confiner = virtualCamera.GetComponent<CinemachineConfiner>();
        if(GameObject.FindGameObjectWithTag("Confiner").TryGetComponent(out Collider2D col))
        {
            bounds = GameObject.FindGameObjectWithTag("Confiner").GetComponent<Collider2D>();
            confiner.m_BoundingShape2D = bounds;
        }

        //POSTPROCESSING
       volume.profile.TryGetSettings(out bloom);

    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        plController = player.gameObject.GetComponent<PlayerController2>();
        if (virtualCamera != null)
        {
            virtualCameraNoise = virtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
        }
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
        if(plController.health < 3)
        {
            bloom.intensity.value = damagedBloomIntensity;
        }
    }

    void SetFollowTarget()
    {
        virtualCamera.Follow = player;
    }
}
