using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyController : MonoBehaviour
{
    public int maxHealth = 20;
    private int currentHealth;
    public bool dead = false;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if(!dead)
        {
            currentHealth -= damage;

            animator.SetTrigger("hit");

            gameObject.GetComponent<ParticleSystem>().Play();

            if(currentHealth <= 0)
            {
                dead = true;
            }
            else
            {
                gameObject.GetComponent<AudioSource>().Play();
            }
        }
    }
}
