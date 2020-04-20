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
    [SerializeField] Transform phisicCamera;

    public float shakeTime = 0.3f;          // Time the Camera Shake effect will last
    public float shakeAmplitude = 1.2f;         // Cinemachine Noise Profile Parameter
    public float shakeFrequency = 2.0f;



    private float givenAmplitude;
    private float givenFrequency;

    private float cntShakeTime = 0f;
    public bool letsShake = false;

    private bool isShaking;
    public bool isOnBossFight = false;
    private bool hitAfterParry;
    private bool isGenerateCS;

    // Cinemachine Shake
    public CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;
    private CinemachineConfiner confiner;
    private Collider2D bounds;

    [SerializeField] PostProcessVolume volume;
    [SerializeField] float damagedBloomIntensity;
    Bloom bloom;

    [Header("Parry Shake")]
    [SerializeField] float parryAmplitude;
    [SerializeField] float parryFrequency;
    
    public bool HitAfterParry1 { get => hitAfterParry; set => hitAfterParry = value; }
    public bool IsGenerateCS { get => isGenerateCS; set => isGenerateCS = value; }

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
        if(virtualCamera.Follow == null)
        {
            SetFollowTarget();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isShaking)
        {
            phisicCamera.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        if (!isGenerateCS)
        {
            if (letsShake)
            {
                cntShakeTime = shakeTime;
                letsShake = false;
            }
            if (!hitAfterParry)
            {
                if (virtualCamera != null && virtualCameraNoise != null)
                {
                    // If Camera Shake effect is still playing
                    if (cntShakeTime > 0)
                    {
                        // Set Cinemachine Camera Noise parameters
                        virtualCameraNoise.m_AmplitudeGain = shakeAmplitude;
                        virtualCameraNoise.m_FrequencyGain = shakeFrequency;

                        // Update Shake Timer
                        cntShakeTime -= Time.deltaTime;
                    }
                    else
                    {
                        // If Camera Shake effect is over, reset variables
                        virtualCameraNoise.m_AmplitudeGain = 0f;
                        virtualCameraNoise.m_FrequencyGain = shakeFrequency;
                        cntShakeTime = 0f;
                    }
                }
            }
        }

        if(plController.health < 3)
        {
            bloom.intensity.value = damagedBloomIntensity;
        }

        ShakeHitAfterParry();
        CameraShake(); // Esta la llamamos desde scripts externos
    }

    void SetFollowTarget()
    {
        virtualCamera.Follow = player;
    }

    void ShakeHitAfterParry()
    {
        if (HitAfterParry1)
        {
            if(cntShakeTime > 0)
            {
                cntShakeTime -= Time.deltaTime;

                virtualCameraNoise.m_AmplitudeGain = parryAmplitude;
                virtualCameraNoise.m_FrequencyGain = parryFrequency;
            }
            else
            {
                virtualCameraNoise.m_AmplitudeGain = 0f;
                virtualCameraNoise.m_FrequencyGain = shakeFrequency;
                hitAfterParry = false;
                cntShakeTime = 0f;
            }
        }
    }

    public void CallHitAfterParry()
    {
        hitAfterParry = true;
        cntShakeTime = shakeTime;
    }

    void CameraShake()
    {
        if (isGenerateCS)
        {
            if (cntShakeTime > 0)
            {
                cntShakeTime -= Time.deltaTime;
                virtualCameraNoise.m_AmplitudeGain = givenAmplitude;
                virtualCameraNoise.m_FrequencyGain = givenFrequency;
            }
            else
            {
                virtualCameraNoise.m_AmplitudeGain = 0f;
                virtualCameraNoise.m_FrequencyGain = shakeFrequency;
                isGenerateCS = false;
                isShaking = false;
                cntShakeTime = 0f;
            }
        }
    }

    public void GenerateCamerashake(float amplitude, float frequency, float time)
    {
        givenAmplitude = amplitude;
        givenFrequency = frequency;
        cntShakeTime = time;
        isGenerateCS = true;
        isShaking = true;
    }
}
