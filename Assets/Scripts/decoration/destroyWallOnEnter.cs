using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyWallOnEnter : MonoBehaviour
{
    void Start()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        var save = GameObject.FindGameObjectWithTag("save").GetComponent<dontDestroySave>();
        if(save.cutscenesWatched[3])
        {
            Destroy(transform.parent.gameObject);
        }
        if (Mathf.Abs(player.position.x + 3.75f) < 0.1f)
        {
            transform.parent.GetComponent<SpriteRenderer>().enabled = false;
            transform.parent.GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            save.cutscenesWatched[3] = true;
            GetComponent<ParticleSystem>().Play();
        }
    }
}
