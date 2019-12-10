using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public void ClickOnPlay()
    {
        SceneManager.LoadScene("GamePlayScene");
    }

    public void ClickOnSettings()
    {

    }

    public void ClickOnCredits()
    {

    }

    public void ClickOnExit()
    {
        Application.Quit();
    }
}
