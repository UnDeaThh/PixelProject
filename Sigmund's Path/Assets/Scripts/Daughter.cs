using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class Daughter : MonoBehaviour
{
    private bool dialogueEnded = false;
    [SerializeField] Image blackImage;
    [SerializeField] float alphaGain;
    float alpha = 0.0f;
    [SerializeField] GameObject logo;

    private bool justOneEnd;

    public bool DialogueEnded { get => dialogueEnded; set => dialogueEnded = value; }


    private void Start()
    {
        logo.SetActive(false);
        blackImage.color = new Color(0f, 0f, 0f, 0f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponent<NpcDialogue>().TriggerDialogue();
        }
    }

    private void Update()
    {
        if (dialogueEnded)
        {
            if(blackImage.color.a < 1)
            {
                alpha += alphaGain;
                blackImage.color = new Color(0f, 0f, 0f, alpha);
            }
            else
            {
                if (!justOneEnd)
                {
                    StartCoroutine(EndGame());
                    justOneEnd = false;
                }
            }
        }
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(1f);
        logo.SetActive(true);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("CreditsScene");
    }
}
