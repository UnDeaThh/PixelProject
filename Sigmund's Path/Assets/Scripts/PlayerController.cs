using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //REFERENCIAS
    private Rigidbody2D rb2d;
    //MOVIMIENTO HORIZONTAL
    public float movSpeed;
    private int facingRight;
    private float movDir;
    //JUMP
    public float jumpForce;
    private int maxJumps = 1;
    private int jumpsLeft;
    private bool isGrounded;

    public Transform feetPos;
    private float checkRadius = 0.2f;
    public LayerMask whatIsGrounded;
    private bool canJump;
    //DASH
    public float dashSpeed;
    private bool canDash;
    private bool isDashing;
    private bool stopDashing = false;
    public float timeTillNextDash = 3f;
    private float timeDashing;
    public float dashDuration;
    private float dashDir;
    void Awake(){
        rb2d = GetComponent<Rigidbody2D>();
        jumpsLeft = maxJumps;
        timeDashing = dashDuration;
    }
    void Update(){
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGrounded);
        CheckIfCanJump();
        CheckIfCanDash();
        PlayerInput();
        Flip();
        
        
    }
    void FixedUpdate(){
        rb2d.velocity = new Vector2(movDir * movSpeed, rb2d.velocity.y);
        Jump();
        Dash();
        HardFallingDown();
    }

    void PlayerInput(){
        movDir = Input.GetAxisRaw("Horizontal");
    }
    void Flip(){
        if(movDir > 0){
             transform.eulerAngles = new Vector3(0f, 0f, 0f);
             facingRight = 1;
        }
        else if(movDir < 0){
             transform.eulerAngles = new Vector3(0f, -180f, 0f);
             facingRight = -1;
        }
    }

    void CheckIfCanJump(){
        if(isGrounded && rb2d.velocity.y <= 0){
            jumpsLeft = maxJumps;
        }
        if(jumpsLeft <= 0){
            canJump = false;
        }
        else{
            canJump = true;
        }
    }
    void Jump(){
        if(Input.GetButtonDown("Jump")){
            if(canJump){
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
                jumpsLeft --;
            }
        }
    }
    void HardFallingDown(){
        if(jumpsLeft <= 0 && Input.GetKeyDown(KeyCode.S)){
            Debug.Log("Hard");
        }
    }

    void CheckIfCanDash(){
        if(isGrounded){
            canDash = true;
        }
        else {
            canDash = false;
        }
    }
    
    void Dash(){
        if(canDash){
            if(Input.GetKeyDown(KeyCode.LeftShift) && !stopDashing){
                isDashing = true;
                dashDir = facingRight;
            }
            if(isDashing && timeDashing >= 0f){
                timeDashing -= Time.fixedDeltaTime;
                rb2d.velocity = new Vector2(dashDir * dashSpeed, rb2d.velocity.y);
            }
            else{
                isDashing = false;
                timeDashing = dashDuration;
            }
            if(timeDashing < 0){
                StartCoroutine(GoNextDash());
            }
        }
    }

    IEnumerator GoNextDash(){
        stopDashing = true;
        yield return new WaitForSeconds(timeTillNextDash);
        stopDashing = false;
    }

    private void OnDrawGizmos(){
        Gizmos.DrawWireSphere(feetPos.position, checkRadius);
    }
}
