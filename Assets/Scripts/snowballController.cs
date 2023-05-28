﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snowballController : MonoBehaviour
{
    public bool destroyOnPlayer = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("ground") || collision.gameObject.CompareTag("fireball") || (collision.gameObject.CompareTag("Player") && destroyOnPlayer)) && collision.gameObject.GetComponent<PlatformEffector2D>() == null)
        {
            Poof();
        }
    }

    void Poof()
    {
        GetComponent<ParticleSystem>().Play();
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
    }
}