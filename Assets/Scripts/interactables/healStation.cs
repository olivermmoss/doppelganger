using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healStation : baseInteractable
{
    public override void Activate()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<playerHealth>().health = player.GetComponent<playerHealth>().maxHealth;
        player.GetComponent<playerHealth>().UpdateHearts();
        gameObject.GetComponent<ParticleSystem>().Play();
        gameObject.GetComponent<Animator>().SetBool("used", true);
        used = true;
        gameObject.GetComponent<AudioSource>().Play();
    }
}
