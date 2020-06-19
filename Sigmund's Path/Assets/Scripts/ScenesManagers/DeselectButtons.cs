using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DeselectButtons : MonoBehaviour
{
    [SerializeField] List<Button> buttons = new List<Button>();
    private EventSystem eventSystem;
    [SerializeField] GameObject firstSelected;
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent<Button>(out Button boton))
            {
                buttons.Add(boton);
            }
        }

        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();

        if(ScenesManager.scenesManager.Gamepad != null)
        {
            eventSystem.SetSelectedGameObject(firstSelected);
            Debug.Log("asas");
        }
        else
        {
            eventSystem.SetSelectedGameObject(null);
        }
    }

    private void Update()
    {
        InputSystem.onDeviceChange +=
            (device, change) =>
            {
                switch (change)
                {
                    case InputDeviceChange.Added:
                        eventSystem.SetSelectedGameObject(firstSelected);
                        break;
                    case InputDeviceChange.Removed:
                        eventSystem.SetSelectedGameObject(null);
                        break;
                }
            };
    }
}
