using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulbController : baseEnemy
{
    private Rigidbody2D rb;
    private bool inAir;
    private float timer;
    public Vector2 jumpForce;

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(player.transform.position, gameObject.transform.position) > 30f || dead)
        {
            if (transform.position.y < -50)
            {
                StartCoroutine(Die());
            }
            return;
        }

        if (Time.time > timer + 1f && !inAir)
        {
            Jump();
        }
        if(inAir)
        {
            transform.up = rb.velocity;

            RaycastHit2D raycastHit = Physics2D.BoxCast(gameObject.GetComponent<BoxCollider2D>().bounds.center + Vector3.up * 0.1f, gameObject.GetComponent<BoxCollider2D>().bounds.size, 0f, Vector2.down, 0.2f);
            if (raycastHit.collider != null && raycastHit.collider.tag == "ground" && raycastHit.collider.gameObject.layer == 0 && Time.time > timer + 0.1f)
            {
                Land();
            }
        }
    }

    void Jump()
    {
        inAir = true;
        animator.SetBool("inAir", true);
        rb.bodyType = RigidbodyType2D.Dynamic;
        float diff = player.transform.position.x - transform.position.x;
        if (diff >= 0)
        {
            rb.AddForce(Vector2.up * jumpForce.y + Vector2.right * Mathf.Clamp(40f * diff, 0, jumpForce.x));
        }
        else
        {
            rb.AddForce(Vector2.up * jumpForce.y - Vector2.right * Mathf.Clamp(40f * -diff, 0, jumpForce.x));
        }
        timer = Time.time;
    }

    void Land()
    {
        transform.rotation = Quaternion.identity;
        timer = Time.time;
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, Mathf.Round(gameObject.transform.position.y));
        inAir = false;
        animator.SetBool("inAir", false);
        rb.bodyType = RigidbodyType2D.Static;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        timer = Time.time;
    }

    public override void OnCollisionEnter2D(Collision2D other)
    {
        base.OnCollisionEnter2D(other);
    }

    public override void ExtraOnHit()
    {
        rb.AddForce((Vector2)(gameObject.transform.position - player.transform.position) * 100);
    }
}
