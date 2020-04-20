using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChange : BossBase
{
    private State actualState;
    public State ActualState { get => actualState; set => actualState = value; }

    [SerializeField] AudioClip[] clips;

    bool hasSounded = false;

    private Vector2 movDir;
    [SerializeField] private float movSpeed = 100;


    private void Start()
    {
        actualState = State.Nothing;
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        mat = sprite.material;
        col = GetComponent<Collider2D>();
        audioSource = GetComponentInChildren<AudioSource>();
        movDir = new Vector2(1, -2);
    }

    private void Update()
    {
        movDir.Normalize();

        switch (actualState)
        {
          
            case State.Enter:
                if (!audioSource.isPlaying && !hasSounded)
                {
                    CameraController.cameraController.IsGenerateCS = true;
                    CameraController.cameraController.GenerateCamerashake(8, 3, clips[0].length - 0.3f);
                    audioSource.clip = clips[0];
                    audioSource.Play();
                    hasSounded = true;
                }
                if(!audioSource.isPlaying && hasSounded)
                {
                    actualState = State.H1;
                }
                break;
            case State.H1:
                break;
            case State.H2:
                break;
            case State.H3:
                break;
            case State.H4:
                break;
            case State.Transition:
                break;
            case State.Dead:
                break;
            case State.Nothing:
                break;
            default:
                break;
        }

        Dead();
    }
    private void FixedUpdate()
    {
        switch (actualState)
        {
            case State.Enter:
                break;
            case State.H1:
                rb.velocity = movDir * movSpeed * Time.deltaTime;
                break;
            case State.H2:
                break;
            case State.H3:
                break;
            case State.H4:
                break;
            case State.Transition:
                break;
            case State.Dead:
                break;
            case State.Nothing:
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (actualState == State.H1)
        {
            if (other.transform.CompareTag("Floor"))
            {
                if (other.transform.name == "LimitDown")
                {
                    movDir.y *= -1;
                }
                else if (other.transform.name == "LimitUp")
                {
                    movDir.y *= -1;
                }
                else if (other.transform.name == "LimitLeft")
                {
                    movDir.x *= -1;
                }
                else if (other.transform.name == "LimitRight")
                {
                    movDir.x *= -1;
                }
            }
        }
    }
}
