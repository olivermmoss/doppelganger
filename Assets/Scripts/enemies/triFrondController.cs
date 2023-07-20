using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triFrondController : baseEnemy
{
    public GameObject[] flowers;
    private SpriteRenderer[] flowerRends;
    private float time;
    public float speed;
    private float cos;
    private float sin;
    public Sprite[] sprites;
    [SerializeField]
    private int animTime = 0;
    public GameObject proj;
    public float aggroDistance;

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(gameObject.transform.position, player.transform.position) < aggroDistance && !dead)
        {
            time = (Time.time * speed) % 6.28318530718f;
            cos = Mathf.Cos(time);
            sin = Mathf.Sin(time);

            flowers[2].transform.localPosition = new Vector3(2f * cos, sin);
            flowers[1].transform.localPosition = new Vector3(2f * cos * -0.5f - sin * 0.866025403784f, 2 * cos * 0.866025403784f + sin * -0.5f);
            flowers[0].transform.localPosition = new Vector3(2f * cos * -0.5f - sin * -0.866025403784f, 2 * cos * -0.866025403784f + sin * -0.5f);

            //flowers[0].transform.localPosition = new Vector3(2f * cos * Mathf.Cos(0) - sin * Mathf.Sin(0), 2 * cos * Mathf.Sin(0) + sin * Mathf.Cos(0));

            //print(time);

            if(time > 0 && animTime == 0 && time < 0.08333333333f)
            {
                flowerRends[0].sprite = sprites[1];
                animTime = 1;
            }
            else if (time > 0.08333333333f && animTime == 1)
            {
                flowerRends[0].sprite = sprites[2];
                var curProj = Instantiate(proj, flowers[0].transform.position, Quaternion.identity);
                curProj.GetComponent<SpriteRenderer>().color = flowerRends[0].color;
                curProj.transform.up = player.transform.position - curProj.transform.position;
                animTime = 2;
            }
            else if (time > 0.16666666f && animTime == 2)
            {
                flowerRends[0].sprite = sprites[0];
                animTime = 3;
            }
            else if (time > 2 && animTime == 3)
            {
                flowerRends[1].sprite = sprites[1];
                animTime = 4;
            }
            else if (time > 2.08333333333f && animTime == 4)
            {
                flowerRends[1].sprite = sprites[2];
                var curProj = Instantiate(proj, flowers[1].transform.position, Quaternion.identity);
                curProj.GetComponent<SpriteRenderer>().color = flowerRends[1].color;
                curProj.transform.up = player.transform.position - curProj.transform.position;
                animTime = 5;
            }
            else if (time > 2.16666666f && animTime == 5)
            {
                flowerRends[1].sprite = sprites[0];
                animTime = 6;
            }
            else if (time > 4 && animTime == 6)
            {
                flowerRends[2].sprite = sprites[1];
                animTime = 7;
            }
            else if (time > 3.08333333333f && animTime == 7)
            {
                flowerRends[2].sprite = sprites[2];
                var curProj = Instantiate(proj, flowers[2].transform.position, Quaternion.identity);
                curProj.GetComponent<SpriteRenderer>().color = flowerRends[2].color;
                curProj.transform.up = player.transform.position - curProj.transform.position;
                animTime = 8;
            }
            else if (time > 4.16666666f && animTime == 8)
            {
                flowerRends[2].sprite = sprites[0];
                animTime = 0;
            }
        }
    }

    public override IEnumerator Die()
    {
        if (dieSource != null)
            dieSource.Play();
        if (animator != null)
            animator.SetBool("isDead", true);
        gameObject.layer = 10;

        foreach (GameObject flower in flowers)
        {
            flower.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            flower.GetComponent<SpriteRenderer>().color = new Color(0.38039215686f, 0.38039215686f, 0.38039215686f);
        }

        for(int i = 0; i<3; i++)
        {
            var lilBit = gameObject.transform.GetChild(i);

            for (int j = 0; j < lilBit.transform.childCount; j++)
            {
                lilBit.transform.GetChild(j).GetComponent<SpriteRenderer>().color = new Color(0.45882352941f, 0.45882352941f, 0.45882352941f);
            }
        }

        yield return new WaitForSeconds(deathTime);
        Destroy(gameObject);
    }

    private void Awake()
    {
        flowerRends = new SpriteRenderer[flowers.Length];
        for(int i = 0; i<flowers.Length; i++)
        {
            flowerRends[i] = flowers[i].GetComponent<SpriteRenderer>();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(gameObject.transform.position, aggroDistance);
    }
}
