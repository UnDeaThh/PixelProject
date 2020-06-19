using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H4Laser : MonoBehaviour
{

    [SerializeField] AudioSource audioSource;
    public void DestroyLaser()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController2>().PlayerDamaged(1, transform.position);
        }
    }


    void Sound()
    {
        audioSource.Play();
    }
}
