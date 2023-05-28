using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minirocController : baseEnemy
{
    //1 to -1
    public float right;
    public float distanceFromPlayer;
    public int aggroDistance = 20;
    public float speed;
    public bool diving;
    public bool divingLeft;
    private float diveTimer;
    public float swoopDepth;
    private Vector2 playerPos;
    public float horizontalMultiplier;

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(player.transform.position, gameObject.transform.position) > aggroDistance || dead)
        {
            diveTimer = Time.time + 5;
            return;
        }

        Vector2 goalPos;
        if(!diving)
        {
            int distMod = 0;
            if(diveTimer < Time.time)
            {
                Dive();
            }
            else if (diveTimer - 0.2f < Time.time)
            {
                distMod = 1;
            }
            //player pos but moved diagonallyUp
            goalPos = (Vector2)player.transform.position + new Vector2(right * (distanceFromPlayer + distMod), distanceFromPlayer + distMod);           
        }
        else
        {
            int diveDir = divingLeft ? -1 : 1;
            //player pos but moved along parabola
            goalPos = playerPos + new Vector2(right * distanceFromPlayer, distanceFromPlayer * right * right * ((distanceFromPlayer + swoopDepth) / (distanceFromPlayer * horizontalMultiplier * horizontalMultiplier)) - swoopDepth);
            right += speed * diveDir * Time.deltaTime / 10 ;

            if(right < -horizontalMultiplier && divingLeft)
            {
                right = -horizontalMultiplier;
                diving = false;
                transform.localScale = new Vector2(-1, 1);
                diveTimer = Time.time + Random.Range(3, 6);
                speed /= 2;
                //animator.SetBool("diving", false);
            }
            else if (right > horizontalMultiplier && !divingLeft)
            {
                right = horizontalMultiplier;
                diving = false;
                transform.localScale = new Vector2(1, 1);
                diveTimer = Time.time + Random.Range(3, 6);
                speed /= 2;
                //animator.SetBool("diving", false);
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, goalPos, speed * Time.deltaTime);
    }

    void Dive()
    {
        diving = true;
        //if right, then true, if left, then false
        divingLeft = right > 0;
        speed *= 2;
        playerPos = player.transform.position;
        //animator.SetBool("diving", true);
    }

    public override void Start()
    {
        base.Start();
        if(player.transform.position.x > gameObject.transform.position.x)
        {
            //flip initial facing dir if player is on right
            right = -right;
            transform.localScale = new Vector2(-1, 1);
        }
    }
}
