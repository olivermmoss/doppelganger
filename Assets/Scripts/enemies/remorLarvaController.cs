using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class remorLarvaController : baseEnemy
{
    public float speed;
    public bool clockwise;
    public float extraHeightTest;
    public bool turning;
    public int turnDir;
    public float rotSpeed;
    public float goalRot;
    Vector2 bounds;
    Vector2 center;
    Vector2 up;
    Vector2 right;

    public override void Start()
    {
        bounds = gameObject.GetComponent<BoxCollider2D>().bounds.size;
        player = GameObject.FindGameObjectWithTag("Player");
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (!dead)
        {
            center = gameObject.GetComponent<BoxCollider2D>().bounds.center;
            //RaycastHit2D raycastHit;
            RaycastHit2D raycastHitDown;
            RaycastHit2D raycastHit;
            up = transform.up;
            right = transform.right;
            int whichWay = clockwise ? 1 : -1;

            float step = speed * Time.deltaTime;

            if (!turning)
            {
                //raycasts!
                raycastHit = Physics2D.Raycast(center, right * whichWay, extraHeightTest);
                raycastHitDown = Physics2D.Raycast(center, -up, bounds.y / 2 + extraHeightTest);

                //move forward
                gameObject.transform.Translate(whichWay * step * gameObject.transform.right, Space.World);

                if(raycastHit.collider != null && raycastHit.collider.CompareTag("ground"))
                {
                    //turn up
                    Turn(true);
                }

                //execute if going to turn down
                else if (raycastHitDown.collider == null)
                {
                    //turn down
                    Turn(false);
                }
            }

            //if turning
            else
            {
                //rotate
                transform.Rotate(new Vector3(0, 0, turnDir) * Time.deltaTime * rotSpeed, Space.World);
                float angle = transform.eulerAngles.z;

                //detect if turn is over
                if (FastApproximately(angle, goalRot, 5))
                {
                    //end turning
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, goalRot));
                    turning = false;
                    //float*float*vector for speed :heart_eyes:
                    //move forward once to make sure it doesn't just turn again
                    gameObject.transform.Translate(whichWay * step * gameObject.transform.right, Space.World);
                }
            }
        }
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
        clockwise = !clockwise;
        GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
    }

    void Turn(bool up)
    {
        Vector2 pos = transform.position;
        //up is false on down raycast, true on forward raycast
        transform.position = new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
        turning = true;
        turnDir = (up ? 1 : -1) * (clockwise ? 1 : -1);
        goalRot = Mathf.Round(transform.eulerAngles.z / 90) * 90 + turnDir * 90;
        if(goalRot < 0)
        {
            goalRot += 360;
        }
    }

    public static bool FastApproximately(float a, float b, float threshold)
    {
        return ((a - b) < 0 ? ((a - b) * -1) : (a - b)) <= threshold;
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