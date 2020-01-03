using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //REFERENCIAS
    private Rigidbody2D rb2d;
    private PlayerParry plParry;
    private PlayerAttack plAttack;
    private Animator anim;
    private PauseManager PM;
    private CameraController cameraController;
    [SerializeField] private SpriteRenderer sprite;

    //MOVIMIENTO HORIZONTAL
    [Header("Movement Attributes")]
    public float movSpeed;
    public int facingRight = 1;
    private float movInputDir;
    public float movementForceInAir;
    //JUMP
    [Header("Jump Attributes")]
    public float jumpForce;
    private int maxJumps = 1;
    private bool jumpPressed = false;
    private bool isJumping = false;
    private int jumpsLeft;
    private RaycastHit2D ledgeCheck;
    private RaycastHit2D underLedgeCheck;

    private bool groundCheckRight;
    private bool groundCheckLeft;
    [HideInInspector] public bool isGrounded;
    public Transform groundCheckLeftPos;
    public Transform groundCheckRightPos;
    public float groundCheckDistance = 0.25f;
    public LayerMask whatIsGrounded;
    private bool canJump;
    //WALLJUMP
    [Header("WallJump Attributes")]
    private Vector2 wallHopDir = new Vector2(1, 1f);
    private Vector2 wallJumpDir = new Vector2(1, 2);
    [HideInInspector] public bool isWallJump = false;
    public float wallHopForce;
    public float wallJumpForce;
    public float lowWallJumpForce;
    private int facingDir = 1;

    //HARDFALLING DOWN
    public float downForce;

    //DASH
    [Header("Dash Attributes")]
    public float dashSpeed;
    private bool canDash;
    private bool isDashing;
    private bool stopDashing = false;
    public float timeTillNextDash = 3f;
    private float timeDashing;
    public float dashDuration;
    private float dashDir;

    //LEDGE GRAB
    [Header("Edge Grab")]
    public bool isHanging = false;
    public float hangDistance = 0.4f;
    public float hangJumpsForce = 100f;
    public Transform ledgeCheckPos;
    public Transform underLedgeCheckPos;

    //WALL SLIDING
    [Header("WallSliding Attributes")]
    public Transform wallCheckPos;
    public float wallCheckDistance = 0.2f;
    public float wallSlideSpeed;
    private bool isTouchingWall;
    public bool isWallSliding;
    private bool isWallSlidingAnim;
    private bool wasWallSliding;

    //LIFE
    [Header("Health Attributes")]
    public int health = 5;
    [HideInInspector] public bool isDead = false;
    [HideInInspector] public int maxHealth = 10;
    public Image[] heartsUI;
    public Sprite fullHeartUI;
    public Sprite emptyHeartUI;
    //POTIONS
    [Header("Potions Attributes")]
    public int potions = 1;
    private int maxPotions = 5;
    public Image[] potionsUI;
    private bool canDrink = true;
    [HideInInspector] public bool isDrinking = false;
    private float timeDrinking = 1f;
    private float currentTimeDrinking;

    public float timeTillNextDrink = 2f;
    private float currentTimeTillNextDrink;

    //DAMAGED
    [Header("Damaged Attributes")]
    private bool isDamaged = false;
    private bool invecibility = false;
    private float invencibleTime;
    public float startInvencibleTime;
    public int damagedPushForce;

    private float damageX = 0;
    private float damageY = 0;

    const float smallAmount = 0.05f;


    void Awake(){
        rb2d = GetComponent<Rigidbody2D>();
        plParry = GetComponent<PlayerParry>();
        plAttack = GetComponent<PlayerAttack>();
        anim = GetComponentInChildren<Animator>();
        PM = GameObject.FindGameObjectWithTag("PauseManager").GetComponent<PauseManager>();
        cameraController = GameObject.FindGameObjectWithTag("CameraManager").GetComponent<CameraController>();

        wallHopDir.Normalize();
        wallJumpDir.Normalize();
        jumpsLeft = maxJumps;
        timeDashing = dashDuration;
    }
    void Update(){
        CheckLife();
        if (!isDead)
        {
            if(!PM.isPaused)
            {

                CheckEnvironment();
                CheckIfWallSliding();
                CheckIfCanJump();
                CheckIfCanDash();
		        CheckIfCanDrink();
                PlayerInput();
                CheckMovement();
                Invencibility();
                CheckPotions();
                GrabLedgeWall();

                UpdateAnimations();
            }
        }
        else
            return;
        
    }
    void FixedUpdate(){
        ApplyMovement();
        Jump();
        LimitVelocity();
        Dash();
        GrabLedgeJump();
        HardFallingDown();
        IsWallSliding();
    }

    void CheckEnvironment(){
        ledgeCheck = Physics2D.Raycast(ledgeCheckPos.position, transform.right, hangDistance, whatIsGrounded);
        underLedgeCheck = Physics2D.Raycast(underLedgeCheckPos.position, transform.right, hangDistance, whatIsGrounded);
        isTouchingWall = Physics2D.Raycast(wallCheckPos.position, transform.right, wallCheckDistance, whatIsGrounded);
        groundCheckLeft = Physics2D.Raycast(groundCheckLeftPos.position, Vector2.down, groundCheckDistance, whatIsGrounded);
        groundCheckRight = Physics2D.Raycast(groundCheckLeftPos.position, Vector2.down, groundCheckDistance, whatIsGrounded);
        if(groundCheckLeft || groundCheckRight){
            isGrounded = true;
        }
        else
            isGrounded = false;
    }

    void PlayerInput(){
        if (!plParry.isParry)
        {
            if(!isHanging){
    		    movInputDir = Input.GetAxisRaw("Horizontal");
            }
		    if (Input.GetKeyDown(KeyCode.X))
		    {
			    DrinkPotion();
		    }
            if (Input.GetButtonDown("Jump") && !isJumping)
            {
                jumpPressed = true;
            } 
        }
		
    }

    void CheckLife()
    {
        if(health > 0)
        {
            isDead = false;
        }
        else
        {
            isDead = true;
        }
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

    private void CheckPotions()
    {
        if(potions > maxPotions)
        {
            potions = maxPotions;
        }

        for(int i = 0; i < potionsUI.Length; i++)
        {
            if(i < potions)
            {
                potionsUI[i].enabled = true;
            }
            else
            {
                potionsUI[i].enabled = false;
            }
        }
    }

	private void CheckIfCanDrink(){
		if(!isGrounded){
			canDrink = false;
		}

	    if(currentTimeTillNextDrink > 0 || !isGrounded)
        {
            canDrink = false;
            currentTimeTillNextDrink -= Time.deltaTime;
        }

        else if(currentTimeTillNextDrink <= 0 && isGrounded)
        {
            canDrink = true;
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
        if (!plParry.isParry)
        {
            if (!plParry.parryFail)
            {
                //Movimiento NORMAL en el suelo
                if (isGrounded && !isDamaged && !isDrinking)
                {
                    rb2d.velocity = new Vector2(movInputDir * movSpeed , rb2d.velocity.y);
                    wasWallSliding = false;
                }
                //Movimiento REDUCIDO en el aire
                else if (!isGrounded && !isWallSliding && !isDamaged)
                {
                    Vector2 forceToAdd = new Vector2(movementForceInAir * movInputDir, 0);
                    rb2d.AddForce(forceToAdd);
                    if (Mathf.Abs(rb2d.velocity.x) > movSpeed)
                    {
                        rb2d.velocity = new Vector2(movSpeed * movInputDir, rb2d.velocity.y);
                    }
                }
                //Movimiento REDUCIDO cuando te estas curando
                else if (isGrounded && !isDamaged && isDrinking)
                {
                    rb2d.velocity = new Vector2(movInputDir * movSpeed * 0.2f , rb2d.velocity.y);
                    wasWallSliding = false;
                }

                //Cuando te DAÑAN te empujan 1 FRAME
                else if(isDamaged)
                {
                    rb2d.velocity = Vector2.zero;
                    //( rb2d.velocity = new Vector2(damageX, damageY);  Old
                    rb2d.AddForce(new Vector2(damageX, damageY));
                    isDamaged = false;
                }
            }
            //Si hace parry y no impacta contra ningun enemigo te mueves mas lento
            else
            {
                rb2d.velocity = new Vector2(movInputDir * movSpeed * 0.7f, rb2d.velocity.y);
                wasWallSliding = false;
            }
        }
        //Cuando haces parry te quedas quieto
        else if(plParry.isParry)
        {
            rb2d.velocity = Vector2.zero;
            wasWallSliding = false;
        }

     /*   if (damageX < 0)   Old
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
        */

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

    void GrabLedgeWall(){
        if(!isGrounded && !isHanging && rb2d.velocity.y < 0f && !ledgeCheck && underLedgeCheck && isTouchingWall){
            //Guardamos la posicion actual
           // Vector3 pos = transform.position;
            // movemos hasta el muro
            //pos.x += (underLedgeCheck.distance - smallAmount) * facingDir;
            //movemos hasta la altura del muro
            //pos.y -= hangDistance;
            //aplicamos la distancia al player
            //transform.position = pos;
            //Set RigidBody to Static
            rb2d.bodyType = RigidbodyType2D.Static;
            //Set is hang to true
            isHanging = true;
        }
    }

    void GrabLedgeJump(){
        if(isHanging && jumpPressed){
            //suelta el ledge
            isHanging = false;
            jumpPressed = false;
            //set rigidbody to Dynamic
            rb2d.bodyType = RigidbodyType2D.Dynamic;
            rb2d.AddForce(new Vector2(0f, hangJumpsForce), ForceMode2D.Impulse);
            Debug.Log("plus");
            //and Exit
            return;
        }
    }

    void CheckIfCanJump()
    {
        if(isGrounded)
        {
            jumpsLeft = maxJumps;
            isJumping = false;
        }
        if(jumpsLeft > 0 && !isDrinking && !isHanging){
            canJump = true;
        }
        else{
            canJump = false;
        }
    }


    void Jump(){
        if(jumpPressed)
        {
            if(canJump){
                if (!plParry.parryFail)
                {
                    Debug.Log("si");
                    rb2d.AddForce(new Vector2(0f, jumpForce));
                    isJumping = true;
                    jumpsLeft --;
                }
                //Si fallas el parry Saltas menos
                else
                {
                    rb2d.AddForce(new Vector2(0f , jumpForce * 0.65f));
                    isJumping = true;
                    jumpsLeft--;
                }
                jumpPressed = false;
            }
            //WALLHOP
             if (isWallSliding && movInputDir == 0f)
            {
                isWallSliding = false;
                Vector2 forceToAdd = new Vector2(wallHopForce * wallHopDir.x * -facingDir, wallHopForce * wallHopDir.y);
                rb2d.AddForce(forceToAdd, ForceMode2D.Impulse);
                Flip();
                jumpPressed = false;
            }
             //WALLJUMP
             else if (isWallSliding && movInputDir != 0f)
            {
                if (facingDir == movInputDir)
                {
                    //salto corto
                    isWallSliding = false;
                    isWallJump = true;
                    Vector2 forceToAdd = new Vector2(lowWallJumpForce * wallJumpDir.x * -movInputDir, lowWallJumpForce * wallJumpDir.y);
                    rb2d.AddForce(forceToAdd, ForceMode2D.Impulse);
                    Flip();                }
                else if(facingDir != movInputDir)
                {
                    //salto largo
                    isWallSliding = false;
                    isWallJump = true;
                    Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDir.x * movInputDir, wallJumpForce * wallJumpDir.y);
                    rb2d.AddForce(forceToAdd, ForceMode2D.Impulse);
                }
                jumpPressed = false;
            }
			else if(!isWallSliding){
				isWallJump = false;
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
        if(canDash || isDashing)
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
        if(isTouchingWall && !isGrounded && !isHanging)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
            isWallSlidingAnim = false;
        }
    }

    void IsWallSliding()
    {
        if(isWallSliding && !isHanging)
        {
            isWallJump = false;
            isJumping = false;
            
            wasWallSliding = true;
            if (rb2d.velocity.y < 0.1f && facingRight == 1 && movInputDir > 0f) //Pared Derecha
            {
                isWallSlidingAnim = true;
                rb2d.velocity = new Vector2(0f, rb2d.velocity.y);
                rb2d.velocity = new Vector2(rb2d.velocity.x, -wallSlideSpeed);
            }
            else if (rb2d.velocity.y < 0.1f && facingRight == -1 && movInputDir < 0f) //Pared Izquierda
            {
                isWallSlidingAnim = true;
                rb2d.velocity = new Vector2(0f, rb2d.velocity.y);
                rb2d.velocity = new Vector2(rb2d.velocity.x, -wallSlideSpeed);
            }
            else
            {
                isWallSlidingAnim = false;
            }
            if(facingRight == 1 && movInputDir > 0f)
            {
                isWallSlidingAnim = true;
            }
            else if (facingRight == -1 && movInputDir < 0f)
            {
                isWallSlidingAnim = true;
            }
        }
    }

    public void PlayerDamaged(int damage, Vector2 normal)
    {
        if (!invecibility)
        {
            invecibility = true;
            plAttack.isAttacking = false;
            isDamaged = true;
            invencibleTime = startInvencibleTime;
            health -= damage;

            if (isGrounded)
            {
                damageX = -normal.x * damagedPushForce;
                Debug.Log(normal.x);
                damageY = damagedPushForce;
            }
            else if (!isGrounded)
            {
                Debug.Log("HIT" + normal.x);
                damageX = -normal.x * damagedPushForce;
                damageY = -normal.y * damagedPushForce;
            }
            cameraController.letsShake = true;
            StartCoroutine(Blinking());
            
           //Lanzar sonido
           //Lanzar particulas

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

    private void DrinkPotion()
    {
        if(canDrink && !isDrinking)
        {
            if(potions > 0 && health < maxHealth)
            {
                isDrinking = true;
                potions--;
                health++;
                StartCoroutine(TimeDrinking());
            }
            else
            {
                //Sonido
                Debug.Log("NOPOTIONS");
            }

            if (potions <= 0)
            {
                potions = 0;
            }
        }
    }
	
    IEnumerator TimeDrinking()
    {
        yield return new WaitForSeconds(0.8f);
        isDrinking = false;
        currentTimeTillNextDrink = timeTillNextDrink;
    }

    private void UpdateAnimations()
    {
        anim.SetFloat("speedX", Mathf.Abs(rb2d.velocity.x));
        anim.SetFloat("speedY", rb2d.velocity.y);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isJumping", isJumping);
        anim.SetBool("isWallJump", isWallJump);
        anim.SetBool("isWallSliding", isWallSlidingAnim);
        anim.SetBool("isHanging", isHanging);
    }


    private void OnDrawGizmos(){
        Gizmos.DrawLine(groundCheckRightPos.position, new Vector3(groundCheckRightPos.position.x, groundCheckRightPos.position.y - groundCheckDistance, groundCheckRightPos.position.z));
        Gizmos.DrawLine(groundCheckLeftPos.position, new Vector3(groundCheckLeftPos.position.x , groundCheckLeftPos.position.y - groundCheckDistance, groundCheckLeftPos.position.z));
        Gizmos.DrawLine(wallCheckPos.position, new Vector3(wallCheckPos.position.x + wallCheckDistance, wallCheckPos.position.y, wallCheckPos.position.z));
        Gizmos.DrawLine(ledgeCheckPos.position,new Vector3(ledgeCheckPos.position.x + hangDistance, ledgeCheckPos.position.y, ledgeCheckPos.position.z));
        Gizmos.DrawLine(underLedgeCheckPos.position, new Vector3 (underLedgeCheckPos.position.x + hangDistance, underLedgeCheckPos.position.y, underLedgeCheckPos.position.z));
    }
}
