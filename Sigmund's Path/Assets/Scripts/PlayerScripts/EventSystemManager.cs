using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemManager : MonoBehaviour
{
    [SerializeField] EventSystem eventSystem;

    private PauseManager pauseManager;
    private bool ffPause;
    private void Awake()
    {
        pauseManager = GameObject.FindGameObjectWithTag("PauseManager").GetComponent<PauseManager>();
    }

    private void Update()
    {
        if(pauseManager.isPaused && !ffPause)
        {
            StartCoroutine(HighlightedFirstButton());
        }
    }

    IEnumerator HighlightedFirstButton()
    {
        eventSystem.SetSelectedGameObject(null);
        yield return null;
        eventSystem.SetSelectedGameObject(eventSystem.firstSelectedGameObject);
        ffPause = true;
    }
}
