using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vendedor : MonoBehaviour
{
    public Canvas canvasVendedor;
    public GameObject pressEText;
    private PauseManager pauseManager;
    private PlayerController plController;
    private void Awake()
    {
        pauseManager = GameObject.FindGameObjectWithTag("PauseManager").GetComponent<PauseManager>();
        plController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        pressEText.SetActive(false);
    }

    private void Update()
    {
        if (pauseManager.isPaused)
        {
            canvasVendedor.enabled = false;
        }
        else
            canvasVendedor.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            pressEText.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(!pauseManager.isPaused)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    print("Enter");
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            pressEText.SetActive(false);
        }
    }
}
