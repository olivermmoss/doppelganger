using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class baseSpawener : MonoBehaviour
{
    //though it's kind of an enemy, it's also kind of not so it doesn't get base class
    public GameObject obj;
    public float cooldown;
    public Transform shootPoint;
    public Animator anim;
    public int aggroDistance = 20;
    public float animOffset;
    private GameObject player;

    private void Spawn()
    {
        if (Vector2.Distance(player.transform.position, gameObject.transform.position) < aggroDistance)
        {
            if(anim != null)
            {
                anim.SetTrigger("drop");
                Invoke("Instan", animOffset);
            }
            else
            {
                Instan();
            }
        }
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        InvokeRepeating("Spawn", 0f, cooldown);
    }

    void Instan()
    {
        Instantiate(obj, shootPoint.position, shootPoint.rotation);
    }
}
