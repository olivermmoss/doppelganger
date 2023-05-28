using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dynamite : MonoBehaviour
{
    public Image panel;
    public Sprite[] sprites;
    private SpriteRenderer rend;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var fireball = collision.gameObject.GetComponent<fireballController>();

        if(fireball != null)
        {
            fireball.Poof();
            StartCoroutine(Explode());
        }
    }

    IEnumerator Explode()
    {
        rend.sprite = sprites[0];
        yield return new WaitForSeconds(1f);
        rend.sprite = sprites[1];
        yield return new WaitForSeconds(0.5f);
        rend.sprite = sprites[2];
        yield return new WaitForSeconds(0.5f);
        rend.sprite = sprites[3];
        yield return new WaitForSeconds(0.5f);
        iTween.ValueTo(gameObject, iTween.Hash("from", 1f, "to", 0f, "time", 1f, "onupdate", "UpdatePanelColor"));
        GetComponent<SpriteRenderer>().enabled = false;
        var boxes = GetComponents<BoxCollider2D>();
        foreach(var box in boxes)
        {
            box.enabled = false;
        }
        Destroy(transform.GetChild(1).gameObject);
        Destroy(transform.GetChild(0).gameObject);
    }

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    void UpdatePanelColor(float alpha)
    {
        panel.color = new Vector4(1, 1, 1, alpha);
    }
}
