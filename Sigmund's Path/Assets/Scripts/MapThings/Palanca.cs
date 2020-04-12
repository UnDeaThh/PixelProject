using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Palanca : MonoBehaviour
{
    private enum Horientacion
    {
        Vertical, Horizontal,
    }

    [SerializeField] Horientacion horientacion;
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
        if(horientacion == Horientacion.Vertical)
        {
            doorFinalPos = new Vector2(door.transform.position.x, door.transform.position.y - descend);
        }
        else if(horientacion == Horientacion.Horizontal)
        {
            doorFinalPos = new Vector2(door.transform.position.x + descend, door.transform.position.y);
        }
        
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
                if(horientacion == Horientacion.Vertical)
                {
                    if (door.transform.position.y <= doorFinalPos.y)
                    {
                        isDoorDown = true;
                        Destroy(door);
                    }
                }
                else if(horientacion == Horientacion.Horizontal)
                {
                    if(door.transform.position.x >= doorFinalPos.x)
                    {
                        isDoorDown = true;
                        Destroy(door);
                    }
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
