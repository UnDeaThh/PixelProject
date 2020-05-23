using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBase : MonoBehaviour
{
    [SerializeField] protected int nBoss;
    [SerializeField] protected int nLifes;
    protected SpriteRenderer sprite;
    protected Material mat;
    protected Collider2D col;
    protected Rigidbody2D rb;
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected float amplitudeShaking;
    [SerializeField] protected float frequencyShaking;
    [SerializeField] protected float timeShaking;

    public bool isDisolve;
    protected bool oneCallDead = false;

    private float fade = 1;

    protected int facingDir = 1;

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

    public virtual void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
        facingDir *= -1;
    }

    public virtual void Dead()
    {
        if (isDisolve)
        {
            fade -= Time.deltaTime;
            if (fade <= 0)
            {
                fade = 0;
                ScenesManager.scenesManager.BossKilled[nBoss] = true;
                SaveSystem.SaveSceneData(ScenesManager.scenesManager);
                isDisolve = false;
            }
            mat.SetFloat("_Fade", fade);
        }
    }
}
