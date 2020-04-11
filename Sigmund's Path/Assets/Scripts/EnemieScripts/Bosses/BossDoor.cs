using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour
{
    private bool closeDoor = false;

    [SerializeField] float toMove = 6f;

    private Vector2 doorClosePos;
    private Vector2 doorOpenPos;
    [SerializeField] float movSpeed = 0.1f;
    [SerializeField] GameObject doorGO;

    [SerializeField] int bossNumber;
    void Start()
    {
        if(doorGO == null)
        {
            doorGO = transform.GetChild(0).gameObject;
        }

        if (ScenesManager.scenesManager.BossKilled[bossNumber])
        {
            Destroy(gameObject);
        }

        doorOpenPos = doorGO.transform.position;
        doorClosePos = new Vector2( doorGO.transform.position.x, doorGO.transform.position.y + toMove);
    }

    void Update()
    {
        CloseDoorOnEnter();
        OpenDoorOnDefeatedBoss();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!ScenesManager.scenesManager.BossKilled[bossNumber])
            {
                closeDoor = true;
            }
        }
    }

    void CloseDoorOnEnter()
    {
        if (closeDoor)
        {
            if(doorGO.transform.position.y < doorClosePos.y)
            {
                doorGO.transform.position = Vector2.MoveTowards(doorGO.transform.position, doorClosePos, movSpeed);
            }
        }

    }

    void OpenDoorOnDefeatedBoss()
    {
        if (ScenesManager.scenesManager.BossKilled[bossNumber])
        {
            if(doorGO.transform.position.y > doorOpenPos.y)
            {
                doorGO.transform.position = Vector2.MoveTowards(doorGO.transform.position, doorOpenPos, movSpeed);
            }
        }
    }
}
