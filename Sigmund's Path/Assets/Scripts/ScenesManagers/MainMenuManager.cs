using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;
using System.Runtime.Serialization.Formatters.Binary;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] Button continueButton;
    [SerializeField] GameObject panelDeleteData;
    [SerializeField] EventSystem eventSystem;
    [SerializeField] GameObject noDeleteButton;
    private string sceneDataPath;
    private string playerDataPath;

    private void OnEnable()
    {
        sceneDataPath = Application.persistentDataPath + "/scenesdata.info";
        playerDataPath = Application.persistentDataPath + "/player.info";
        panelDeleteData.SetActive(false);
        if (File.Exists(sceneDataPath))
        {
            continueButton.interactable = true;
        }
        else
        {
            continueButton.interactable = false;
        }
    }
    public void ClickOnNewGame()
    {
        if (File.Exists(sceneDataPath))
        {
            panelDeleteData.SetActive(true);
            eventSystem.SetSelectedGameObject(noDeleteButton);
        }
        else
        {
            SceneManager.LoadScene(3);
        }
    }

    public void YesDeleteData()
    {
        File.Delete(sceneDataPath);
        if (File.Exists(playerDataPath))
        {
            File.Delete(playerDataPath);
        }
        //REINICIAMOS EL SCENEMANAGER
        ScenesManager.scenesManager.toLoadScene = 0;
        ScenesManager.scenesManager.cutSceneDone = false;
        SceneManager.LoadScene(3);
    }
    public void NoDeleteData()
    {
        panelDeleteData.SetActive(false);
        eventSystem.SetSelectedGameObject(eventSystem.firstSelectedGameObject);
    }
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
