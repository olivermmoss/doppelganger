using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class tinySpiderController : baseEnemy
{
    public float speed;
    private float timer;

    public Sprite webbed;

    public override void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        timer = Time.time;
    }
    private IEnumerator Die(bool sound = true)
    {
        gameObject.layer = 10;
        if(sound)
            gameObject.GetComponent<AudioSource>().Play();
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<ParticleSystem>().Play();

        yield return new WaitForSeconds(deathTime);
        Destroy(gameObject);
    }

    public override void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject == player && !dead)
            KillPlayer();
        if (other.gameObject.layer == 8 && !dead)
        {
            dead = true;
            gameObject.GetComponent<SpriteRenderer>().sprite = webbed;
            gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            other.gameObject.layer = 0;
        }
        if (other.gameObject.CompareTag("ground") && !dead)
        {
            StartCoroutine(Die(false));
        }
        if (other.gameObject.CompareTag("fireball") && !dead)
        {
            StartCoroutine(Die());
        }
    }

    //taking damage
    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "playerAttack")
        {
            StartCoroutine(Die());
        }
    }

    public void FixedUpdate()
    {
        if(!dead)
        {
            gameObject.transform.Translate(speed * Vector2.up);
            if (Time.time - 100 >= timer)
            {
                Destroy(gameObject);
            }
        }
        
    }
}