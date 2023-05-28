using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wastelandBat : baseEnemy
{
    public float speed = 5;
    public int aggroDistance = 20;
    private float hitTime;

    //ai: chase after player, face player, turn around, all that good stuff
    void Update()
    {
        if(Vector2.Distance(player.transform.position, gameObject.transform.position) > aggroDistance || dead)
        {
            return;
        }

        float step = speed * Time.deltaTime;
        gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, player.transform.position, step);
        gameObject.transform.LookAt(player.transform);

        if(player.transform.position.x <= gameObject.transform.position.x)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }

        gameObject.transform.rotation = Quaternion.Euler(1, 1, 1);


        if(Time.time >= hitTime)
        {
            if(speed < 0)
                speed = -speed;    
        }
    }

    public override void ExtraOnHit()
    {
        speed = -speed;
        hitTime = Time.time + 0.3f;
    }
}
