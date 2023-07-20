using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class remorhazController : MonoBehaviour
{
    public Animator anim;
    public int maxHealth;
    public int hp;
    private float hitTimer;
    private GameObject player;
    public float upSpeed;
    private bool dead = false;
    [SerializeField]
    private bool movingUp = false;
    private float timeBtwnJump = 2;
    private float waitTimer;
    private bool grounded = true;
    public GameObject platformPrefab;
    public Transform[] pipes;
    public Vector2[] positions;
    public float sideSpeed;
    private bool movingSideways = false;
    public Transform shootPoint;
    public GameObject fireballObject;
    private int attacksDone = 0;
    public GameObject iciclePrefab;
    public SpriteRenderer hitRend;
    private SpriteRenderer thisRend;
    private float timer3 = float.PositiveInfinity;
    public AudioSource dieSlashSource;

    private void Start()
    {
        hp = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player");
        waitTimer = Time.time;
        thisRend = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("playerAttack") && Time.time > hitTimer + 0.1f && !dead)
        {
            hitTimer = Time.time;
            hp -= player.GetComponent<playerAttack>().attackDamage;

            StartCoroutine(hitAnim());

            if (hp <= 0)
            {
                Die();
            }
        }
    }

    IEnumerator hitAnim()
    {
        hitRend.enabled = true;
        thisRend.enabled = false;
        yield return new WaitForSeconds(0.083333f);
        hitRend.enabled = false;
        thisRend.enabled = true;
    }

    void Die()
    {
        anim.SetTrigger("die");
        movingUp = false;
        transform.position = new Vector2(transform.position.x, 2);
        timeBtwnJump = float.PositiveInfinity;
        waitTimer = float.PositiveInfinity;
        grounded = false;
        GetComponent<BoxCollider2D>().enabled = false;
        var intro = GetComponent<remorhazIntro>();
        intro.iceDoors[0].SetBool("down", false);
        intro.iceDoors[0].gameObject.GetComponent<BoxCollider2D>().enabled = false;
        intro.iceDoors[1].enabled = true;
        intro.iceDoors[1].SetBool("down", false);
        intro.iceDoors[1].gameObject.GetComponent<BoxCollider2D>().enabled = false;
        StopAllCoroutines();

        var cam = FindObjectOfType<CameraSystem>();
        cam.xMin = 0;
        cam.xMax = 130;
        cam.yMin = 0;
        cam.yMax = 100;
        FindObjectOfType<dontDestroyMusic>().StopBGM();
        Time.timeScale = 0.1f;
        timer3 = Time.time;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>().SetBool("shake1", true);
        dontDestroySave save = FindObjectOfType<dontDestroySave>();
        save.bossesKilled[1] = true;
        save.bossQuick = false;
        save.SaveGame();
        hitRend.enabled = false;
        thisRend.enabled = true;
        dieSlashSource.Play();

        foreach(remorhazFireball fireball in FindObjectsOfType<remorhazFireball>())
        {
            fireball.Poof();
        }
    }

    IEnumerator Jump()
    {
        grounded = false;
        anim.SetTrigger("jump");
        yield return new WaitForSeconds(0.583f);
        movingUp = true;
        yield return new WaitForSeconds(1f);
        movingUp = false;
        timeBtwnJump = 0f;
        waitTimer = Time.time;
    }
    IEnumerator Land()
    {
        int location = Random.Range(0, 7);
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        if (location < 3)
            transform.localScale = new Vector2(-1, 1);
        else if (location > 3)
            transform.localScale = new Vector2(1, 1);
        grounded = true;
        transform.position = new Vector2(pipes[location].position.x, 14);
        movingUp = true;
        upSpeed = -upSpeed;
        yield return new WaitForSeconds(1f);
        movingUp = false;
        upSpeed = -upSpeed;
        transform.position = new Vector2(pipes[location].position.x, -2);
        anim.SetTrigger("land");
        timeBtwnJump = Random.Range(1f, 3f);
        waitTimer = Time.time;
    }

    private void FixedUpdate()
    {
        if(movingUp)
        {
            gameObject.transform.Translate(new Vector3(0f, Time.deltaTime * upSpeed, 0f));
        }
        else if(movingSideways)
        {
            gameObject.transform.Translate(new Vector3(0f, Time.deltaTime * upSpeed, 0f));
        }
    }

    private void Update()
    {
        if(!movingUp && Time.time > waitTimer + timeBtwnJump )
        {
            if(grounded)
            {
                ChooseNextAttack();
                waitTimer = Time.time + 100f;
            }
            else
            {
                StartCoroutine(Land());
                waitTimer = Time.time + 100f;
            }
        }
        if(timer3 <= Time.time - 0.1f)
        {
            Time.timeScale = 1;
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>().SetBool("shake1", false);
        }
    }

    void ChooseNextAttack()
    {
        if (attacksDone >= 3)
        {
            attacksDone = 0;
            StartCoroutine(Jump());
        }
        else
        {
            attacksDone++;
            int attack = Random.Range(1, 5);
            switch (attack)
            {
                case 1:
                    StartCoroutine(Fireball(1));
                    break;
                case 2:
                    StartCoroutine(Fireball(3));
                    break;
                case 3:
                    StartCoroutine(Fireball(5));
                    break;
                case 4:
                    StartCoroutine(IcicleAttack());
                    break;
                default:
                    Debug.LogError("generated attack that doesn't exist");
                    break;
            }
        }
    }
    IEnumerator Fireball(int balls)
    {
        for (int i = 0; i < balls; i++)
        {
            anim.SetTrigger("fireball");
            yield return new WaitForSeconds(.75f);
            Instantiate(fireballObject, shootPoint.position, Quaternion.Euler(0, 0, transform.localScale.x > 0 ? 90 : 270));
            yield return new WaitForSeconds(.25f);
        }
        timeBtwnJump = Random.Range(1f, 3f);
        waitTimer = Time.time;
    }

    IEnumerator IcicleAttack()
    {
        anim.SetTrigger("scream");
        GetComponent<AudioSource>().Play();
        foreach(Transform pipe in pipes)
        {
            if(Random.Range(0, 5) != 0)
            {
                Instantiate(iciclePrefab, pipe.position + new Vector3(0, 0.75f), Quaternion.identity);
            }
            yield return null;
            continue;
        }
        timeBtwnJump = Random.Range(1f, 3f);
        waitTimer = Time.time;
    }
}
