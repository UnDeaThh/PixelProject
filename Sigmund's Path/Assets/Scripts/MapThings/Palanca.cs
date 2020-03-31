using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Palanca : MonoBehaviour
{
    public bool isOpen = false;
    public bool isDoorDown;
    [SerializeField] GameObject door;
    [SerializeField] float openSpeed;
    [SerializeField] float descend;
    private Vector2 doorFinalPos;
    private SpriteRenderer spriteRenderer;
    [SerializeField] Sprite spriteOpenPalanca;
    [SerializeField] int numberOfPalanca;
    private Animator anim;
    private void Start()
    {
        doorFinalPos = new Vector2(door.transform.position.x, transform.position.y - descend);
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        isOpen = ScenesManager.scenesManager.palancasState[numberOfPalanca];
        if (isOpen)
        {
            isDoorDown = true;
            Destroy(anim);
            spriteRenderer.sprite = spriteOpenPalanca;
            Destroy(door);
            Collider2D col = GetComponent<Collider2D>();
            col.enabled = false;
        }
    }
    void Update()
    {
        BajandoPuerta();
    }
    void BajandoPuerta()
    {
        if (isOpen && !isDoorDown)
        {
            if (door != null)
            {
                door.transform.position = Vector2.MoveTowards(door.transform.position, doorFinalPos, openSpeed);
                if (door.transform.position.y <= doorFinalPos.y)
                {
                    isDoorDown = true;
                    Destroy(door);
                }
            }
        }
    }

    public void OpenDoor()
    {
        if (!isOpen)
        {
            if(door != null)
            {
                anim.SetTrigger("PalancaActivated");
                isOpen = true;
                ScenesManager.scenesManager.palancasState[numberOfPalanca] = true;
                SaveSystem.SaveSceneData(ScenesManager.scenesManager);
            }
        }
    }
}
