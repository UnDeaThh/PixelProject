using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParry : MonoBehaviour
{
    private float timeBtwParry;
    public float startTimeBtwParry = 0.1f;
    public float parryDuration = 1f;

    private bool canParry;
    [HideInInspector] public bool isParry;

    public Collider2D parryCol;
    private PlayerController plController;

    private void Awake()
    {
        plController = GetComponent<PlayerController>();
        isParry = false;
        parryCol.enabled = false;
    }
    private void Update()
    {
        CheckIfCanParry();
        Parry();
    }

    void CheckIfCanParry()
    {
        if(timeBtwParry <= 0 && plController.isGrounded == true)
        {
            canParry = true;
        }
        else 
        {
            timeBtwParry -= Time.deltaTime;
            canParry = false;
        }
    }

    void Parry()
    {
        if (canParry)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                isParry = true;
                StartCoroutine(DoingParry());
            }
        }

    }

    IEnumerator DoingParry()
    {
        parryCol.enabled = true;
        yield return new WaitForSeconds(parryDuration);
        parryCol.enabled = false;
        isParry = false;
        timeBtwParry = startTimeBtwParry;
    }
}
