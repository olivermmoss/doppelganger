using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class remorhazIntro : MonoBehaviour
{
    public Animator beast;
    public float breakTime;
    public AudioClip bossMusic;
    private dontDestroyMusic theAM;
    private CameraSystem cam;
    private GameObject player;
    public Animator[] iceDoors;
    public ParticleSystem iceShatter;
    public Vector2[] positions;
    private bool done = false;
    public AudioSource breakSource;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("fireball") && !done)
        {
            collision.gameObject.GetComponent<fireballController>().Poof();
            theAM = FindObjectOfType<dontDestroyMusic>();
            cam = FindObjectOfType<CameraSystem>();
            player = GameObject.FindGameObjectWithTag("Player");

            player.GetComponent<PlayerMove>().Immobilize();

            theAM.StopBGM();
            //gameObject.GetComponent<BoxCollider2D>().enabled = false;
            iceDoors[0].SetBool("down", true);
            iceDoors[0].gameObject.GetComponent<BoxCollider2D>().enabled = true;
            //webDoor2.SetBool("appear", true);
            //webDoor2.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            cam.xMin = 75;

            if (!FindObjectOfType<dontDestroySave>().bossQuick)
                StartCoroutine("Cutscene");
            else
                StartCoroutine("CutsceneQuick");
        }
    }

    private IEnumerator Cutscene()
    {
        beast.SetTrigger("unfreeze");
        yield return new WaitForSeconds(1.6f);
        Animator canim = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
        canim.SetBool("shake1", true);
        yield return new WaitForSeconds(0.4f);
        breakSource.Play();
        gameObject.transform.position = positions[0];
        yield return new WaitForSeconds(0.1f);
        canim.SetBool("shake1", false);
        iceShatter.Play();
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(breakTime);
        theAM.ChangeBGM(bossMusic);
        player.GetComponent<PlayerMove>().Unimmobilize();
        FindObjectOfType<dontDestroySave>().bossQuick = true;
        beast.gameObject.GetComponent<remorhazController>().enabled = true;
        this.enabled = false;
        done = true;
    }

    private IEnumerator CutsceneQuick()
    {
        beast.SetTrigger("quickStart");
        gameObject.transform.position = positions[0];
        Animator canim = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
        canim.SetBool("shake1", true);
        yield return new WaitForSeconds(0.5f);
        breakSource.Play();
        canim.SetBool("shake1", false);
        iceShatter.Play();
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(breakTime);
        theAM.ChangeBGM(bossMusic);
        player.GetComponent<PlayerMove>().Unimmobilize();
        beast.gameObject.GetComponent<remorhazController>().enabled = true;
        this.enabled = false;
        done = true;
    }

    private void Start()
    {
        if (FindObjectOfType<dontDestroySave>().bossesKilled[1])
        {
            iceDoors[0].SetBool("down", false);
            iceDoors[0].gameObject.GetComponent<BoxCollider2D>().enabled = false;
            iceDoors[1].enabled = true;
            iceDoors[1].SetBool("down", false);
            iceDoors[1].gameObject.GetComponent<BoxCollider2D>().enabled = false;

            var cam = FindObjectOfType<CameraSystem>();
            cam.xMin = 0;
            cam.xMax = 130;
            cam.yMin = 0;
            cam.yMax = 56;

            Destroy(gameObject);
        }
    }
}
