using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //REFERENCIAS
    private Rigidbody2D rb2d;
    //MOVIMIENTO HORIZONTAL
    public float movSpeed;
    private int facingRight = 1;
    private float movInputDir;
    public float movementForceInAir;
    //JUMP
    public float jumpForce;
    private int maxJumps = 1;
    private int jumpsLeft;
    private bool isGrounded;

    public Transform feetPos;
    private float checkRadius = 0.2f;
    public LayerMask whatIsGrounded;
    private bool canJump;
    //WALLJUMP
    private Vector2 wallHopDir = new Vector2(1, 1f);
    private Vector2 wallJumpDir = new Vector2(1, 2);
    public float wallHopForce;
    public float wallJumpForce;
    public float lowWallJumpForce;
    private int facingDir = 1;

    //HARDFALLING DOWN
    public float downForce;

    //DASH
    public float dashSpeed;
    private bool canDash;
    private bool isDashing;
    private bool stopDashing = false;
    public float timeTillNextDash = 3f;
    private float timeDashing;
    public float dashDuration;
    private float dashDir;

    //WALL SLIDING
    public Transform wallCheckPos;
    private float wallCheckDistance = 0.2f;
    public float wallSlideSpeed;
    private bool isTouchingWall;
    private bool isWallSliding;
    public bool wasWallSliding;

    void Awake(){
        wallHopDir.Normalize();
        wallJumpDir.Normalize();
        rb2d = GetComponent<Rigidbody2D>();
        jumpsLeft = maxJumps;
        timeDashing = dashDuration;
    }
    void Update(){
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGrounded);
        isTouchingWall = Physics2D.Raycast(wallCheckPos.position, transform.right, wallCheckDistance, whatIsGrounded); 
        CheckIfWallSliding();
        CheckIfCanJump();
        CheckIfCanDash();
        PlayerInput();
        CheckMovement();
        
    }
    void FixedUpdate(){
        ApplyMovement();
        Jump();
        LimitVelocity();
        Dash();
        HardFallingDown();
        IsWallSliding();
    }

    void PlayerInput(){
        movInputDir = Input.GetAxisRaw("Horizontal");
    }

    void CheckMovement()
    {
        if(facingRight == 1 && movInputDir < 0)
        {
            Flip();
        }
        else if (facingRight == -1 && movInputDir > 0)
        {
            Flip();
        }
    }

    void ApplyMovement()
    {
        if (isGrounded)
        {
            rb2d.velocity = new Vector2(movInputDir * movSpeed, rb2d.velocity.y);
            wasWallSliding = false;
        }

        //FLOTANDO
        else if(!isGrounded && !isWallSliding && movInputDir != 0)
        {
            Vector2 forceToAdd = new Vector2(movementForceInAir * movInputDir, 0);
            rb2d.AddForce(forceToAdd);

            if(Mathf.Abs(rb2d.velocity.x) > movSpeed)
            {
                rb2d.velocity = new Vector2(movSpeed * movInputDir, rb2d.velocity.y);
            }
        }
    }

    void LimitVelocity()
    {
        if (wasWallSliding)
        {
            if(rb2d.velocity.y > 15f)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, 15f);
            }
            else if(rb2d.velocity.y < -15f)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, -15f);
            }
        }
    }
    void Flip(){
        if (!isWallSliding)
        {
            facingDir *= -1;
            facingRight *= -1;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }

    void CheckIfCanJump()
    {
        if(isGrounded && rb2d.velocity.y <= 0)
        {
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
        if(Input.GetButtonDown("Jump"))
        {
            if(canJump){
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
                jumpsLeft --;
            }
            //WALLHOP
             if (isWallSliding && movInputDir == 0f)
            {
                isWallSliding = false;
                Vector2 forceToAdd = new Vector2(wallHopForce * wallHopDir.x * -facingDir, wallHopForce * wallHopDir.y);
                rb2d.AddForce(forceToAdd, ForceMode2D.Impulse);
                Flip();
            }
             //WALLJUMP
             else if (isWallSliding)
            {
                if (facingDir == movInputDir)
                {
                    //salto corto
                    Debug.Log("SaltoCorto");
                    isWallSliding = false;
                    Vector2 forceToAdd = new Vector2(lowWallJumpForce * wallJumpDir.x * movInputDir, lowWallJumpForce * wallJumpDir.y);
                    rb2d.AddForce(forceToAdd, ForceMode2D.Impulse);
                }
                else if(facingDir != movInputDir)
                {
                    //salto largo
                    isWallSliding = false;
                    Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDir.x * movInputDir, wallJumpForce * wallJumpDir.y);
                    rb2d.AddForce(forceToAdd, ForceMode2D.Impulse);
                    Flip();
                }
               
            }
        }
    }

    void HardFallingDown()
        {
            if(jumpsLeft <= 0 && Input.GetKeyDown(KeyCode.S) && !isGrounded){
                rb2d.velocity = (Vector2.down * downForce);
        }
    }

    void CheckIfCanDash(){
        if(isGrounded)
        {
            canDash = true;
        }
        else
        {
            canDash = false;
        }
    }
    
    void Dash(){
        if(canDash)
        {
            if(Input.GetKeyDown(KeyCode.LeftShift) && !stopDashing)
            {
                isDashing = true;
                dashDir = facingRight;
            }
            if(isDashing && timeDashing >= 0f)
            {
                timeDashing -= Time.fixedDeltaTime;
                rb2d.velocity = new Vector2(dashDir * dashSpeed, rb2d.velocity.y);
            }
            else
            {
                isDashing = false;
                timeDashing = dashDuration;
            }
            if(timeDashing < 0)
            {
                StartCoroutine(GoNextDash());
            }
        }
    }

    IEnumerator GoNextDash()
    {
        stopDashing = true;
        yield return new WaitForSeconds(timeTillNextDash);
        stopDashing = false;
    }

    void CheckIfWallSliding()
    {
        if(isTouchingWall && !isGrounded)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
    }

    void IsWallSliding()
    {
        if(isWallSliding)
        {
            wasWallSliding = true;
            if(rb2d.velocity.y < 0f && facingRight == 1 && movInputDir > 0f)
            {
                rb2d.velocity = new Vector2(0f, rb2d.velocity.y);
                rb2d.velocity = new Vector2(rb2d.velocity.x, -wallSlideSpeed);
            }
            else if(rb2d.velocity.y < 0 && facingRight == -1 && movInputDir < 0f)
            {
                rb2d.velocity = new Vector2(0f, rb2d.velocity.y);
                rb2d.velocity = new Vector2(rb2d.velocity.x, -wallSlideSpeed);
            }
        }
    }

    private void OnDrawGizmos(){
        Gizmos.DrawWireSphere(feetPos.position, checkRadius);
        Gizmos.DrawLine(wallCheckPos.position, new Vector3(wallCheckPos.position.x + wallCheckDistance, wallCheckPos.position.y, wallCheckPos.position.z));
    }
}
