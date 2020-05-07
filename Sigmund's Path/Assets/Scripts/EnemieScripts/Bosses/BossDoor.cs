using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour
{
    private bool closeDoor = false;

    [SerializeField] float toMoveDoor1 = 6f;
    [SerializeField] float toMoveAltarDoor = 7;

    private Vector2 doorClosePos;
    private Vector2 doorOpenPos;
    private Vector2 doorAltarOpenPos;
    [SerializeField] float movSpeed = 0.1f;
    [SerializeField] GameObject doorGO;
    [SerializeField] GameObject doorAltar;

    [SerializeField] int bossNumber;
    [SerializeField] GameObject theBoss;
    void Start()
    {
        if(doorGO == null)
        {
            doorGO = transform.GetChild(0).gameObject;
        }
        bossNumber = bossNumber - 1;
        if (ScenesManager.scenesManager.BossKilled[bossNumber])
        {
            Destroy(gameObject);
        }

        doorOpenPos = doorGO.transform.position;
        doorClosePos = new Vector2( doorGO.transform.position.x, doorGO.transform.position.y + toMoveDoor1);

        doorAltarOpenPos = new Vector2(doorAltar.transform.position.x, doorAltar.transform.position.y - toMoveAltarDoor);
    }

    void Update()
    {
        DoorLogic();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!ScenesManager.scenesManager.BossKilled[bossNumber])
            {
                closeDoor = true;
                GetComponent<Collider2D>().enabled = false;
                if (theBoss)
                {
                    if (theBoss.TryGetComponent(out BossChange boss))
                    {
                        boss.ActualState = State.Enter;
                    }
                }
            }
        }
    }

    void DoorLogic()
    {
        if (!ScenesManager.scenesManager.BossKilled[bossNumber])
        {
            if (closeDoor)
            {
                if(doorGO.transform.position.y < doorClosePos.y)
                {
                    doorGO.transform.position = Vector2.MoveTowards(doorGO.transform.position, doorClosePos, movSpeed);
                }
            }
        }
        else
        {
            if (doorGO.transform.position.y > doorOpenPos.y)
            {
                doorGO.transform.position = Vector2.MoveTowards(doorGO.transform.position, doorOpenPos, movSpeed);
            }
            if (doorAltar)
            {
                if(doorAltar.transform.position.y > doorAltarOpenPos.y)
                {
                    doorAltar.transform.position = Vector2.MoveTowards(doorAltar.transform.position, doorAltarOpenPos, movSpeed);
                }
            }
        }
    }
}
