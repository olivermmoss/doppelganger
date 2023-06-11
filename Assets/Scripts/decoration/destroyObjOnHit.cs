using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyObjOnHit : MonoBehaviour
{
    //destroys object
    private bool done;
    public float waitTime = 5;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("playerAttack") && !done)
        {
            StartCoroutine(TakeDamage());
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("fireball") && !done)
        {
            StartCoroutine(TakeDamage());
        }
    }

    public IEnumerator TakeDamage()
    {
        done = true;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<ParticleSystem>().Play();
        gameObject.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }
}
