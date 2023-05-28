using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class spiderController : MonoBehaviour
{
    public GameObject player;
    public int deathTime = 10;
    public Animator animator;
    private float hitTime;
    public int attackDamage = 1;
    public AudioSource dieSource;

    public int maxHealth = 20;
    private int currentHealth;
    public bool dead = false;

    public AIDestinationSetter setter;
    public AIPath path;
    public GameObject emitter;

    private void Update()
    {
        emitter.transform.position = gameObject.transform.position;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        currentHealth = maxHealth;
        setter.target = player.transform;
    }
    private IEnumerator Die()
    {
        dieSource.Play();
        animator.SetBool("dead", true);
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        gameObject.layer = 10;
        setter.enabled = false;
        path.enabled = false;
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        yield return new WaitForSeconds(deathTime);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject == player && !dead)
            KillPlayer();
    }

    //taking damage
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "playerAttack")
        {
            TakeDamage(other.transform.parent.GetComponent<playerAttack>().attackDamage);
            hitTime = Time.time + 0.3f;
        }
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

            animator.SetTrigger("hit");

            emitter.GetComponent<ParticleSystem>().Play();

            if (currentHealth <= 0)
            {
                dead = true;
                StartCoroutine(Die());
            }
            else
            {
                gameObject.GetComponent<AudioSource>().Play();
            }
        }
    }
}