using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class impostorShroom : MonoBehaviour
{
    public Sprite[] sprites;
    private SpriteRenderer sr;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<playerHealth>().TakeDamage(999, gameObject);
            collision.transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().enabled = false;

            StartCoroutine(MovingPicture());
        }
    }

    private void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    IEnumerator MovingPicture()
    {
        sr.sprite = sprites[0];
        yield return new WaitForSeconds(0.75f);
        sr.sprite = sprites[1];
        yield return new WaitForSeconds(0.083333f);
        sr.sprite = sprites[2];
    }
}
