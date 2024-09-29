using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class shroomBossController : MonoBehaviour
{
    public Animator anim;
    public int maxHealth;
    public int hp;
    private float timer;
    private GameObject player;
    public GameObject offProj;
    public GameObject onProj;
    public GameObject blooProj;
    public Transform spawner;
    public float projCooldown;
    private bool dead = false;
    public GameObject barrier;
    public Sprite openBarrier;

    //timer is for being hit repeatedly
    //timer2 is for spawning little spiders
    //timer3 is for going up and down;
    //timer4 is for screaming

    private void Start()
    {
        hp = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player");
        ShroomWallAttack();
    }

    public void TakeDamage()
    {
        timer = Time.time;
        hp -= player.GetComponent<playerAttack>().attackDamage;

        if (hp <= 0)
        {
            //Die();
        }
        else
        {
            anim.SetTrigger("hit");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("playerAttack") && Time.time > timer + 0.1f && !dead)
        {
            TakeDamage();
        }
    }

    /*void Die()
    {
        basicProjectile[] bugs = FindObjectsOfType<basicProjectile>();
        foreach (basicProjectile bug in bugs)
        {
            Destroy(bug.gameObject);
        }
        anim.SetTrigger("die");
        dead = true;
        legs = FindObjectsOfType<spiderLegController>();
        foreach (spiderLegController leg in legs)
        {
            StartCoroutine(leg.Die());
        }
        fakeHead.SetActive(true);
        fakeHead.transform.position = gameObject.transform.position;
        barrier1.GetComponent<BoxCollider2D>().enabled = false;
        barrier1.GetComponent<Animator>().SetBool("appear", false);
        barrier2.GetComponent<BoxCollider2D>().enabled = false;
        barrier2.GetComponent<Animator>().SetBool("appear", false);
        FindObjectOfType<CameraSystem>().xMin = -28;
        FindObjectOfType<CameraSystem>().xMax = 22;
        FindObjectOfType<dontDestroyMusic>().StopBGM();
        Time.timeScale = 0.1f;
        timer3 = Time.time;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>().SetBool("shake1", true);
        dontDestroySave save = FindObjectOfType<dontDestroySave>();
        save.bossesKilled[0] = true;
        save.bossQuick = false;
        save.SaveGame();
    }*/

    public void ShroomWallAttack()
    {
        timer = Time.time;
        GameObject proj;
        int r = Random.Range(0, 3);
        switch (r) {
            case 0:
                for (int i = 0; i < 8; i++)
                {
                    if (i > 4)
                        proj = onProj;
                    else if (i > 1)
                        proj = offProj;
                    else
                        proj = blooProj;
                    Instantiate(proj, new Vector2(spawner.position.x, spawner.position.y + 1.5f * i), Quaternion.Euler(new Vector3(0, 0, 90)));
                }
                break;
            case 1:
                for (int i = 0; i < 8; i++)
                {
                    if (i > 4)
                        proj = offProj;
                    else if (i > 1)
                        proj = onProj;
                    else
                        proj = blooProj;
                    Instantiate(proj, new Vector2(spawner.position.x, spawner.position.y + 1.5f * i), Quaternion.Euler(new Vector3(0, 0, 90)));
                }
                break;
            case 2:
                for (int i = 0; i < 8; i++)
                {
                        proj = onProj;
                    Instantiate(proj, new Vector2(spawner.position.x, spawner.position.y + 1.5f * i), Quaternion.Euler(new Vector3(0, 0, 90)));
                }
                break;
            default:
                Debug.LogWarning("Generated bad number: " + r);
                break;
        }
    }

    private void Update()
    {
        if (timer <= Time.time - projCooldown && !dead)
        {
            ShroomWallAttack();
        }
    }
}
