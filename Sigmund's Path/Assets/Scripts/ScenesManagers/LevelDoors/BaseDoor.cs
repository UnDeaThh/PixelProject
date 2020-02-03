using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDoor : MonoBehaviour
{
    [HideInInspector] public GameManager GM;
    public int sceneToLoad;

    private void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log(sceneToLoad);
            //GM.ChangeScene(sceneToLoad);
        }
    }
}
