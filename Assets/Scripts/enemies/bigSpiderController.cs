using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class bigSpiderController : MonoBehaviour
{
    public bool up = false;
    public Animator anim;
    public int maxHealth;
    public int hp;
    private float timer;
    private GameObject player;
    public GameObject tinySpiderPrefab;
    public Transform[] spawners;
    private float timer2;
    public float tinySpiderCooldown;
    private int spawnerNum = 2;
    public float timer3;
    public float upAndDownTime;
    public GameObject legPrefab;
    private spiderLegController[] legs;
    private int headPos;
    private int[] legPoses;
    private bool dead = false;
    public GameObject fakeHead;
    public GameObject barrier1;
    public GameObject barrier2;
    public Sprite openBarrier;
    private int screamCounter = 0;
    private float timer4;

    //timer is for being hit repeatedly
    //timer2 is for spawning little spiders
    //timer3 is for going up and down;
    //timer4 is for screaming

    private void Start()
    {
        hp = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player");
        timer2 = 999999;
        upAndDownTime = Random.value * 10 + 5;
        timer3 = 9999999;
    }

    public void TakeDamage()
    {
            timer = Time.time;
            hp -= player.GetComponent<playerAttack>().attackDamage;

            if(hp <= 0)
            {
                Die();
            } else if (hp <= 150)
            {
                tinySpiderCooldown = 0.5f;
                spawnerNum = 9;
                if (screamCounter  == 4)
                {
                    screamCounter += 1;
                    anim.SetBool("screaming", true);
                    timer4 = Time.time;
                    FindObjectOfType<bigSpiderIntro>().gameObject.GetComponent<AudioSource>().Play();
                }
                else
                {
                    anim.SetTrigger("hit");
                }
            } else if (hp <= 300)
            {
                tinySpiderCooldown = 1;
                spawnerNum = 7;
                if (screamCounter  == 2)
                {
                    screamCounter += 1;
                    anim.SetBool("screaming", true);
                    timer4 = Time.time;
                    FindObjectOfType<bigSpiderIntro>().gameObject.GetComponent<AudioSource>().Play();
                }
                else
                {
                    anim.SetTrigger("hit");
                }
            } else if (hp <= 450)
            {
                tinySpiderCooldown = 1.25f;
                spawnerNum = 4;
                if (screamCounter  == 0)
                {
                    screamCounter += 1;
                    anim.SetBool("screaming", true);
                    timer4 = Time.time;
                    FindObjectOfType<bigSpiderIntro>().gameObject.GetComponent<AudioSource>().Play();
                }
                else
                {
                    anim.SetTrigger("hit");
                }
            }
            else
            {
                anim.SetTrigger("hit");
            }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("playerAttack") && Time.time > timer + 0.1f && up && !dead)
        {
            TakeDamage();
        }
    }

    void Die()
    {
        basicProjectile[] bugs = FindObjectsOfType<basicProjectile>();
        foreach(basicProjectile bug in bugs)
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
    }

    public void SpiderCrawlAttack()
    {
        timer2 = Time.time;
        Instantiate(tinySpiderPrefab, spawners[Random.Range(0, spawnerNum)]);
    }

    private void Update()
    {
        if(timer2 <= Time.time - tinySpiderCooldown && !dead)
        {
            SpiderCrawlAttack();
        }
        if(timer3 <= Time.time - upAndDownTime && up && !dead)
        {
            upAndDownTime = Random.value * 3 + 1;
            //upAndDownTime = 0.1f;
            up = false;
            anim.SetBool("leave", true);
            timer3 = Time.time;
        }
        else if (timer3 <= Time.time - upAndDownTime && !up && !dead)
        {
            upAndDownTime = Random.value * 10 + 5;
            //upAndDownTime = 1;
            up = true;
            anim.SetBool("leave", false);
            timer3 = Time.time;

            legs = FindObjectsOfType<spiderLegController>();

            CalculateHeadPos();

            gameObject.transform.position = new Vector2(headPos, gameObject.transform.position.y);

            int legPos = Random.Range(-8, 4);
            if(legPos >= gameObject.transform.position.x - 2.5f)
            {
                legPos += 5;
            }

            Instantiate(legPrefab, new Vector2(legPos, 0), Quaternion.identity);

            foreach(spiderLegController leg in legs) {
                leg.CalculateLook();
            }
        }
        else if (timer3 <= Time.time - 2.3f && dead)
        {
            GetComponent<AudioSource>().Play();
            timer3 = 999999;
        }
        else if (timer3 <= Time.time - 0.1f && dead)
        {
            Time.timeScale = 1;
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>().SetBool("shake1", false);
        }
        if (timer4 <= Time.time - 1 && !dead && screamCounter % 2 == 1)
        {
            anim.SetBool("screaming", false);
            screamCounter += 1;
        }
    }

    void CalculateHeadPos()
    {
        legPoses = new int[legs.Length];
        for (int i = 0; i < legPoses.Length; i++)
        {
            legPoses[i] = Mathf.RoundToInt(legs[i].gameObject.transform.position.x);
        }
        int[] allNum = new int[] { -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5 };
        int[] headPoses = allNum.Except(legPoses).ToArray();
        for (int i = 0; i < legPoses.Length; i++)
        {
            legPoses[i] = Mathf.RoundToInt(legs[i].gameObject.transform.position.x) + 1;
        }
        headPoses = headPoses.Except(legPoses).ToArray();
        for (int i = 0; i < legPoses.Length; i++)
        {
            legPoses[i] = Mathf.RoundToInt(legs[i].gameObject.transform.position.x) -1;
        }
        headPoses = headPoses.Except(legPoses).ToArray();
        for (int i = 0; i < legPoses.Length; i++)
        {
            legPoses[i] = Mathf.RoundToInt(legs[i].gameObject.transform.position.x) + 2;
        }
        headPoses = headPoses.Except(legPoses).ToArray();
        for (int i = 0; i < legPoses.Length; i++)
        {
            legPoses[i] = Mathf.RoundToInt(legs[i].gameObject.transform.position.x) - 2;
        }
        headPoses = headPoses.Except(legPoses).ToArray();
        headPos = headPoses[Random.Range(0, headPoses.Length)];
    }
}
