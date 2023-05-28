using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shamblingMoundController : MonoBehaviour
{
    public GameObject[] hands;
    private Vector3[] handPozzes = new Vector3[3];
    private Vector3[] lastHandPozzes = new Vector3[3];
    public float adjustSpeed;
    public float adjustSpeed2;
    //private SpriteRenderer[] flowerRends;
    private float time;
    public float speed;
    private float cos;
    private float sin;
    public Sprite[] sprites;
    [SerializeField]
    //private float animTime = 0;
    //public GameObject proj;
    public int attack = 1;
    private float timer;
    private SpriteRenderer sr;
    private Animator heartAnim;
    private float hitTImer;
    private bool dead = false;
    public int hp;
    public int maxHp;
    private GameObject player;
    private int bossState = 0;
    public Sprite deadHand;
    public GameObject endPlatforms;

    // Start is called before the first frame update
    void Start()
    {
        timer = Time.time;
        sr = GetComponent<SpriteRenderer>();
        heartAnim = transform.GetChild(1).GetComponent<Animator>();
        hitTImer = Time.time;
        hp = maxHp;
        player = GameObject.FindGameObjectWithTag("Player");
        attack = Random.Range(1, 5 + bossState);
    }

    // Update is called once per frame
    void Update()
    {
        if(dead)
        {
            return;
        }

        //choose new attack
        if(timer + 10f < Time.time)
        {
            timer = Time.time;
            attack = Random.Range(1, 5 + bossState);

            if (hp <= 400 && bossState == 0)
            {
                speed *= 1.2f;
                bossState = 1;
            }
            else if (hp <= 200 && bossState == 1)
            {
                speed *= 1.2f;
                bossState = 2;
            }
        }

        //tri frond
        if(attack == 1)
        {
            time = mod((Time.time * speed), 6.28318530718f);
            cos = Mathf.Cos(time);
            sin = Mathf.Sin(time);

            handPozzes[0] = new Vector3(6f * cos, 3f * sin);
            handPozzes[1] = new Vector3(6f * cos * -0.5f - 3 * sin * 0.866025403784f, 6 * cos * 0.866025403784f + 3 * sin * -0.5f);
            handPozzes[2] = new Vector3(6f * cos * -0.5f - 3 * sin * -0.866025403784f, 6 * cos * -0.866025403784f + 3 * sin * -0.5f);
        }
        //bounce down
        else if(attack == 2)
        {
            time = mod((Time.time * speed) , 6);
            float height = Mathf.Abs(time % 2 - 1);
            time = (time + 1) % 6;
            handPozzes[0] = new Vector3(-4, time < 2 ? Mathf.Lerp(-6,1,1 - height * height) : 1-height);
            handPozzes[1] = new Vector3(0, time >= 2 && time < 4 ? Mathf.Lerp(-6, 1, 1 - height * height) : 1-height);
            handPozzes[2] = new Vector3(4, time >= 4 ? Mathf.Lerp(-6, 1, 1 - height * height) : 1-height);
        }
        //lissajous 1
        else if (attack == 3)
        {
            time = mod((Time.time * speed/1.5f), 6.28318530718f);
            handPozzes[0] = new Vector3(6 * Mathf.Sin(2 * (time)), 6 * Mathf.Cos(time));
            handPozzes[1] = new Vector3(6 * Mathf.Sin(2 * (time + 2.09439510239f)), 6 * Mathf.Cos(time + 2.09439510239f));
            handPozzes[2] = new Vector3(6 * Mathf.Sin(2 * (time + 4.18879020479f)), 6 * Mathf.Cos(time + 4.18879020479f));
        }
        //star
        else if (attack == 4)
        {
            time = mod((Time.time * speed * 2), 18.8495559215f);
            handPozzes[0] = new Vector3(5 * (0.5f * Mathf.Cos(time + 4.71238898038f) + 0.75f * Mathf.Cos(0.6666666666667f * time + 4.71238898038f)), 5 * (0.5f * Mathf.Sin(time + 4.71238898038f) - 0.75f * Mathf.Sin(0.6666666666667f * time + 4.71238898038f)));
            handPozzes[1] = new Vector3(5 * (0.5f * Mathf.Cos(time + 4.71238898038f + 3.14159265359f) + 0.75f * Mathf.Cos(0.6666666666667f * (time + 3.14159265359f) + 4.71238898038f)), 5 * (0.5f * Mathf.Sin(time + 4.71238898038f + 3.14159265359f) - 0.75f * Mathf.Sin(0.6666666666667f * (time + 3.14159265359f) + 4.71238898038f)));
            handPozzes[2] = new Vector3(5 * (0.5f * Mathf.Cos(time + 4.71238898038f + 6.28318530718f) + 0.75f * Mathf.Cos(0.6666666666667f * (time + 6.28318530718f) + 4.71238898038f)), 5 * (0.5f * Mathf.Sin(time + 4.71238898038f + 6.28318530718f) - 0.75f * Mathf.Sin(0.6666666666667f * (time + 6.28318530718f) + 4.71238898038f)));
        }
        //hypotrochoid 1
        //\left(\left((a-b)\cos t+c\cos((a/b-1)t),(a-b)\sin t-c\sin((a/b-1)t)\right)\right)
        //a=-4,b=1,c=3
        else if(attack == 5)
        {
            time = mod((Time.time * speed/2f), 6.28318530718f);
            //homestuck!
            float a = -4;
            float b = 1;
            float c = 2;

            handPozzes[0] =  1.2f * new Vector3(((a - b)*Mathf.Cos(time)+c*Mathf.Cos((a/b - 1) * time)), ((a - b) * Mathf.Sin(time) - c * Mathf.Sin((a / b - 1) * time)) - 1);
            handPozzes[1] =  1.2f * new Vector3(((a - b) * Mathf.Cos(time + 2.09439510239f) + c * Mathf.Cos((a / b - 1) * (time + 2.09439510239f))), ((a - b) * Mathf.Sin(time + 2.09439510239f) - c * Mathf.Sin((a / b - 1) * (time + 2.09439510239f))) - 1);
            handPozzes[2] =  1.2f * new Vector3(((a - b) * Mathf.Cos(time + 4.18879020479f) + c * Mathf.Cos((a / b - 1) * (time + 4.18879020479f))), ((a - b) * Mathf.Sin(time + 4.18879020479f) - c * Mathf.Sin((a / b - 1) * (time + 4.18879020479f))) - 1);
        }
        //hypotrochoid 2
        else if (attack == 6)
        {
            time = mod((Time.time * speed / 2f), 6.28318530718f);
            //homestuck!
            float a = 4;
            float b = 1;
            float c = 2;

            handPozzes[0] = 1.2f * new Vector3(((a - b) * Mathf.Cos(time) + c * Mathf.Cos((a / b - 1) * time)), ((a - b) * Mathf.Sin(time) - c * Mathf.Sin((a / b - 1) * time)) - 1);
            handPozzes[1] = 1.2f * new Vector3(((a - b) * Mathf.Cos(time + 2.09439510239f) + c * Mathf.Cos((a / b - 1) * (time + 2.09439510239f))), ((a - b) * Mathf.Sin(time + 2.09439510239f) - c * Mathf.Sin((a / b - 1) * (time + 2.09439510239f))) - 1);
            handPozzes[2] = 1.2f * new Vector3(((a - b) * Mathf.Cos(time + 4.18879020479f) + c * Mathf.Cos((a / b - 1) * (time + 4.18879020479f))), ((a - b) * Mathf.Sin(time + 4.18879020479f) - c * Mathf.Sin((a / b - 1) * (time + 4.18879020479f))) - 1);
        }

        //pause
        if (timer + 2f > Time.time)
        {
            if (timer + 1.25f > Time.time)
                sr.sprite = sprites[2];
            else
                sr.sprite = sprites[1];
            hands[0].transform.localPosition = Vector3.MoveTowards(hands[0].transform.localPosition, new Vector3(4,4), Time.deltaTime * adjustSpeed);
            hands[1].transform.localPosition = Vector3.MoveTowards(hands[1].transform.localPosition, new Vector3(0, 4), Time.deltaTime * adjustSpeed);
            hands[2].transform.localPosition = Vector3.MoveTowards(hands[2].transform.localPosition, new Vector3(-4, 4), Time.deltaTime * adjustSpeed);
        }
        //not pause
        else
        {
            if (timer + 9f < Time.time)
                sr.sprite = sprites[1];
            else
                sr.sprite = sprites[0];
            hands[0].transform.localPosition = Vector3.MoveTowards(hands[0].transform.localPosition, handPozzes[0], Time.deltaTime * adjustSpeed);
            hands[1].transform.localPosition = Vector3.MoveTowards(hands[1].transform.localPosition, handPozzes[1], Time.deltaTime * adjustSpeed);
            hands[2].transform.localPosition = Vector3.MoveTowards(hands[2].transform.localPosition, handPozzes[2], Time.deltaTime * adjustSpeed);
        }

        //set rotations
        hands[0].transform.localEulerAngles = new Vector3(0, 0, Angle(hands[0].transform.localPosition - lastHandPozzes[0]));
        hands[1].transform.localEulerAngles = new Vector3(0, 0, Angle(hands[1].transform.localPosition - lastHandPozzes[1]));
        hands[2].transform.localEulerAngles = new Vector3(0, 0, Angle(hands[2].transform.localPosition - lastHandPozzes[2]));

        lastHandPozzes[0] = hands[0].transform.localPosition;
        lastHandPozzes[1] = hands[1].transform.localPosition;
        lastHandPozzes[2] = hands[2].transform.localPosition;
    }

    float mod(float x, float m)
    {
        float r = x % m;
        return r < 0 ? r + m : r;
    }

    public static float Angle(Vector3 vector2)
    {
        return (360 - 10 * Mathf.Round(Mathf.Atan2(vector2.x, vector2.y) * Mathf.Rad2Deg / 10)) % 360;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("playerAttack") && Time.time > hitTImer + 0.1f && !dead && this.enabled)
        {
            hitTImer = Time.time;
            hp -= player.GetComponent<playerAttack>().attackDamage;

            if (hp <= 0)
            {
                StartCoroutine(Die());
            }

            heartAnim.SetTrigger("hit");
        }
    }

    IEnumerator Die()
    {
        dontDestroySave save = FindObjectOfType<dontDestroySave>();
        save.bossesKilled[2] = true;
        var cam = GameObject.FindGameObjectWithTag("MainCamera");
        var camSys = cam.transform.parent.GetComponent<CameraSystem>();
        dead = true;
        foreach (GameObject hand in hands)
        {
            hand.GetComponent<damagePlayer>().yes = false;
            hand.GetComponent<SpriteRenderer>().sprite = deadHand;
            hand.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            hand.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 500);
        }
        camSys.xMax = 159;
        camSys.xMin = 0;
        camSys.yMax = 15;
        camSys.yMin = 0;
        heartAnim.SetTrigger("die");
        GetComponent<ParticleSystem>().Play();
        Time.timeScale = 0.1f;
        cam.GetComponent<Animator>().SetBool("shake1", true);
        sr.sprite = sprites[3];
        yield return new WaitForSeconds(0.1f);
        Time.timeScale = 1;
        cam.GetComponent<Animator>().SetBool("shake1", false);
        for(int i=4; i<=9; i++)
        {
            sr.sprite = sprites[i];
            yield return new WaitForSeconds(0.166666f);
        }
        endPlatforms.SetActive(true);
        iTween.MoveBy(endPlatforms, iTween.Hash(
                "y", 5f,
                "time", 2f,
                "easetype", "easeInOutSine"
            ));
    }
}
