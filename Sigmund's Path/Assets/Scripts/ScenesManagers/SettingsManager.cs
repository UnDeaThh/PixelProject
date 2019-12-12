using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider slider;
    private void Awake()
    {
     slider.value = PlayerPrefs.GetFloat("volume", 0);
    }
    public void BackMenuButton()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void SetVolume(Slider slider)
    {
        audioMixer.SetFloat("volume", slider.value);
        PlayerPrefs.SetFloat("volume", slider.value);
    }
}
