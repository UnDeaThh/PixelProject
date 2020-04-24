using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;
    [SerializeField] GameObject bgDialogue;
    [SerializeField] TextMeshProUGUI dialogueTextGO;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] EventSystem eventSystem;
    [SerializeField] GameObject butonNextSentence;
    private PlayerController2 player;
    public bool talking;
    void Start()
    {
        sentences = new Queue<string>();
        bgDialogue.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController2>();
        if(eventSystem == null)
        {
            eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        }
    }
    private void Update()
    {
        if (talking)
        {
            eventSystem.SetSelectedGameObject(butonNextSentence);
        }
    }
    public void StartDialogue(Dialogue dialogue)
    {
        player.isOnKinematic = true;
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
        if(SceneManager.GetActiveScene().name == "T2") //HABLANDO CON EL ASKA
        {
            FindObjectOfType<Askafroa>().isSleeping = true;
            ScenesManager.scenesManager.FirstTalkAska = true;
            SaveSystem.SaveSceneData(ScenesManager.scenesManager);
        }
        else if(SceneManager.GetActiveScene().name == "NerbuzScene")
        {
            FindObjectOfType<NerbuzAI>().ActualState = State.Enter;
        }

        bgDialogue.SetActive(false);
        player.isOnKinematic = false;
    }
}
