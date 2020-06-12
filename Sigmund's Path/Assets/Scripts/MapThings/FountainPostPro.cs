using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class FountainPostPro : MonoBehaviour
{
    FountainController fountain;

    PostProcessVolume postProcess;
    Vignette vignette;

    private ColorParameter vignetteColor;
    private Color normalColor;
    [SerializeField] Color insideColor;
    [SerializeField] ColorParameter ci;

    void Start()
    {
        fountain = GameObject.FindObjectOfType<FountainController>();
        postProcess = GetComponent<PostProcessVolume>();
        postProcess.profile.TryGetSettings(out vignette);
        normalColor = vignette.color;
        if (fountain == null)
        {
            Destroy(this);
        }
        ci.overrideState = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (fountain.PlayerIn)
        {
            
            vignette.color = ci;
        }
        else
        {
            
        }
    }
}
