using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //REFERENCIAS
    private Rigidbody2D rb2d;
    private PlayerParry plParry;
    private Animator anim;
    [SerializeField] private SpriteRenderer sprite;

    //MOVIMIENTO HORIZONTAL
    public float movSpeed;
    [HideInInspector] public int facingRight = 1;
    private float movInputDir;
    public float movementForceInAir;
    //JUMP
    public float jumpForce;
    private int maxJumps = 1;
    private int jumpsLeft;
    [HideInInspector] public bool isGrounded;

    public Transform feetPos;
    private float checkRadius = 0.25f;
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
    private bool wasWallSliding;

    //LIFE
    public int health = 5;
    private int maxHealth = 10;
    public Image[] heartsUI;
    public Sprite fullHeartUI;
    public Sprite emptyHeartUI;

    //DAMAGED
    private bool damaged = false;
    private bool invecibility = false;
    private float invencibleTime;
    public float startInvencibleTime;
    public int damagedPushForce;

    //DAMAGE FORCE
    private int damageX = 0;
    private int damageY = 0;




    void Awake(){
        rb2d = GetComponent<Rigidbody2D>();
        plParry = GetComponent<PlayerParry>();
        anim = GetComponentInChildren<Animator>();

        wallHopDir.Normalize();
        wallJumpDir.Normalize();
        jumpsLeft = maxJumps;
        timeDashing = dashDuration;
    }
    void Update(){
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGrounded);
        isTouchingWall = Physics2D.Raycast(wallCheckPos.position, transform.right, wallCheckDistance, whatIsGrounded);
        CheckLife();
        CheckIfWallSliding();
        CheckIfCanJump();
        CheckIfCanDash();
        PlayerInput();
        CheckMovement();
        Invencibility();


        UpdateAnimations();
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

    void CheckLife()
    {
        if(health > maxHealth)
        {
            health = maxHealth;
        }

        for (int i = 0; i < heartsUI.Length; i++)
        {
            if(i < health)
            {
                heartsUI[i].sprite = fullHeartUI;
            }
            else
            {
                heartsUI[i].sprite = emptyHeartUI;
            }

            if(i < maxHealth)
            {
                heartsUI[i].enabled = true;
            }
            else
            {
                heartsUI[i].enabled = false;
            }
        }
    }

    void CheckMovement()
    {
        if(facingRight == 1 && movInputDir < 0 && plParry.isParry == false)
        {
            Flip();
        }
        else if (facingRight == -1 && movInputDir > 0 && plParry.isParry == false)
        {
            Flip();
        }
    }

    void ApplyMovement()
    {
        Debug.Log(damageX);
        if (isGrounded && !plParry.isParry && !plParry.parryFail && !damaged)
        {
            rb2d.velocity = new Vector2(movInputDir * movSpeed + damageX, rb2d.velocity.y + damageY);
            wasWallSliding = false;
        }
        else if((isGrounded || !isGrounded) && !plParry.isParry && !plParry.parryFail && damaged)
        {
            rb2d.velocity = Vector2.zero;
            rb2d.velocity = new Vector2(damageX, damageY);
            damaged = false;
        }
        else if (isGrounded && plParry.isParry == true) 
        {
            rb2d.velocity = Vector2.zero;
            wasWallSliding = false;
        }

        if (damageX < 0)
        {
            damageX++;
        }
        else if (damageX > 0)
        {
            damageX--;
        }

        if(damageY < 0)
        {
            damageY++;
        }
        else if (damageY > 0)
        {
            damageY--;
        }

        //SEMI-CONTROL EN EL AIRE
        else if (!isGrounded && !isWallSliding)
        {
            Vector2 forceToAdd = new Vector2(movementForceInAir * movInputDir, 0);
            rb2d.AddForce(forceToAdd);

            if (Mathf.Abs(rb2d.velocity.x) > movSpeed)
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
             else if (isWallSliding || (isTouchingWall && !isGrounded))
            {
                if (facingDir == movInputDir)
                {
                    //salto corto
                    isWallSliding = false;
                    Vector2 forceToAdd = new Vector2(lowWallJumpForce * wallJumpDir.x * -movInputDir, lowWallJumpForce * wallJumpDir.y);
                    rb2d.AddForce(forceToAdd, ForceMode2D.Impulse);
                    Flip();                }
                else if(facingDir != movInputDir)
                {
                    //salto largo
                    isWallSliding = false;
                    Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDir.x * movInputDir, wallJumpForce * wallJumpDir.y);
                    rb2d.AddForce(forceToAdd, ForceMode2D.Impulse);
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
                Physics2D.IgnoreLayerCollision(9, 10);
            }
            else
            {
                isDashing = false;
                timeDashing = dashDuration;
                
            }
            if(timeDashing < 0)
            {
                Physics2D.IgnoreLayerCollision(9, 10, false);
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
            if(rb2d.velocity.y < 0f && facingRight == 1 && movInputDir > 0f) //Pared Derecha
            {
                rb2d.velocity = new Vector2(0f, rb2d.velocity.y);
                rb2d.velocity = new Vector2(rb2d.velocity.x, -wallSlideSpeed);
            }
            else if(rb2d.velocity.y < 0 && facingRight == -1 && movInputDir < 0f) //Pared Izquierda
            {
                rb2d.velocity = new Vector2(0f, rb2d.velocity.y);
                rb2d.velocity = new Vector2(rb2d.velocity.x, -wallSlideSpeed);
            }
        }
    }

    public void Damaged(int damage, Vector2 normal)
    {
        if (!invecibility)
        {
            int normalX = (int)normal.x;
            int normalY = (int)normal.y;
            invecibility = true;
            damaged = true;
            invencibleTime = startInvencibleTime;
            health -= damage;

            if (isGrounded)
            {
                damageX = -facingRight * damagedPushForce;
                damageY = damagedPushForce;
            }
            else if (!isGrounded)
            {
                Debug.Log("HIT" + normalX);
                damageX = -normalX * damagedPushForce;
                damageY = -normalY * damagedPushForce;
            }
            Debug.Log(health);
            StartCoroutine(Blinking());
            
           //Lanzar sonido
           //Lanzar particulas
           //Efecto de camara

        }
    }

    IEnumerator Blinking()
    {
        for(int i = 0; i < 3; i++)
        {
            sprite.color = Color.gray;
            yield return new WaitForSeconds(0.1f);
            sprite.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void Invencibility()
    {
        if (invecibility)
        {
            invencibleTime -= Time.deltaTime;
            Physics2D.IgnoreLayerCollision(9, 10);
            if(invencibleTime <= 0)
            {
                Physics2D.IgnoreLayerCollision(9, 10, false);
                invecibility = false;
            }
        }

    }

    private void UpdateAnimations()
    {
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("speedX", Mathf.Abs(rb2d.velocity.x));
    }


    private void OnDrawGizmos(){
        Gizmos.DrawWireSphere(feetPos.position, checkRadius);
        Gizmos.DrawLine(wallCheckPos.position, new Vector3(wallCheckPos.position.x + wallCheckDistance, wallCheckPos.position.y, wallCheckPos.position.z));
    }
}
