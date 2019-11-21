using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParry : MonoBehaviour
{
    private float timeBtwParry;
    public float startTimeBtwParry = 0.1f;
    public float parryDuration = 1f;

    private bool canParry;
    [HideInInspector] public bool isParry;
    private bool parryDone = false;
    public Vector2 parryRange = new Vector2(1f, 1f);
    public LayerMask whatIsEnemie;

    public Collider2D parryCol;
    private PlayerController plController;
    private Animator anim;
    private void Awake()
    {
        plController = GetComponent<PlayerController>();
        anim = GetComponentInChildren<Animator>();
        isParry = false;
        parryCol.enabled = false;
        
    }
    private void Update()
    {
        CheckIfCanParry();
        Parry();


        UpdateAnimations();
    }

    void CheckIfCanParry()
    {
        if(timeBtwParry <= 0 && plController.isGrounded == true)
        {
            canParry = true;
        }
        else 
        {
            timeBtwParry -= Time.deltaTime;
            canParry = false;
        }
    }

    void Parry()
    {
        if (canParry)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                isParry = true;
                //StartCoroutine(DoingParry());
                //if (isParry)
              //  {
                   Collider2D[] enemiesToParry = Physics2D.OverlapCapsuleAll(transform.position, parryRange, CapsuleDirection2D.Vertical, 0, whatIsEnemie);
                    if(enemiesToParry.Length == 0)
                    {
                        Debug.Log("nada");
                    }
                    else
                    {
                        Debug.Log("Enemie");
                    }
               // }
            }
        }

    }

   /* IEnumerator DoingParry()
    {
        isParry = true;
        parryCol.enabled = true;
        yield return new WaitForSeconds(parryDuration);
        parryCol.enabled = false;
        isParry = false;
        timeBtwParry = startTimeBtwParry;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isParry)
        {
            if(other.tag == "Enemy")
            {
                other.GetComponentInParent<Enemy>().Stuned();
            }
        }
    }
    */
    void UpdateAnimations()
    {
        anim.SetBool("isParry", isParry);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, parryRange);
    }
}
