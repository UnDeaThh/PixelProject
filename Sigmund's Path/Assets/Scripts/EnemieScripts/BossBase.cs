using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBase : MonoBehaviour
{
    [SerializeField] protected int nLifes;
    protected SpriteRenderer sprite;
    protected Material mat;
    protected Collider2D col;

    protected bool isDisolve;
    protected bool oneCallDead;

    private float fade;



    private void Start()
    {
        fade = 1;
    }

    public void TakeDamage(int damage)
    {
        if (nLifes > 0)
        {
            nLifes -= damage;
            StartCoroutine(Blinking());
        }
        else
            return;
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

    public virtual void Dead()
    {
        if(nLifes <= 0 && !oneCallDead)
        {
            col.enabled = false;
            isDisolve = true;
            oneCallDead = true;
        }

        if (isDisolve)
        {
            fade -= Time.deltaTime;
            if (fade <= 0)
            {
                fade = 0;
                isDisolve = false;
            }
            mat.SetFloat("_Fade", fade);
        }
    }
}
