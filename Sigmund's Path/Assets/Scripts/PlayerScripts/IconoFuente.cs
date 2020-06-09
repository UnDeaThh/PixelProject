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
    private Inventory2 inventory;
    [SerializeField] Settings settings;
    private Image image;
    private Button button;

    [SerializeField] AudioSource clickSound;
    [SerializeField] AudioClip denegateSound;

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
        inventory = player.GetComponentInChildren<Inventory2>();

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
        if (inventory.nTP > 0)
        {
            player.isOnKinematic = true;
            transform.parent = null;
            settings.Resume();
            clickSound.Play();
            StartCoroutine(TP());
        }
        else
        {
            clickSound.clip = denegateSound;
            clickSound.Play();
        }

    }

    IEnumerator TP()
    {
        yield return new WaitForSeconds(0.1f);
        player.StartTP = true;
        ScenesManager.scenesManager.ApearsOnFountain = true;
        yield return new WaitForSeconds(1f);
        GameObject.FindObjectOfType<LevelDoor>().Anim.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1.2f);
        inventory.nTP--;
        SaveSystem.SavePlayerData(player, inventory, player.GetComponent<PlayerAttack>());
        SceneManager.LoadScene(sceneToLoad);
    }
}
