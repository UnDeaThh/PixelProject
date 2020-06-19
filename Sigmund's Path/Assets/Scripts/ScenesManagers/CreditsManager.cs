using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsManager : MonoBehaviour
{
    PlayerInputs inputs;
    [SerializeField] float time = 21;
    private void OnEnable()
    {
        inputs.LogoControls.Enable();
    }
    private void OnDisable()
    {
        inputs.LogoControls.Disable();

    }

    private void Awake()
    {
        inputs = new PlayerInputs();
        StartCoroutine(EndCredits());
    }

    private void Update()
    {
        if (inputs.LogoControls.Exit.triggered)
        {
            AudioManager.instanceAudio.PlayerFirstMenuSong = false;
            SceneManager.LoadScene("MainMenuScene");
        }
    }

    IEnumerator EndCredits()
    {
        yield return new WaitForSeconds(time);
        AudioManager.instanceAudio.PlayerFirstMenuSong = false;
        SceneManager.LoadScene("MainMenuScene");
    }
}
