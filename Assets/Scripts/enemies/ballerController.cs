using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ballerController : MonoBehaviour
{
    //though it's kind of an enemy, it's also kind of not so it doesn't get base class
    public GameObject snowball;
    public float cooldown;
    public Transform shootPoint;
    public Animator anim;
    public int aggroDistance = 20;
    public float animOffset;
    private GameObject player;

    private void DropSnowball()
    {
        if (Vector2.Distance(player.transform.position, gameObject.transform.position) < aggroDistance)
        {
            anim.SetTrigger("drop");
            Invoke("Instan", animOffset);
        }
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        InvokeRepeating("DropSnowball", 0f, cooldown);
    }

    void Instan()
    {
        Instantiate(snowball, shootPoint.position, shootPoint.rotation);
    }
}
