using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breakableWall : MonoBehaviour
{
    private bool broken = false;
    private SpriteRenderer sr;
    public Sprite sprite;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("playerAttack"))
        {
            if (!broken)
            {
                broken = true;
                sr.sprite = sprite;
                transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
                transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
            }
            else
            {
                sr.enabled = false;
                GetComponent<BoxCollider2D>().enabled = false;
                transform.GetChild(0).gameObject.SetActive(false);
                GetComponent<ParticleSystem>().Play();
            }
        }
    }

    private void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
    }
}
