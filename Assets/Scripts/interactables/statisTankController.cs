using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class statisTankController : baseInteractable
{
    public Sprite activated;
    public Sprite deactivated;
    public SpriteRenderer indicatorLight;
    public Vector2 maxes;
    public Vector2 mins;
    private AudioSource source;
    public AudioClip[] clips;
    private bool sound = false;
    public string overrideScene;
    public Vector2 overrideCoord;
    private bool ranStart = false;

    public override void Activate()
    {
        used = true;
        if(sound)
        {
            source = GetComponent<AudioSource>();
            source.clip = clips[3];
            source.Play();
        }
        indicatorLight.sprite = activated;

        if (ranStart)
        {
            var save = GameObject.FindGameObjectWithTag("save").GetComponent<dontDestroySave>();
            save.stasisScene = overrideScene;
            save.stasisCoords = overrideCoord;
            save.stasisMaxes = maxes;
            save.stasisMins = mins;
            save.SaveGame();
        }

        statisTankController[] tanks = FindObjectsOfType<statisTankController>();
        foreach(statisTankController tank in tanks)
        {
            if(tank != this)
            {
                tank.Deactivate();
            }
        }
    }

    public void Deactivate()
    {
        used = false;
        indicatorLight.sprite = deactivated;
    }

    public IEnumerator Respawn()
    {
        source = GetComponent<AudioSource>();
        //print("respawn");
        used = true;
        indicatorLight.sprite = activated;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Animator cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
        player.transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        player.GetComponent<PlayerMove>().canMove = false;
        yield return new WaitForSeconds(0.2f);
        cam.SetBool("shake1", true);
        source.clip = clips[0];
        source.Play();
        yield return new WaitForSeconds(0.2f);
        cam.SetBool("shake1", false);
        yield return new WaitForSeconds(0.8f);
        cam.SetBool("shake1", true);
        source.clip = clips[1];
        source.Play();
        yield return new WaitForSeconds(0.2f);
        cam.SetBool("shake1", false);
        yield return new WaitForSeconds(0.6f);
        gameObject.GetComponent<ParticleSystem>().Play();
        gameObject.GetComponent<Animator>().SetTrigger("break");
        player.transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        player.GetComponent<Rigidbody2D>().WakeUp();
        player.GetComponent<PlayerMove>().canMove = true;
        cam.SetBool("shake1", true);
        source.clip = clips[2];
        source.Play();
        yield return new WaitForSeconds(0.2f);
        cam.SetBool("shake1", false);
    }

    public override void Start()
    {
        base.Start();
        sound = true;
        if (string.IsNullOrEmpty(overrideScene))
            overrideScene = SceneManager.GetActiveScene().name;
        if (Mathf.Approximately(0f, overrideCoord.x) && Mathf.Approximately(0f, overrideCoord.y))
            overrideCoord = gameObject.transform.position;
        ranStart = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;

        Gizmos.DrawLine(new Vector2(mins.x - 12, maxes.y + 7), new Vector2(maxes.x + 12, maxes.y + 7));
        Gizmos.DrawLine(new Vector2(maxes.x + 12, maxes.y + 7), new Vector2(maxes.x + 12, mins.y - 7));
        Gizmos.DrawLine(new Vector2(maxes.x + 12, mins.y - 7), new Vector2(mins.x - 12, mins.y - 7));
        Gizmos.DrawLine(new Vector2(mins.x - 12, mins.y - 7), new Vector2(mins.x - 12, maxes.y + 7));
    }
}
