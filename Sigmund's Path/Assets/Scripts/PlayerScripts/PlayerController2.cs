using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController2 : MonoBehaviour
{
    public static PlayerController2 plController2;

    public int health = 5;
    public int maxHealth = 5;
    public int facingDir = 1;
    public int potions;
    private int maxPotions;
    private int dashDir;

    private float movDir;
    public float speedMov = 10f;
    public float wallDistance;
    public float groundDistance;
    public float jumpForce;
    public float wallSlideSpeed;
    private float stickyTime = 0.22f;
    private float cntStickyTime;
    public float wallJumpForce;
    private float heedTime = 0.16f;
    private float cntHeedTime;
    private float cntTimeNextDrink;
    public float timeNextDrink = 0.7f;
    public float timeDrinking;
    private float cntTimeDrinking;
    public float dashSpeed;
    public float dashDuration;
    private float cntDashDuration;
    public float timeNextDash;
    private float cntTimeNextDash;
    public float invencibilityTime;
    private float cntinvencibilityTime;
    public float damagedPushForce;
    const float coyoteTime = 0.07f;
    private float cntCoyoteTime;

    public bool isDead;
    public bool isGrounded;
    private bool jumpPressed = false;
    private bool canNormalJump;
    public bool isWallSliding;
    private bool canFlip = false;
    private bool stillBounding;
    private bool wallJumped;
    private bool canDrink;
    public bool isDrinking;
    [SerializeField] private bool heedArrows = true;
    private bool canDash;
    private bool shiftPressed;
    private bool isDashing;
    private bool isDamaged;
    private bool isInvencible;
    private bool shiftAlreadyPressed = false;
    public bool dashUnlocked = false;
    public bool highJumpUnlocked = false;
    public bool wallJumpUnlocked;
    public bool isGODmode = false;


    public Image[] potionsUI;
    public Image[] heartsUI;
    public Sprite fullHeartUI;
    public Sprite emptyHeartUI;

    private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer sprite; 
    public Transform frontPos;
    public Transform feetPosLeft;
    public Transform feetPosRight;

    public Vector2 wallJumpDir;


    public RaycastHit2D isGroundedLeft;
    public RaycastHit2D isGroundedRight;
    private RaycastHit2D isTouchingWall;

    public LayerMask whatIsGround;
    [SerializeField] Collider2D plCollider;
    private void Awake()
    {
        if (plController2 == null)
        {
            plController2 = this;
        }
        if (plController2 != this)
        {
            Destroy(gameObject);
        }

        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        GODmode();

        CheckLife();
        if (!isGODmode)
        {
            if (!isDead)
            {
                CheckEnvironment();
                CheckIfWallSliding();
                CheckIfCanJump();
                CheckIfCanDrink();
                CheckPotionsUI();
                CheckIfCanDash();
                InputPlayer();
                FacingDirection();
                Invencibility();
            }
        }

    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            AplyMovement();
            Jump();
            IsWallSliding();
            Dash();

            ReturnControlForMovement();
            LimitVelocity();
        }
    }

    void GODmode()
    {
        if(Input.GetKeyDown(KeyCode.R) && Input.GetKey(KeyCode.E))
        {
            isGODmode = !isGODmode;
        }

        if (isGODmode)
        {
            plCollider.enabled = false;
            rb.bodyType = RigidbodyType2D.Static;
            float verticalDir;
            verticalDir = Input.GetAxisRaw("Vertical");
            float horizontalDir = Input.GetAxisRaw("Horizontal");
            transform.position += new Vector3(horizontalDir * 0.5f, verticalDir * 0.5f, transform.position.z);
            gameObject.tag = "GOD";

        }
        else
        {
            plCollider.enabled = true;
            rb.bodyType = RigidbodyType2D.Dynamic;
            gameObject.tag = "Player";
        }
    }

    void InputPlayer()
    {
        movDir = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            jumpPressed = true;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Drink();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            shiftPressed = true;
        }
    }

    void AplyMovement()
    {
        if (!isGODmode)
        {
            if (heedArrows)
            {
                if (!isWallSliding)
                {
                    rb.velocity = new Vector2(movDir * speedMov * Time.fixedDeltaTime, rb.velocity.y);
                }
            }
        }
    }

    void CheckEnvironment()
    {
        isTouchingWall = Physics2D.Raycast(frontPos.position, transform.right, wallDistance, whatIsGround);
        isGroundedRight = Physics2D.Raycast(feetPosRight.position, Vector2.down, groundDistance, whatIsGround);
        isGroundedLeft = Physics2D.Raycast(feetPosLeft.position, Vector2.down, groundDistance, whatIsGround);
        if (!isGroundedRight && !isGroundedLeft)
        {
            isGrounded = false;
        }
        else
            isGrounded = true;

    }

    void CheckIfCanJump()
    {
        if (isGrounded)
        {
            cntCoyoteTime = 0;
        }
        else
        {
            cntCoyoteTime += Time.deltaTime;
        }


        if (isGrounded && !isWallSliding)
        {
            canNormalJump = true;
        }
        else
        {
            canNormalJump = false;
        }
    }

    void CheckIfWallSliding()
    {
        if (isTouchingWall && !isGroundedLeft)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
    }

    void CheckIfCanDrink()
    {
        if (!isGrounded)
        {
            canDrink = false;
        }
        else
        {
            if(cntTimeNextDrink < timeNextDrink)
            {
                canDrink = false;
                cntTimeNextDrink += Time.deltaTime;
            }
            else if(cntTimeNextDrink >= timeNextDrink)
            {
                canDrink = true;
            }
        }
    }

    void CheckLife()
    {
        if (health > 0)
        {
            isDead = false;
        }
        else
            isDead = true;
        if(health > maxHealth)
        {
            health = maxHealth;
        }
        if(maxHealth <= 5)
        {
            maxHealth = 5;
        }
        else if(maxHealth >= 10)
        {
            maxHealth = 10;
        }

        //UI
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
            if (i < maxHealth)
            {
                heartsUI[i].enabled = true;
            }
            else
            {
                heartsUI[i].enabled = false;
            }
        }
    }

    void CheckPotionsUI()
    {
        if(potions == maxPotions)
        {
            potions = maxPotions;
        }
        for (int i = 0; i < potionsUI.Length; i++)
        {
            if (i < potions)
            {
                potionsUI[i].enabled = true;
            }
            else
                potionsUI[i].enabled = false;
        }
    }

    void CheckIfCanDash()
    {
        if (isGrounded && dashUnlocked)
        {
            canDash = true;
        }
        else
            canDash = false;
    }

    void IsWallSliding()
    {
        if (isWallSliding)
        {
            cntHeedTime = 0;
            if (rb.velocity.y < -0.1)
            {
                if ((facingDir == 1 && movDir > 0) || (facingDir == -1 && movDir < 0)) //Misma Pared que flecha
                {
                    rb.velocity = new Vector2(0f, -wallSlideSpeed);
                }
                else if ((facingDir == 1 && movDir < 0) || (facingDir == -1 && movDir > 0)) // Contraria Pared que flecha
                {
                    cntStickyTime += Time.deltaTime;
                }
            }
            //Desengancharse
            if (cntStickyTime >= stickyTime)
            {
                canFlip = true;
                Flip();
                cntStickyTime = 0;
            }
        }
    }
    void FacingDirection()
    {
        if (facingDir == 1 && movDir < 0)
        {
            Flip();
        }
        else if (facingDir == -1 && movDir > 0)
        {
            Flip();
        }
    }

    void Flip()
    {
        if (!isWallSliding || canFlip)
        {
            facingDir *= -1;
            transform.Rotate(0f, 180f, 0f);
            canFlip = false;
        }
    }

    void Jump()
    {
        if (jumpPressed)
        {
            if (canNormalJump || cntCoyoteTime < coyoteTime)
            {
                rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            }
            WallJump();
            jumpPressed = false;
        }
    }

    void WallJump()
    {
        if (wallJumpUnlocked)
        {
            if (isWallSliding)
            {
                heedArrows = false; //Mientras 
                rb.AddForce(new Vector2(wallJumpDir.x * wallJumpForce * -facingDir, wallJumpDir.y * wallJumpForce));
                canFlip = true;
                Flip();
                cntStickyTime = 0;
                print("WallJump");
            }
        }
    }
    void ReturnControlForMovement()
    {
        if (!heedArrows)
        {
            cntHeedTime += Time.deltaTime;
            if (cntHeedTime >= heedTime)
            {
                heedArrows = true;
                cntHeedTime = 0;
            }
        }
    }

    void Drink()
    {
        //Un primer evento que pone el bool isDrinking = true para que el jmugador se relantice
        //un segundo evento al final de la animacion para curarse
        if(canDrink && !isDrinking)
        {
            if(potions > 0 && health < maxHealth)
            {
                //lamar al evento por animacion 
                isDrinking = true;
                potions--;
                health++;
                StartCoroutine(TimeDrinking());
            }
        }
    }

    void Dash()
    {
        if (canDash || isDashing)
        {
            if (shiftPressed && !shiftAlreadyPressed)
            {
                shiftAlreadyPressed = true;
                isDashing = true;
                dashDir = facingDir;
                cntDashDuration = 0;
            }
            if (isDashing && cntDashDuration < dashDuration) // tiempo que esta dasheando
            {
                cntDashDuration += Time.deltaTime;
                rb.velocity = new Vector2(dashSpeed * dashDir * Time.fixedDeltaTime, rb.velocity.y);
                Physics2D.IgnoreLayerCollision(9, 10);
            }
            else // dash acabado
            {
                isDashing = false;
                shiftPressed = false;
                cntDashDuration = 0;
            }
            if(cntDashDuration > dashDuration)
            {
                Physics2D.IgnoreLayerCollision(9, 10, false);
                StartCoroutine(GoNextDash());
            }
        }

    }
    IEnumerator GoNextDash()
    {
        shiftAlreadyPressed = true;
        yield return new WaitForSeconds(timeNextDash);
        shiftAlreadyPressed = false;   
    }

    IEnumerator TimeDrinking()
    {
        yield return new WaitForSeconds(0.8f);
        isDrinking = false;
        cntTimeNextDrink = 0;
    }

    void LimitVelocity()
    {
        if (rb.velocity.y >= 20f)
        {
            rb.velocity = new Vector2(rb.velocity.x, 20);
            Debug.Log("maxSpeedY");
        }
    }

    public void PlayerDamaged(int damage, Vector2 enemyPos)
    {
        if (!isInvencible)
        {
            isInvencible = true;
            isDamaged = true;
            cntinvencibilityTime = 0;
            health -= damage;

            if(enemyPos.x <= transform.position.x)
            {
                heedArrows = false;
                rb.AddForce(Vector2.one * damagedPushForce);
            }
            else if(enemyPos.x > transform.position.x)
            {
                heedArrows = false;
                rb.AddForce(Vector2.one * -damagedPushForce);
            }

            CameraController.cameraController.letsShake = true;
            StartCoroutine(Blinking());
        }

    }

    IEnumerator Blinking()
    {
        for (int i = 0; i < 2; i++)
        {
            sprite.color = Color.gray;
            yield return new WaitForSeconds(0.1f);
            sprite.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
    }

    void Invencibility()
    {
        if (isInvencible)
        {
            cntinvencibilityTime += Time.deltaTime;
            Physics2D.IgnoreLayerCollision(9, 10);
            if(cntinvencibilityTime >= invencibilityTime)
            {
                Physics2D.IgnoreLayerCollision(9, 10, false);
                isInvencible = false;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (facingDir == 1)
        {
            Gizmos.DrawLine(frontPos.position, new Vector3(frontPos.position.x + wallDistance, frontPos.position.y, frontPos.position.z));
        }
        else
        {
            Gizmos.DrawLine(frontPos.position, new Vector3(frontPos.position.x - wallDistance, frontPos.position.y, frontPos.position.z));
        }
        Gizmos.DrawLine(feetPosLeft.position, new Vector3(feetPosLeft.position.x, feetPosLeft.position.y - groundDistance, feetPosLeft.position.z));
        Gizmos.DrawLine(feetPosRight.position, new Vector3(feetPosRight.position.x, feetPosRight.position.y - groundDistance, feetPosRight.position.z));
    }

}
