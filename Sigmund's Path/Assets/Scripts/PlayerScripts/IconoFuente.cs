using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IconoFuente : MonoBehaviour
{
    [SerializeField] int iconNumber;
    [SerializeField] int sceneToLoad;
    private PlayerController2 player;
    [SerializeField] Settings settings;
    private Image image;
    private Button button;

    [SerializeField] AudioSource clickSound;

    private void Start()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();

        if(ScenesManager.scenesManager.FountainUnlocked[iconNumber])
        {
            image.enabled = true;
            button.enabled = true;
        }
        else
        {
            image.enabled = false;
            button.enabled = false;
        }
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController2>();

        sceneToLoad += 3;
    }

    private void Update()
    {
        if (!image.enabled)
        {
            if (ScenesManager.scenesManager.FountainUnlocked[iconNumber])
            {
                button.enabled = true;
                image.enabled = true;
            }
        }
    }
    public void InicilizeTP()
    {
        player.isOnKinematic = true;
        transform.parent = null;
        settings.Resume();
        clickSound.Play();
        StartCoroutine(TP());

    }

    IEnumerator TP()
    {
        yield return new WaitForSeconds(0.1f);
        player.StartTP = true;
        ScenesManager.scenesManager.apearsOnFountain = true;
        yield return new WaitForSeconds(1f);
        GameObject.FindObjectOfType<LevelDoor>().Anim.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene(sceneToLoad);
    }
}
