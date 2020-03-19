using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public bool isTalking;
    bool oneTime = false;

    private void Update()
    {
        if(dialogue.name == "Askafroa")
        {
            if (isTalking && !oneTime)
            {
                TriggerDialogue();
                oneTime = true;
            }
        }
    }
    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
