using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class retractWhenPlayerNear : MonoBehaviour
{
    //first is player far, final is player close
    public Sprite[] sprites;
    private SpriteRenderer sr;
    private Transform player;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        sr.sprite = sprites[Mathf.FloorToInt(Mathf.Min((Mathf.Abs(player.position.x - transform.position.x) / 3), sprites.Length - 1))];
    }
}
