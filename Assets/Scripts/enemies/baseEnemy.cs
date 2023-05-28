using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baseEnemy : MonoBehaviour
{
    public int maxHealth = 20;
    public int currentHealth;
    public bool dead = false;
    public int attackDamage = 1;
    public AudioSource dieSource;
    public Animator animator;
    public GameObject player;
    public int deathTime = 10;

    public virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        currentHealth = maxHealth;
    }

    public void KillPlayer()
    {
        player.GetComponent<playerHealth>().TakeDamage(attackDamage, gameObject);
    }

    public void TakeDamage(int damage)
    {
        if (!dead)
        {
            currentHealth -= damage;

            if(animator != null)
                animator.SetTrigger("hit");

            if(gameObject.GetComponent<ParticleSystem>() != null)
                gameObject.GetComponent<ParticleSystem>().Play();

            if (currentHealth <= 0)
            {
                dead = true;
                StartCoroutine(Die());
            }
            else
            {
                if (gameObject.GetComponent<AudioSource>() != null)
                    gameObject.GetComponent<AudioSource>().Play();
            }
        }
    }

    public virtual IEnumerator Die()
    {
        if(dieSource != null)
            dieSource.Play();
        if(animator != null)
            animator.SetBool("isDead", true);
        if(gameObject.GetComponent<Rigidbody2D>() != null)
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        gameObject.layer = 10;
        yield return new WaitForSeconds(deathTime);
        Destroy(gameObject);
    }
    //damage player on collision
    public virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject == player && !dead)
        {
            KillPlayer();
            ExtraOnHitPlayer();
        }
        if (other.gameObject.CompareTag("fireball") && !dead)
        {
            TakeDamage(other.gameObject.GetComponent<fireballController>().attackDamage);
            ExtraOnHit();
        }
    }

    //taking damage trigger
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("playerAttack") && !dead)
        {
            TakeDamage(other.transform.parent.GetComponent<playerAttack>().attackDamage);
            ExtraOnHit();
        }
        if (other.gameObject.CompareTag("fireball") && !dead)
        {
            TakeDamage(other.gameObject.GetComponent<fireballController>().attackDamage);
            ExtraOnHit();
        }
    }

    public virtual void ExtraOnHit()
    {
        //override this;
    }

    public virtual void ExtraOnHitPlayer()
    {
        //override this;
    }
}
