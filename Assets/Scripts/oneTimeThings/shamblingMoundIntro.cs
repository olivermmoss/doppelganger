using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shamblingMoundIntro : MonoBehaviour
{
    public GameObject beast;
    public GameObject[] hands;
    private byte handsDown;
    private float timer;
    public float adjustSpeed = 20;
    public GameObject bridge;
    public Sprite[] bridgeSprites;
    public GameObject sunflower;

    private void Start()
    {
        dontDestroySave save = FindObjectOfType<dontDestroySave>();
        if(save.bossesKilled[2])
        {
            beast.GetComponent<shamblingMoundController>().endPlatforms.SetActive(true);
            beast.GetComponent<shamblingMoundController>().endPlatforms.transform.position = Vector2.zero;
            sunflower.SetActive(true);
            Destroy(bridge);
            Destroy(beast);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(AttackAhh());
    }

    private IEnumerator AttackAhh()
    {
        float twelfth = 1f / 12f;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>().canMove = false;
        var cam = GameObject.FindGameObjectWithTag("MainCamera").transform.parent.GetComponent<CameraSystem>();
        cam.xMax = 61;
        cam.xMin = 61;
        cam.yMax = 0;
        cam.yMin = 0;
        handsDown = 1;
        yield return new WaitForSeconds(2f);
        handsDown = 2;
        timer = Time.time;
        yield return new WaitForSeconds(1f);
        bridge.GetComponent<Collider2D>().enabled = false;
        bridge.GetComponent<SpriteRenderer>().sprite = bridgeSprites[0];
        yield return new WaitForSeconds(twelfth);
        bridge.GetComponent<SpriteRenderer>().sprite = bridgeSprites[1];
        yield return new WaitForSeconds(twelfth);
        bridge.GetComponent<SpriteRenderer>().sprite = bridgeSprites[0];
        yield return new WaitForSeconds(twelfth);
        bridge.GetComponent<SpriteRenderer>().sprite = bridgeSprites[2];
        yield return new WaitForSeconds(twelfth);
        bridge.GetComponent<SpriteRenderer>().sprite = bridgeSprites[3];
        yield return new WaitForSeconds(0.6666f);
        beast.transform.GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.333f);
        beast.GetComponent<shamblingMoundController>().enabled = true;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>().canMove = true;
        Destroy(bridge);
        Destroy(gameObject);
    }

    private void Update()
    {
        if(handsDown == 1)
        {
            hands[0].transform.localPosition = Vector3.MoveTowards(hands[0].transform.localPosition, new Vector3(4, 4), Time.deltaTime * adjustSpeed);
            hands[1].transform.localPosition = Vector3.MoveTowards(hands[1].transform.localPosition, new Vector3(0, 4), Time.deltaTime * adjustSpeed);
            hands[2].transform.localPosition = Vector3.MoveTowards(hands[2].transform.localPosition, new Vector3(-4, 4), Time.deltaTime * adjustSpeed);
        }
        else if (handsDown == 2)
        {
            float time = Time.time - timer;
            float height = Mathf.Abs(time + 1 % 2 - 1);
            hands[0].transform.localPosition = Vector3.MoveTowards(hands[0].transform.localPosition, new Vector3(4, 4), Time.deltaTime * adjustSpeed);
            hands[1].transform.localPosition = Vector3.MoveTowards(hands[1].transform.localPosition, new Vector3(0, 4), Time.deltaTime * adjustSpeed);
            hands[2].transform.localPosition = Vector3.MoveTowards(hands[2].transform.localPosition, new Vector3(-4, Mathf.Lerp(-6, 4, 1 - height * height)), Time.deltaTime * adjustSpeed);        }
    }

}
