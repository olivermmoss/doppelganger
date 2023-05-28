using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicProjectile : MonoBehaviour
{
    public float speed;
    public float timer;
    public GameObject player;
    public int deathTime = 10;
    public int attackDamage = 1;
    public bool damaging = true;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        timer = Time.time;
    }
    public virtual void FixedUpdate()
    {
        gameObject.transform.Translate(speed * Vector2.up);
        if (Time.time - deathTime >= timer)
        {
            Destroy(gameObject);
        }
    }

    public virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject == player && damaging)
            KillPlayer();
    }

    public void KillPlayer()
    {
        player.GetComponent<playerHealth>().TakeDamage(attackDamage, gameObject);
    }
}
