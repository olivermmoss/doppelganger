using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class playerHealth : MonoBehaviour
{
    public int maxHealth;
    public int health = 10;
    public Sprite deadSprite;
    public SpriteRenderer restartUI;
    public bool dead;
    private Animator anim;

    public Image[] hearts;
    public Sprite heartFull;
    public Sprite heartEmpty;

    private float hitTimer;
    public float hitCooldown;

    public float dieDepth = -100;
    public bool invincible = false;

    void Start()
    {
        if(health == 10)
            health = maxHealth;

        anim = gameObject.transform.GetChild(1).GetChild(0).GetComponent<Animator>();
        var heartsTemp = GameObject.FindGameObjectWithTag("healthUI").transform;
        hearts = new Image[heartsTemp.childCount];
        for(int i = 0; i<hearts.Length; i++)
        {
            hearts[i] = heartsTemp.GetChild(i).GetComponent<Image>();
        }

        UpdateHearts();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X) && dead)
		{
            GameObject.FindGameObjectWithTag("save").GetComponent<dontDestroySave>().playerDead = true;
            if (GameObject.FindGameObjectWithTag("save").GetComponent<dontDestroySave>().stasisScene != null && GameObject.FindGameObjectWithTag("save").GetComponent<dontDestroySave>().stasisScene != "")
            {
                SceneManager.LoadScene(GameObject.FindGameObjectWithTag("save").GetComponent<dontDestroySave>().stasisScene);
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
		}

        if(gameObject.transform.position.y <= dieDepth && !invincible)
        {
            Die();
        }
    }

    public void TakeDamage(int damage, GameObject damager)
    {
        if(Time.time < hitTimer + hitCooldown || invincible)
        {
            return;
        }

        health -= damage;
        anim.SetTrigger("hit");
        UpdateHearts();
        
        StartCoroutine(TimeFreeze());

        if(health <= 0)
        {
            Die();
        }
        else
        {
            gameObject.GetComponent<ParticleSystem>().Play();

        }
        Vector3 dir = damager.transform.position - transform.position;
        dir = -dir.normalized;
        gameObject.GetComponent<Rigidbody2D>().AddForce(dir * 300);

        hitTimer = Time.time;
    }

    void Die()
    {
        if(dead)
            return;

        dead = true;
        health = 0;
        UpdateHearts();

        anim.enabled = false;
        gameObject.GetComponent<PlayerMove>().enabled = false;
        gameObject.GetComponent<playerAttack>().enabled = false;
        gameObject.GetComponent<playerAttack>().attackTrigger.gameObject.SetActive(false);
        anim.gameObject.GetComponent<SpriteRenderer>().sprite = deadSprite;

        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

        iTween.ValueTo(restartUI.gameObject, iTween.Hash(
		"time", 2f,
		"from", new Color(1, 1, 1, 0),
		"to", new Color(1, 1, 1, 1),
		"onupdate", "setSpriteColor",
		"onupdatetarget", this.gameObject
	    ));
    }

    public void UpdateHearts()
    {
        print("update hearts");

        foreach(Image heart in hearts)
        {
            heart.sprite = heartFull;
            heart.color = new Vector4(1,1,1,1);
        }
        for(int i = maxHealth; i < hearts.Length; i++)
        {
            hearts[i].color = new Vector4(1,1,1,0);
        }
        for (int i = health; i < maxHealth; i++)
        {
            hearts[i].sprite = heartEmpty;
        }
    }

    void setSpriteColor(Color c)
{
	restartUI.color = c;
}

    IEnumerator TimeFreeze()
    {
        Time.timeScale = 0.1f;
        yield return new WaitForSeconds(0.01f);
        Time.timeScale = 1;
    }
}
