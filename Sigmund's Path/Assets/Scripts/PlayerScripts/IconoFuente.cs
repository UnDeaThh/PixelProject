using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IconoFuente : MonoBehaviour
{
    [SerializeField] int iconNumber;
    [SerializeField] int sceneToLoad;
    private PlayerController2 player;
    [SerializeField] Settings settings;

    private void Start()
    {
        if(ScenesManager.scenesManager.FountainUnlocked[iconNumber])
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController2>();

        sceneToLoad += 3;
    }



    public void InicilizeTP()
    {
        //El player pierde el control
        player.isOnKinematic = true;
        transform.parent = null;
        settings.Resume();
        StartCoroutine(TP());
        //Sprite Player desaparece
        //El speramos 0.5f segundos
        //Cargamos siguiente escena
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
