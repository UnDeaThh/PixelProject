using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public enum EnemyClass
    {
        Changeling, Bermonch, Tatzel, Nach, Neck
    }
public class BaseEnemy : MonoBehaviour
{
    public int nLifes;
    public int damage;
    public bool isStuned;
    [HideInInspector] public bool callDead = false;
    [HideInInspector] public bool oneCallDead = false;
    public SpriteRenderer sprite;
    public AudioSource deadSound;
    public Material mat;
    private bool isDisolving;
    protected float fade = 1;


    public float movSpeed;
    [SerializeField] protected float timeStuned = 1f;
    protected float cntTimeStuned = 0;

    [HideInInspector]public float detectionRange;
    public LayerMask whatIsDetected;

    //public SoulTrail soul;

    public GameObject soulColectable;
    public EnemyClass enemyType;
    public Animator anim;

    public bool IsDisolving { get => isDisolving; set => isDisolving = value; }

    public virtual void Dead()
    {
        if(nLifes <= 0 && !oneCallDead)
        {
            //Instantiate Soul desde el Animation Event
            anim.SetTrigger("callDead");
            deadSound.Play();
            Collider2D col = GetComponent<Collider2D>();
            col.enabled = false;
            IsDisolving = true;
            oneCallDead = true;
        }
        if (IsDisolving)
        {
            fade -= Time.deltaTime;
            if(fade <= 0)
            {
                fade = 0;
                IsDisolving = false;
            }
            mat.SetFloat("_Fade", fade);
        }

        if (callDead)
        {
            if (deadSound)
            {
                if (!deadSound.isPlaying && fade <= 0f) 
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
    public void InstantiateSoul(EnemyClass enemyTipo)
    {
        soulColectable.GetComponent<SoulPickUp>().MoneyValor(enemyTipo);
        Instantiate(soulColectable, transform.position, Quaternion.identity);
    }

    public virtual void TakeDamage(int damage)
    {
        nLifes -= damage;
        Debug.Log(nLifes);
        StartCoroutine(Blinking());
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


    #region OVERRIDE EMPTY FUNCTIONS
    public virtual void StartStun()
    {

    }
    public virtual void Stuned()
    {

    }
    #endregion
}
