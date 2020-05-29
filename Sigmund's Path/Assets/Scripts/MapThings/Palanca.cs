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
    [SerializeField] AudioSource openSound;
    [SerializeField] AudioSource palancaSound;
    [SerializeField] ParticleSystem moveGroundParticle;
    [SerializeField] ParticleSystem polvoParticle;
    private bool hasSounded = false;
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

        isOpen = ScenesManager.scenesManager.PalancasState[numberOfPalanca];
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
        MoviendoPuerta();
    }
    void MoviendoPuerta()
    {
        if (isOpen && !isDoorDown)
        {
            if (!hasSounded)
            {
                openSound.Play();
                hasSounded = true;
            }
            if (door != null)
            {
                door.transform.position = Vector2.MoveTowards(door.transform.position, doorFinalPos, openSpeed);
                if(horientacion == Horientacion.Vertical)
                {
                    if (door.transform.position.y <= doorFinalPos.y)
                    {
                        isDoorDown = true;
                        moveGroundParticle.Stop();
                        polvoParticle.Stop();
                        Destroy(door);
                    }
                    else
                    {
                        if (!moveGroundParticle.isEmitting)
                        {
                            moveGroundParticle.Play();
                            polvoParticle.Play();
                        }
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
                palancaSound.Play();
                isOpen = true;
                Debug.Log("Save");
                ScenesManager.scenesManager.PalancasState[numberOfPalanca] = true;
                SaveSystem.SaveSceneData(ScenesManager.scenesManager);
            }
        }
    }
}
