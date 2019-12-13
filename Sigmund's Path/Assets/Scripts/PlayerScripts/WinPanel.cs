using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinPanel : MonoBehaviour
{
    public GameManager GM;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
         anim.SetBool("isPlayer", GM.isPlayer); 
    }

    public void GameWin()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
