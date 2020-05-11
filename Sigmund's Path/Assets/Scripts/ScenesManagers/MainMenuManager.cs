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
        playerDataPath = Application.persistentDataPath + "/playerdata.info";
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
            SceneManager.LoadScene("T1");
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
        ScenesManager.scenesManager.FirstTalkAska = false;
        ScenesManager.scenesManager.SwordPicked = false;

        for (int i = 0; i < ScenesManager.scenesManager.palancasState.Length; i++)
        {
                ScenesManager.scenesManager.palancasState[i] = false;
        }
        for (int i = 0; i < ScenesManager.scenesManager.heartsPickUp.Length; i++)
        {
            ScenesManager.scenesManager.heartsPickUp[i] = false;
        }
        for (int i = 0; i < ScenesManager.scenesManager.cumuloState.Length; i++)
        {
            ScenesManager.scenesManager.cumuloState[i] = false;
        }
        for (int i = 0; i < ScenesManager.scenesManager.UnlokedZone.Length; i++)
        {
            ScenesManager.scenesManager.UnlokedZone[i] = false;
        }
        for (int i = 0; i < ScenesManager.scenesManager.DestruibleWall.Length; i++)
        {
            ScenesManager.scenesManager.DestruibleWall[i] = false;
        }
        for (int i = 0; i < ScenesManager.scenesManager.DestruibleWall.Length; i++)
        {
            ScenesManager.scenesManager.DestruibleWall[i] = false;
        }

        SceneManager.LoadScene("T1");
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
            SceneManager.LoadScene("T1");
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
