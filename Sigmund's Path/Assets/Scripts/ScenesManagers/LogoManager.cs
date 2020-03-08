using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoManager : MonoBehaviour
{
    PlayerInputs inputs;

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
        StartCoroutine(GoMainMenu());
    }

    private void Update()
    {
        if(inputs.LogoControls.Exit.triggered)
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
