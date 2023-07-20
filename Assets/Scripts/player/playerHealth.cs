using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

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

    private InputActionAsset actions;

    public AudioClip fallToDeath;
    private AudioSource src;
    public ParticleSystem ps;

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

        actions = gameObject.GetComponent<PlayerMove>().actions;
        actions.FindActionMap("gameplay").FindAction("attack").performed += TryReset;
        src = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(gameObject.transform.position.y <= dieDepth && !invincible && !dead)
        {
            src.clip = fallToDeath;
            src.Play();
            Die();
        }
    }

    private void TryReset(InputAction.CallbackContext context = new InputAction.CallbackContext())
    {
        if (dead)
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
    }

    private void OnDisable()
    {
        // for the "jump" action, we add a callback method for when it is performed
        actions.FindActionMap("gameplay").FindAction("attack").performed -= TryReset;
    }

    public void TakeDamage(int damage, GameObject damager)
    {
        if(Time.time < hitTimer + hitCooldown || invincible)
        {
            return;
        }

        health -= damage;
        if (health < 0)
            health = 0;
        anim.SetTrigger("hit");
        UpdateHearts();
        
        StartCoroutine(TimeFreeze());

        if(health <= 0)
        {
            Die();
        }
        else
        {
            ps.Play(false);

        }
        Vector3 dir = damager.transform.position - transform.position;
        dir = -dir.normalized;
        gameObject.GetComponent<Rigidbody2D>().AddForce(dir * 300);

        hitTimer = Time.time;
    }

    public void Die()
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

        LeanTween.value(gameObject, setSpriteColor, new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), 2f);
    }

    public void UpdateHearts()
    {
        //print("update hearts");

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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector2(-999, dieDepth), new Vector2(999, dieDepth));
    }
}
