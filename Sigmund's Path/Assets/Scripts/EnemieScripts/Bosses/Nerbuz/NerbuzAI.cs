using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NerbuzAI : BossBase
{

    private State actualState;

    public State ActualState { get => actualState; set => actualState = value; }

    void Start()
    {
        actualState = State.Nothing;
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        mat = sprite.material;
        col = GetComponent<Collider2D>();
    }
    void Update()
    {
        switch (actualState)
        {
            case State.Enter:
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
    }
}
