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

    [SerializeField] AudioSource source;

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
        source.Play();
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
        source.Play();
        File.Delete(sceneDataPath);
        if (File.Exists(playerDataPath))
        {
            File.Delete(playerDataPath);
        }
        //REINICIAMOS EL SCENEMANAGER
        ScenesManager.scenesManager.toLoadScene = 0;
        ScenesManager.scenesManager.CutSceneDone = false;
        ScenesManager.scenesManager.FirstTalkAska = false;
        ScenesManager.scenesManager.SwordPicked = false;
        ScenesManager.scenesManager.ApearsOnFountain = false;

        for (int i = 0; i < ScenesManager.scenesManager.PalancasState.Length; i++)
        {
                ScenesManager.scenesManager.PalancasState[i] = false;
        }
        for (int i = 0; i < ScenesManager.scenesManager.HeartsPickUp.Length; i++)
        {
            ScenesManager.scenesManager.HeartsPickUp[i] = false;
        }
        for (int i = 0; i < ScenesManager.scenesManager.CumuloState.Length; i++)
        {
            ScenesManager.scenesManager.CumuloState[i] = false;
        }
        for (int i = 0; i < ScenesManager.scenesManager.UnlokedZone.Length; i++)
        {
            ScenesManager.scenesManager.UnlokedZone[i] = false;
        }
        for (int i = 0; i < ScenesManager.scenesManager.DestruibleWall.Length; i++)
        {
            ScenesManager.scenesManager.DestruibleWall[i] = false;
        }
        for (int i = 0; i < ScenesManager.scenesManager.BossKilled.Length; i++)
        {
            ScenesManager.scenesManager.BossKilled[i] = false;
        }
        for (int i = 0; i < ScenesManager.scenesManager.ShopUnlocked.Length; i++)
        {
            ScenesManager.scenesManager.ShopUnlocked[i] = false;
        }
        for (int i = 0; i < ScenesManager.scenesManager.FountainUnlocked.Length; i++)
        {
            ScenesManager.scenesManager.FountainUnlocked[i] = false;
        }

        SceneManager.LoadScene("T1");
    }
    public void NoDeleteData()
    {
        source.Play();
        panelDeleteData.SetActive(false);
        eventSystem.SetSelectedGameObject(eventSystem.firstSelectedGameObject);
    }
    public void ClickOnPlay()
    {
        source.Play();
        if (ScenesManager.scenesManager.toLoadScene == 0)
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
        source.Play();
        SceneManager.LoadScene("SettingsScene");
    }

    public void ClickOnCredits()
    {
        source.Play();
        SceneManager.LoadScene("CreditsScene");
    }

    public void ClickOnExit()
    {
        source.Play();
        Application.Quit();
    }
}
