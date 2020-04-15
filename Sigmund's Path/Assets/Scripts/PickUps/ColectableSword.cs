using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColectableSword : MonoBehaviour
{
    [SerializeField] Image[] instructionsForSwordImage;
    [SerializeField] Sprite[] devicesSprites;
    [SerializeField] GameObject[] texts;
    [SerializeField] float timeInstructions = 2f;

    private PlayerController2 player;
    private void Start()
    {
        if (ScenesManager.scenesManager.SwordPicked)
        {
            Destroy(gameObject);
        }

        for (int i = 0; i < instructionsForSwordImage.Length; i++)
        {
            instructionsForSwordImage[i].enabled = false;
        }
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].SetActive(false);
        }

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController2>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObject player = collision.gameObject;
            collision.GetComponent<PlayerAttack>().haveSword = true;
            collision.GetComponent<PlayerController2>().isOnKinematic = true;
            FindObjectOfType<NpcDialogue>().isTalking = true;
            ScenesManager.scenesManager.SwordPicked = true;
            SaveSystem.SavePlayerData(player.GetComponent<PlayerController2>(), player.GetComponentInChildren<Inventory2>(), player.GetComponent<PlayerAttack>());
            GetComponent<Collider2D>().enabled = false;
            Destroy(gameObject.transform.GetChild(0).gameObject);
            StartCoroutine(DeviceImages());
        }
    }

    IEnumerator DeviceImages()
    {

        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].SetActive(true);
        }

        if (player.Gamepad != null)
        {
            instructionsForSwordImage[0].sprite = devicesSprites[1]; //AttackImage sprite = west gamepad;
            instructionsForSwordImage[1].sprite = devicesSprites[3]; //ParryImage sprite = R1 gamepad
        }
        else
        {
            instructionsForSwordImage[0].sprite = devicesSprites[0]; //AttackImage sprite = leftClick;
            instructionsForSwordImage[1].sprite = devicesSprites[2]; //ParryImage sprite = rightClick;
        }
        for (int i = 0; i < instructionsForSwordImage.Length; i++)
        {
            instructionsForSwordImage[i].enabled = true;
            instructionsForSwordImage[i].SetNativeSize();
        }

        yield return new WaitForSeconds(timeInstructions);

        for (int i = 0; i < instructionsForSwordImage.Length; i++)
        {
            instructionsForSwordImage[i].enabled = false;
        }
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].SetActive(false);
        }
        Destroy(gameObject);
    }
}
