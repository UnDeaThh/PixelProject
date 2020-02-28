using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Palanca : MonoBehaviour
{
    public bool isOpen = false;
    public bool alreadyOpen;
    [SerializeField] GameObject door;
    [SerializeField] float openSpeed;
    [SerializeField] float descend;
    private Vector2 doorFinalPos;
    private SpriteRenderer spriteRenderer;
    [SerializeField] Sprite spriteOpenPalanca;

    // Update is called once per frame

    private void Start()
    {
        doorFinalPos = new Vector2(door.transform.position.x, transform.position.y - descend);
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (alreadyOpen)
        {
            spriteRenderer.sprite = spriteOpenPalanca;
            Collider2D col = GetComponent<Collider2D>();
            col.enabled = false;
        }
    }
    void Update()
    {
        if (isOpen && !alreadyOpen)
        {
            if(door != null)
            {
                door.transform.position = Vector2.MoveTowards(door.transform.position, doorFinalPos, openSpeed);
                if(door.transform.position.y <= doorFinalPos.y)
                {
                    alreadyOpen = true;
                    Destroy(door);
                }      
            }
        }
    }
}
