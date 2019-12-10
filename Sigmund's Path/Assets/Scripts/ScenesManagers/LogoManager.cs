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

    IEnumerator GoMainMenu()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("MainMenuScene");
    }
}
