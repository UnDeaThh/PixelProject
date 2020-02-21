using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController2 : MonoBehaviour
{
    private PauseManager pauseManager;
    private Inventory2 inventory;
    private PlayerAudio plAudio;
    private PlayerAttack plAttack;

    public int health = 5;
    public int maxHealth = 5;
    public int facingDir = 1;
    public int potions;
    public int maxPotions = 5;
    private int dashDir;
    private int maxJumps;
    private int cntJumps;

    public int lastScene = 3;

    private float movDir;
    public float speedMov = 10f;
    public float jumpTime = 0.5f;
    private float cntJumpTime;
    public float wallDistance;
    public float groundDistance;
    public float jumpForce;
    public float maxSpeedY;
    public float wallSlideSpeed;
    private float stickyTime = 0.22f;
    private float cntStickyTime;
    public float wallJumpForce;
    private float heedTime = 0.16f;
    private float cntHeedTime;
    [HideInInspector] public float cntTimeNextDrink;
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
    //[HideInInspector] bool deadChange;
    public bool isGrounded;
    public bool isJumping;
    private bool oneChanceDirection = false;
    private bool jumpPressed = false;
    private bool jumpHolded;
    private bool jumpUnhold;
    private bool canJump;
    private bool canNormalJump;
    public bool isWallSliding;
    private bool canFlip = false;
    private bool stillBounding;
    private bool wallJumped;
    private bool canDrink;
    public bool isDrinking;
    [SerializeField] public bool heedArrows = true;
    private bool canDash;
    private bool shiftPressed;
    public bool isDashing;
    private bool bombPressed;
   // private bool isDamaged;
    private bool isInvencible;
    private bool shiftAlreadyPressed = false;
    //ABILITIES
    public bool dashUnlocked = false;
    public bool dobleJumpUnlocked = false;
    public bool wallJumpUnlocked = false;
    //
    public bool isOnKinematic = false;
    public bool isGODmode = false;


    public Image[] potionsUI;
    public Image[] heartsUI;
    public Sprite fullHeartUI;
    public Sprite emptyHeartUI;

    [HideInInspector] public Rigidbody2D rb;
    [SerializeField] private SpriteRenderer sprite; 
    public Transform frontPos;
    public Transform feetPosLeft;
    public Transform feetPosRight;

    public Vector2 wallJumpDir;
    [Header("DeadPanel")]
    [SerializeField] GameObject deadPanelUI;
    [HideInInspector] public bool pasSceneDead;


    public RaycastHit2D isGroundedLeft;
    public RaycastHit2D isGroundedRight;
    private RaycastHit2D isTouchingWall;

    public LayerMask whatIsGround;
    [SerializeField] Collider2D plCollider;
    public GameObject bombPrefab;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pauseManager = GameObject.FindGameObjectWithTag("PauseManager").GetComponent<PauseManager>();
        inventory = GetComponentInChildren<Inventory2>();
        plAudio = GetComponentInChildren<PlayerAudio>();
        plAttack = GetComponent<PlayerAttack>();
    }
    private void Update()
    {
        GODmode();

        CheckLife();
        if (!isGODmode)
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
            ThrowBombs();


            Dead();
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
        if (!isOnKinematic)
        {
            if (!isDead)
            {
                movDir = Input.GetAxisRaw("Horizontal");

                if (Input.GetButtonDown("Jump"))
                {
                    jumpPressed = true;
                }
                if (Input.GetButton("Jump"))
                {
                    jumpHolded = true;
                }
                else
                {
                    jumpHolded = false;
                }

                if (Input.GetKeyDown(KeyCode.X))
                {
                    Drink();
                }

                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    shiftPressed = true;
                }

                if (Input.GetKeyDown(KeyCode.C))
                {
                    bombPressed = true;
                }
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
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
        if (!dobleJumpUnlocked)
        {
            maxJumps = 1;
        }
        else
        {
            maxJumps = 2;
        }

        if (isGrounded && !isJumping)
        {
            cntJumps = 0;
        }

        if (!isDashing)
        {
            if(cntJumps < maxJumps)
            {
                canJump = true;
            }
            else
            {
                canJump = false;
            }
        }
        else
        {
            canJump = false;
        }
    }

    void CheckIfWallSliding()
    {
        if (isTouchingWall && !isGroundedLeft && wallJumpUnlocked)
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
        {
            isDead = true;
        }
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
        if (dashUnlocked)
        {
            if (!plAttack.isAttacking)
            {
                if (isGrounded)
                {
                    canDash = true;
                }
                else
                {
                    canDash = false;
                    shiftPressed = false;
                }
            }
            else
            {
                canDash = false;
                shiftPressed = false;
            }
        }
        else
        {
            shiftPressed = false;
        }
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

        if (oneChanceDirection)
        {
            if(facingDir == 1)
            {
                Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);
                transform.rotation = rotation;
            }
            else if(facingDir == -1)
            {
                Quaternion rotation = Quaternion.Euler(0f, 180f, 0f);
                transform.rotation = rotation;
            }
            oneChanceDirection = false;
        }
        

        if (heedArrows)
        {
            if (facingDir == 1 && movDir < 0)
            {
                Flip();
            }
            else if (facingDir == -1 && movDir > 0)
            {
                Flip();
            }
            else if(facingDir == 0)
            {
                facingDir = 1;
            }
        }
    }

    void Flip()
    {
        if (!isWallSliding || canFlip)
        {
            oneChanceDirection = true;
            facingDir *= -1;
            //transform.Rotate(0f, 180f, 0f);
            canFlip = false;
        }
    }

    void Jump()
    {
        if (jumpPressed)
        {
            if (canJump)
            {
                isJumping = true;
                cntJumpTime = jumpTime;
                cntJumps++;
                rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                if (!plAudio.jumpSound.isPlaying)
                {
                    plAudio.jumpSound.pitch = Random.Range(0.7f, 1.3f);
                    plAudio.jumpSound.Play();
                }
            }
            WallJump();
            jumpPressed = false;
        }

        if(jumpHolded && isJumping)
        {
            if(cntJumpTime > 0)
            {
                rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                cntJumpTime -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
        if (!jumpHolded)
        {
            isJumping = false;
        }
        /*
        if (jumpPressed)
        {
            if (canNormalJump || cntCoyoteTime < coyoteTime)
            {
                rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            }
            WallJump();
            jumpPressed = false;
        }
        */
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
        if (!isOnKinematic)
        {
            if (!isDrinking && !isDead && !plAttack.isAttacking)
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
        }
    }

    void Drink()
    {
        if(canDrink && !isDrinking)
        {
            if(potions > 0 && health < maxHealth)
            {
                isDrinking = true;
                heedArrows = false;
                potions--;
                rb.velocity = Vector2.zero;
                plAudio.healingSound[0].Play();
                //StartCoroutine(TimeDrinking());
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
                if (!plAudio.dashSound.isPlaying)
                {
                    plAudio.dashSound.Play();
                }
            }
            if (isDashing && cntDashDuration < dashDuration) // tiempo que esta dasheando
            {
                cntDashDuration += Time.deltaTime;
                rb.velocity = new Vector2(dashSpeed * dashDir * Time.fixedDeltaTime, rb.velocity.y);
                Physics2D.IgnoreLayerCollision(9, 10);
                Physics2D.IgnoreLayerCollision(12, 10);
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
                Physics2D.IgnoreLayerCollision(12, 10, false);
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

    /*
    IEnumerator TimeDrinking()
    {
        
        yield return new WaitForSeconds(timeDrinking);
        health++;
        isDrinking = false;
        cntTimeNextDrink = 0;
    }
    */
    void LimitVelocity()
    {
        if (rb.velocity.y >= maxSpeedY)
        {
            rb.velocity = new Vector2(rb.velocity.x, maxSpeedY);
        }
    }

    void ThrowBombs()
    {
        if (bombPressed)
        {
            if (inventory.nBombs > 0)
            {
                Instantiate(bombPrefab, transform.position + new Vector3(0.8f * facingDir, 0f, 0f), Quaternion.identity);
                inventory.nBombs--;
            }
            else
                Debug.Log("NO BOMS");
            bombPressed = false;
        }
    }

    public void PlayerDamaged(int damage, Vector2 enemyPos)
    {
        if (!isInvencible)
        {
            if (plAttack.isAttacking)
            {
                plAttack.isAttacking = false;
                plAttack.gndAttackingFront = false;
                plAttack.gndAttackingFront = false;
                plAttack.airAttackingFront = false;
                plAttack.airAttackingUp = false;
            }
            isInvencible = true;
            //isDamaged = true;
            cntinvencibilityTime = 0;
            health -= damage;
            if(health > 1)
            {
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
            }
            else
            {
                print("muerto");
            }

            CameraController.cameraController.letsShake = true;
            StartCoroutine(Blinking());
        }

    }

    IEnumerator Blinking()
    {
        for (int i = 0; i < 3; i++)
        {
            sprite.color = Color.gray;
            yield return new WaitForSeconds(0.15f);
            sprite.color = Color.white;
            yield return new WaitForSeconds(0.15f);
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

    private void OnParticleCollision(GameObject other)
    {
        if (!isDashing)
        {
            PlayerDamaged(1, other.gameObject.transform.position);
        }
        else
        {
            Debug.Log("was Dashing");
        }
    }

    void Dead()
    {
        if (isDead)
        {
            heedArrows = false;
            deadPanelUI.SetActive(true);
        }
        else
        {
            deadPanelUI.SetActive(false);
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
