using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class maxHealthUp : baseInteractable
{
    public int healthUpIndex;
    private dontDestroySave save;
    private Vector2 approachPos;
    private RectTransform rect;
    private float Timer;
    public Sprite hpDing;
    public AudioSource startSFX;
    public AudioSource finishSFX;

    public override void Activate()
    {
        if (used)
            return;

        save.maxHealthIncreases[healthUpIndex] = true;
        save.playerMaxHealth++;
        save.SaveGame();
        var health = GameObject.FindGameObjectWithTag("Player").GetComponent<playerHealth>();
        health.maxHealth++;
        health.health = health.maxHealth;
        health.UpdateHearts();
        transform.GetChild(0).gameObject.SetActive(false);
        var UIheart = GameObject.FindGameObjectWithTag("Player").GetComponent<playerHealth>().hearts[health.maxHealth - 1];
        rect = UIheart.GetComponent<RectTransform>();
        approachPos = rect.anchoredPosition;
        rect.position = transform.GetChild(0).position + new Vector3(-0.5f, 0.5f, 0);
        used = true;
        StartCoroutine(moveHeart());
    }

    public override void Start()
    {
        base.Start();
        save = GameObject.FindGameObjectWithTag("save").GetComponent<dontDestroySave>();
        used = save.maxHealthIncreases[healthUpIndex];
        transform.GetChild(0).gameObject.SetActive(!used);
    }

    IEnumerator moveHeart()
    {
        startSFX.Play();
        Timer = Time.time;
        while(rect.anchoredPosition != approachPos)
        {
            rect.anchoredPosition = Vector3.MoveTowards(rect.anchoredPosition, approachPos, (Time.time - Timer) * 100 * Time.deltaTime);
            yield return null;
        }
        Sprite spr = rect.gameObject.GetComponent<Image>().sprite;
        rect.gameObject.GetComponent<Image>().sprite = hpDing;
        startSFX.Stop();
        finishSFX.Play();
        yield return new WaitForSeconds(0.1666f);
        rect.gameObject.GetComponent<Image>().sprite = spr;
    }
}
