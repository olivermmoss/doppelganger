using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bigSpiderIntro : MonoBehaviour
{
    public Animator head;
    public float breakTime;
    public AudioClip bossMusic;
    private dontDestroyMusic theAM;
    private GameObject player;
    public Sprite closedWebDoor;
    public Animator webDoor;
    public Animator webDoor2;
    public GameObject legPrefab;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            theAM = FindObjectOfType<dontDestroyMusic>();
            player = collision.gameObject;

            player.GetComponent<PlayerMove>().canMove = false;
            player.GetComponent<playerAttack>().enabled = false;

            theAM.StopBGM();
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            webDoor.SetBool("appear", true);
            webDoor.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            webDoor2.SetBool("appear", true);
            webDoor2.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            FindObjectOfType<CameraSystem>().xMin = 0;
            FindObjectOfType<CameraSystem>().xMax = 0;

            if(!FindObjectOfType<dontDestroySave>().bossQuick)
                StartCoroutine("Cutscene");
            else
                StartCoroutine("CutsceneQuick");
        }
    }

    private IEnumerator Cutscene()
    {
        head.SetBool("leave", false);
        yield return new WaitForSeconds(breakTime);
        Instantiate(legPrefab, new Vector2(4f, 0), Quaternion.identity);
        yield return new WaitForSeconds(breakTime);
        Instantiate(legPrefab, new Vector2(-4, 0), Quaternion.identity);
        yield return new WaitForSeconds(breakTime);
        Instantiate(legPrefab, new Vector2(6f, 0), Quaternion.identity);
        yield return new WaitForSeconds(breakTime);
        Instantiate(legPrefab, new Vector2(-6f, 0), Quaternion.identity);
        yield return new WaitForSeconds(breakTime * 2);
        head.SetBool("screaming", true);
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(breakTime * 3);
        head.SetBool("screaming", false);
        yield return new WaitForSeconds(breakTime);
        theAM.ChangeBGM(bossMusic);
        player.GetComponent<PlayerMove>().canMove = true;
        player.GetComponent<playerAttack>().enabled = true;
        FindObjectOfType<dontDestroySave>().bossQuick = true;
        head.gameObject.GetComponent<bigSpiderController>().SpiderCrawlAttack();
        head.gameObject.GetComponent<bigSpiderController>().timer3 = Time.time;
        head.gameObject.GetComponent<bigSpiderController>().upAndDownTime = Random.value * 10 + 5;
        head.gameObject.GetComponent<bigSpiderController>().up = true;
    }

    private IEnumerator CutsceneQuick()
    {
        head.SetBool("leave", false);
        Instantiate(legPrefab, new Vector2(4f, 0), Quaternion.identity);
        Instantiate(legPrefab, new Vector2(-4, 0), Quaternion.identity);
        Instantiate(legPrefab, new Vector2(6f, 0), Quaternion.identity);
        Instantiate(legPrefab, new Vector2(-6f, 0), Quaternion.identity);
        yield return new WaitForSeconds(breakTime * 2);
        theAM.ChangeBGM(bossMusic);
        player.GetComponent<PlayerMove>().canMove = true;
        player.GetComponent<playerAttack>().enabled = true;
        head.gameObject.GetComponent<bigSpiderController>().SpiderCrawlAttack();
        head.gameObject.GetComponent<bigSpiderController>().timer3 = Time.time;
        head.gameObject.GetComponent<bigSpiderController>().upAndDownTime = Random.value * 10 + 5;
        head.gameObject.GetComponent<bigSpiderController>().up = true;
    }

    private void Start()
    {
        if(FindObjectOfType<dontDestroySave>().bossesKilled[0])
        {
            Destroy(gameObject);
        }
    }
}
