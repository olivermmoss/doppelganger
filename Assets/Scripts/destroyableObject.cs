using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyableObject : MonoBehaviour
{
    //changes sprite
    private bool done;
    public Sprite toChange;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("playerAttack") && !done)
        {
            TakeDamage();
        }
        if (other.gameObject.CompareTag("fireball") && !done)
        {
            TakeDamage();
        }
    }

    public void TakeDamage()
    {
        done = true;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().sprite = toChange;
        gameObject.GetComponent<ParticleSystem>().Play();
        gameObject.GetComponent<AudioSource>().Play();
    }
}