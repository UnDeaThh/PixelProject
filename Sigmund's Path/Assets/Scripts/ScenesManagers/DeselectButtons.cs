using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DeselectButtons : MonoBehaviour
{
    private EventSystem eventSystem;
    [SerializeField] GameObject firstSelected;
    void Start()
    {
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();

        if(ScenesManager.scenesManager.Gamepad != null)
        {
            eventSystem.SetSelectedGameObject(firstSelected);
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
