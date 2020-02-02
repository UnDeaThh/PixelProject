using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoManager : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(GoMainMenu());
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenuScene");
        }
    }
    IEnumerator GoMainMenu()
    {
        yield return new WaitForSeconds(6.1f);
        SceneManager.LoadScene("MainMenuScene");
    }
}
