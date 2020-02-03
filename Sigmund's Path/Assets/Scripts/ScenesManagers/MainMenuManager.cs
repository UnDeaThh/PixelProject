using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public void ClickOnPlay()
    {
        SceneManager.LoadScene(3);
    }

    public void ClickOnSettings()
    {
        SceneManager.LoadScene("SettingsScene");
    }

    public void ClickOnCredits()
    {
		
    }

    public void ClickOnExit()
    {
        Application.Quit();
    }
}
