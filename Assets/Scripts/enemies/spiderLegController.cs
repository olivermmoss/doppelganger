using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spiderLegController : baseEnemy
{
    private bigSpiderController boss;
    private Animator anim;
    private float timer = 0;
    private float timer2 = 99999;

    public override void Start()
    {
        boss = FindObjectOfType<bigSpiderController>();
        anim = gameObject.GetComponent<Animator>();
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player");

        CalculateLook();
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("playerAttack") && Time.time > timer + 0.1f)
        {
            timer = Time.time;
            currentHealth -= player.GetComponent<playerAttack>().attackDamage;
            boss.TakeDamage();
            anim.SetTrigger("hit");

            if (currentHealth <= 0)
            {
                StartCoroutine(Die());
            }
        }
    }

    public override IEnumerator Die()
    {
        dead = true;
        GetComponent<PolygonCollider2D>().enabled = false;
        anim.SetBool("down", false);
        gameObject.GetComponent<AudioSource>().Play();
        Destroy(gameObject, 0.5f);
        yield return null;
    }

    public void CalculateLook()
    {
        boss = FindObjectOfType<bigSpiderController>();

        if (boss.gameObject.transform.position.x >= gameObject.transform.position.x)
        {
            gameObject.transform.localScale = new Vector2(-1, 1);
        }

        GetComponent<SpriteRenderer>().sortingOrder = -Mathf.Abs(Mathf.RoundToInt(boss.gameObject.transform.position.x - gameObject.transform.position.x));

        timer2 = Time.time;
        GetComponent<PolygonCollider2D>().enabled = false;
    }

    private void Update()
    {
        if(timer2 <= Time.time - 1f && !dead)
        {
            GetComponent<PolygonCollider2D>().enabled = true;
        }
    }
}
