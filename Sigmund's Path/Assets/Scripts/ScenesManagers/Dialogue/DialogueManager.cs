using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;
    [SerializeField] GameObject bgDialogue;
    [SerializeField] TextMeshProUGUI dialogueTextGO;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] EventSystem eventSystem;
    [SerializeField] GameObject butonNextSentence;
    public bool talking;
    void Start()
    {
        sentences = new Queue<string>();
        bgDialogue.SetActive(false);
        if(eventSystem == null)
        {
            eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        }
    } 
    public void StartDialogue(Dialogue dialogue)
    {
        talking = true;
        bgDialogue.SetActive(true);
        eventSystem.SetSelectedGameObject(butonNextSentence);
        sentences.Clear();

        foreach(string sentece in dialogue.sentences)
        {
            sentences.Enqueue(sentece);
        }
        nameText.text = dialogue.name;

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueTextGO.text = sentence;
    }


    void EndDialogue()
    {
        talking = false;
        if(ScenesManager.scenesManager.actualScene == 4) //HABLANDO CON EL ASKA
        {
            FindObjectOfType<Askafroa>().isSleeping = true;
            ScenesManager.scenesManager.FirstTalkAska = true;
            SaveSystem.SaveSceneData(ScenesManager.scenesManager);
        }
        bgDialogue.SetActive(false);
        FindObjectOfType<PlayerController2>().isOnKinematic = false;
    }
}
