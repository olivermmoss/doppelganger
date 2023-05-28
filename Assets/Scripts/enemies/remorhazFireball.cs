using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class remorhazFireball : fireballController
{
    [SerializeField]
    private float rotSpeed;
    [SerializeField]
    private int ballType;
    private Vector2 lastPos;
    public override void FixedUpdate()
    {
        if(ballType == 1)
        {
            float angle = Mathf.Atan2(player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x) * Mathf.Rad2Deg - 90;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
            base.FixedUpdate();
        } else if(ballType == 0)
        {
            float angle = Vector2.SignedAngle(new Vector2(1, 0), new Vector2(1, Mathf.Cos(Time.time*16))) - 90;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            base.FixedUpdate();
        }
        else
        {
            lastPos = transform.position;
            Vector2 target = (Vector2)player.transform.position + 4 * RadianToVector2(Time.time);
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            float angle = Mathf.Atan2(transform.position.y - lastPos.y, transform.position.x - lastPos.x) * Mathf.Rad2Deg - 90;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.rotation = targetRotation;
        }

        checkIfHit(Physics2D.OverlapCircle(transform.position+transform.up*0.25f, 0.75f,0));
    }

    public override void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.GetComponent<playerHealth>().TakeDamage(attackDamage, gameObject);
            Poof();
        }
        else if (other.gameObject.CompareTag("ground") || other.gameObject.CompareTag("fireball"))
        {
            Poof();
        }
    }

    void checkIfHit(Collider2D other)
    {
        if (other != null && other.gameObject.CompareTag("ground"))
        {
            Poof();
        }
    }

    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }
}
