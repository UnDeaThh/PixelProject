using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Experimental.Rendering.LWRP;
public class PostProcManager : MonoBehaviour
{
    PostProcessVolume postProcess;
    Bloom bloom;

    private  Light2D luz;

    private bool setLightning = false;

    private float cntTimeToLighting;
    [SerializeField] float midTimeToLighting;

    private float normalLight;
    [SerializeField] float lightningIntensity;

    private float normalBloomIntensity;
    [SerializeField] float lightingBloomIntensity;

    [SerializeField] float returnNormalSpeed;
    [SerializeField] AudioSource source;

    void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex < 14 || SceneManager.GetActiveScene().buildIndex > 23)
        {
            Destroy(this);
        }
        else
        {
            postProcess = GetComponent<PostProcessVolume>();
            postProcess.profile.TryGetSettings(out bloom);
            luz = GameObject.Find("BGFokLight").GetComponent<Light2D>();
            normalLight = luz.intensity;
            normalBloomIntensity = bloom.intensity.value;
            cntTimeToLighting = midTimeToLighting + Random.Range(-7f, 7f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(cntTimeToLighting > 0f)
        {
            cntTimeToLighting -= Time.deltaTime;
        }
        else
        {
            if (!setLightning)
            {
                luz.intensity = lightningIntensity;
                bloom.intensity.value = lightingBloomIntensity;
                source.pitch = Random.Range(0.7f, 1.25f);
                source.volume = Random.Range(0.4f, 0.6f);
                source.Play();
                setLightning = true;
            }

            if(luz.intensity > normalLight)
            {
                luz.intensity -= returnNormalSpeed;
            }
            if(bloom.intensity.value > normalBloomIntensity)
            {
                bloom.intensity.value -= returnNormalSpeed;
            }

            if(luz.intensity < normalLight)
            {
                luz.intensity = normalLight;
            }

            if(bloom.intensity.value < normalBloomIntensity)
            {
                bloom.intensity.value = normalBloomIntensity;
            }

            if(luz.intensity == normalLight && bloom.intensity.value == normalBloomIntensity)
            {
                cntTimeToLighting = midTimeToLighting + Random.Range(-7.32f, 7.56f);
                setLightning = false;
            }
        }
    }
}
