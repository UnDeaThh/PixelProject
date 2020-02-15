using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bombs : MonoBehaviour
{
    public float timeToExplode;
    public float explosionRadius;
    private float cntTimeToExplode;
    private bool exploding = false;
    private bool alreadyExploted;

    public LayerMask whatIsHitting;

    public SpriteRenderer spriteBomb;
    private Collider2D col;
    private Rigidbody2D rb;
    public ParticleSystem ps;
    private void Awake()
    {
        Physics2D.IgnoreLayerCollision(13, 10);
        cntTimeToExplode = timeToExplode;
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(cntTimeToExplode  > 0)
        {
            cntTimeToExplode -= Time.deltaTime;
            exploding = false;
        }
        else
        {
            exploding = true;
        }

        if (exploding && !alreadyExploted)
        {
            Collider2D explosCol = Physics2D.OverlapCircle(transform.position, explosionRadius, whatIsHitting);
            if (explosCol != null)
            {
                explosCol.gameObject.GetComponent<DestructibleWalls>().WallDestroyed();
            }
            else
                print("NO WALL");

            ps.Play();
            spriteBomb.enabled = false;
            col.enabled = false;
            Destroy(rb);

            exploding = false;
            alreadyExploted = true;
            Destroy(gameObject, 0.5f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
