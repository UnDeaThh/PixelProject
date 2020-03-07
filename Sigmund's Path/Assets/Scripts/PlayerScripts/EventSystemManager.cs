using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemManager : MonoBehaviour
{
    [SerializeField] EventSystem eventSystem;

    private PauseManager pauseManager;
    public bool ffPause;
    [SerializeField] GameObject[] firstSelected = new GameObject[3];
    private void Awake()
    {
        pauseManager = GameObject.FindGameObjectWithTag("PauseManager").GetComponent<PauseManager>();
        if(eventSystem == null)
        {
            eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        }
    }

    private void Update()
    {
        if (pauseManager.isPaused)
        {
            SelectedButton();
        }
    }

    IEnumerator HighlightedFirstButton(int selected)
    {
        eventSystem.SetSelectedGameObject(null);
        yield return null;
        eventSystem.SetSelectedGameObject(firstSelected[selected]);
        ffPause = true; 
    }



    void SelectedButton()
    {
        if(pauseManager.isOnInventory && !ffPause)
        {
            StartCoroutine(HighlightedFirstButton(0));
            return;
        }
        if(pauseManager.isOnMap && !ffPause)
        {
            StartCoroutine(HighlightedFirstButton(1));
            return;
        }
        if(pauseManager.isOnSettings && !ffPause)
        {
            StartCoroutine(HighlightedFirstButton(2));
            return;
        }
    }
}
