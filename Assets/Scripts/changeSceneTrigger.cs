using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class changeSceneTrigger : MonoBehaviour
{
    public string sceneToGoTo;
    public bool playAudio = false;
    public AudioSource aud;
    public float waitTime = 0;

    public Vector2 NewScenePos;
    public bool facingRight;
    public Vector2 camMins;
    public Vector2 camMaxes;

    private screenWipeController wipe;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (playAudio)
                aud.Play();

            FindObjectOfType<PlayerMove>().canMove = false;
            StartCoroutine(SceneChange());
            GameObject.FindGameObjectWithTag("Player").GetComponent<playerHealth>().invincible = true;
        }
    }

    IEnumerator SceneChange()
    {
        dontDestroySave save = GameObject.FindGameObjectWithTag("save").GetComponent<dontDestroySave>();

        yield return new WaitForSeconds(waitTime);

        var box = GetComponent<BoxCollider2D>();

        if (wipe != null && box != null)
        {
            var player = GameObject.FindGameObjectWithTag("Player").transform.position;
            var vertical = box.size.x >= box.size.y;
            var posDiff = (vertical ? gameObject.transform.position.y - player.y : gameObject.transform.position.x - player.x) < 0;
            wipe.WipeOn(vertical, posDiff);
            save.verticalWipe = vertical;
            save.wipePosDiff = posDiff;
        }
        else
        {
            Debug.LogError("no box and/or wipe");
        }
        save.ChangeScene(sceneToGoTo, NewScenePos, facingRight, camMins, camMaxes);
    }

    private void Start()
    {
        wipe = GameObject.FindGameObjectWithTag("screenWipe").GetComponent<screenWipeController>();
    }
}
