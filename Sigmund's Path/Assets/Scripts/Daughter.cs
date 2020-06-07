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

    public bool DialogueEnded { get => dialogueEnded; set => dialogueEnded = value; }


    private void Start()
    {
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
                SceneManager.LoadScene("CreditsScene");
            }
        }
    }
}
