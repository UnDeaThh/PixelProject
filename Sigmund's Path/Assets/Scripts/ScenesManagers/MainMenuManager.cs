using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public void ClickOnPlay()
    {
        if(ScenesManager.scenesManager.toLoadScene == 0)
        {
            SceneManager.LoadScene(3);
        }
        else 
        {
            SceneManager.LoadScene(ScenesManager.scenesManager.toLoadScene);
        }
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
