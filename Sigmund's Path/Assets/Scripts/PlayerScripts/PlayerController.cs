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
    private GamePlayManager GM;
    [SerializeField] private SpriteRenderer sprite;

    //MOVIMIENTO HORIZONTAL
    [Header("Movement Attributes")]
    public float movSpeed;
    [HideInInspector] public int facingRight = 1;
    private float movInputDir;
    public float movementForceInAir;
    //JUMP
    [Header("Jump Attributes")]
    public float jumpForce;
    private int maxJumps = 1;
    private int jumpsLeft;
    [HideInInspector] public bool isGrounded;

    public Transform feetPos;
    private float checkRadius = 0.25f;
    public LayerMask whatIsGrounded;
    private bool canJump;
    //WALLJUMP
    [Header("WallJump Attributes")]
    private Vector2 wallHopDir = new Vector2(1, 1f);
    private Vector2 wallJumpDir = new Vector2(1, 2);
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

    //WALL SLIDING
    [Header("WallSliding Attributes")]
    public Transform wallCheckPos;
    private float wallCheckDistance = 0.2f;
    public float wallSlideSpeed;
    private bool isTouchingWall;
    private bool isWallSliding;
    private bool wasWallSliding;

    //LIFE
    [Header("Health Attributes")]
    public int health = 5;
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

    private float timeTillNextDrink = 2f;
    private float currentTimeTillNextDrink;

    //DAMAGED
    [Header("Damaged Attributes")]
    private bool damaged = false;
    private bool invecibility = false;
    private float invencibleTime;
    public float startInvencibleTime;
    public int damagedPushForce;

    private float damageX = 0;
    private float damageY = 0;




    void Awake(){
        rb2d = GetComponent<Rigidbody2D>();
        plParry = GetComponent<PlayerParry>();
        anim = GetComponentInChildren<Animator>();
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GamePlayManager>();

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
        CheckPotions();
        


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
		if(!GM.isPause){	
			movInputDir = Input.GetAxisRaw("Horizontal");
			if (Input.GetKeyDown(KeyCode.X) )
			{
				DrinkPotion();
			}
		}
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
        //Movimiento Normal
        if (isGrounded && !plParry.isParry && !plParry.parryFail && !damaged && !isDrinking)
        {
            rb2d.velocity = new Vector2(movInputDir * movSpeed + damageX, rb2d.velocity.y + damageY);
            wasWallSliding = false;
        }
        //Movimiento reducido cuando te estas curando
        else if (isGrounded && !plParry.isParry && !plParry.parryFail && !damaged && isDrinking)
        {
            rb2d.velocity = new Vector2(movInputDir * movSpeed * 0.2f + damageX, rb2d.velocity.y + damageY);
            wasWallSliding = false;
        }

        //Cuando te dañan te empujan
        else if((isGrounded || !isGrounded) && !plParry.isParry && !plParry.parryFail && damaged)
        {
            rb2d.velocity = Vector2.zero;
            rb2d.velocity = new Vector2(damageX, damageY);
            damaged = false;
        }
        //Cuando haces parry te quedas quieto
        else if (isGrounded && plParry.isParry == true) 
        {
            rb2d.velocity = Vector2.zero;
            wasWallSliding = false;
        }

        if (damageX < 0)
        {
            damageX+=1.0f;
        }
        else if (damageX > 0)
        {
            damageX-=1.0f;
        }

        if(damageY < 0)
        {
            damageY+=1.0f;
        }
        else if (damageY > 0)
        {
            damageY-=1.0f;
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
            invecibility = true;
            damaged = true;
            invencibleTime = startInvencibleTime;
            health -= damage;

            if (isGrounded)
            {
                damageX = -normal.x * damagedPushForce;
                damageY = damagedPushForce;
            }
            else if (!isGrounded)
            {
                Debug.Log("HIT" + normal.x);
                damageX = -normal.x * damagedPushForce;
                damageY = -normal.y * damagedPushForce;
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

    private void DrinkPotion()
    {
        if (canDrink)
        {
            if(potions > 0 && health < maxHealth)
            {
                isDrinking = true;
                potions--;
                health++;
                StartCoroutine(TimeDrinking());
               
                Debug.Log(potions);
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
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("speedX", Mathf.Abs(rb2d.velocity.x));
    }


    private void OnDrawGizmos(){
        Gizmos.DrawWireSphere(feetPos.position, checkRadius);
        Gizmos.DrawLine(wallCheckPos.position, new Vector3(wallCheckPos.position.x + wallCheckDistance, wallCheckPos.position.y, wallCheckPos.position.z));
    }
}
