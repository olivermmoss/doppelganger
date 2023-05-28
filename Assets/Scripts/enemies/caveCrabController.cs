using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class caveCrabController : baseEnemy
{
    public float speed = 5;
    public bool movingRight;
    public float extraHeightTest;
    public bool barriers = true;

    void Update()
    {
        RaycastHit2D raycastHit;
        RaycastHit2D raycastHitDown;
        //check if hitting wall
        if (movingRight)
        {
            raycastHit = Physics2D.BoxCast(gameObject.GetComponent<BoxCollider2D>().bounds.center, gameObject.GetComponent<BoxCollider2D>().bounds.size * new Vector2(0.9f, 0.9f), 0f, Vector2.right, extraHeightTest);
            raycastHitDown = Physics2D.Raycast(gameObject.GetComponent<BoxCollider2D>().bounds.center + new Vector3(1, -0.6f, 0), Vector2.down, extraHeightTest);
        }
        else
        {
            raycastHit = Physics2D.BoxCast(gameObject.GetComponent<BoxCollider2D>().bounds.center, gameObject.GetComponent<BoxCollider2D>().bounds.size * new Vector2(0.9f, 0.9f), 0f, Vector2.left, extraHeightTest);
            raycastHitDown = Physics2D.Raycast(gameObject.GetComponent<BoxCollider2D>().bounds.center + new Vector3(-1, -0.6f, 0), Vector2.down, extraHeightTest);
        }
        if ((raycastHit.collider != null && raycastHit.collider.CompareTag("ground")) || (raycastHitDown.collider == null && !barriers))
        {
            FlipDir();
        }

        //move
        float step = speed * Time.deltaTime;

        if (movingRight)
        {
            gameObject.transform.position = new Vector2(gameObject.transform.position.x + step, gameObject.transform.position.y);
        }
        else
        {
            gameObject.transform.position = new Vector2(gameObject.transform.position.x - step, gameObject.transform.position.y);
        }

        //gameObject.transform.rotation = Quaternion.Euler(1, 1, 1);
    }

    public override IEnumerator Die()
    {
        dieSource.Play();
        animator.SetBool("dead", true);
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        gameObject.layer = 10;
        speed = 0;
        yield return new WaitForSeconds(deathTime);
        Destroy(gameObject);
    }

    void FlipDir()
    {
        movingRight = !movingRight;
        GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
    }
    void OnDrawGizmosSelected()
    {
        // Draws a 5 unit long red line in front of the object
        Gizmos.color = Color.red;
        Vector3 direction = Vector3.down * extraHeightTest;
        Gizmos.DrawRay(gameObject.GetComponent<BoxCollider2D>().bounds.center + new Vector3(1, -0.6f, 0), direction);
    }

    public override void ExtraOnHit()
    {
        FlipDir();
    }
    public override void ExtraOnHitPlayer()
    {
        FlipDir();
    }
}