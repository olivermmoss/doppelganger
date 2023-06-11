using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomSprite : MonoBehaviour
{
    public Sprite[] sprites;
    private int index;
    public bool hanging;

    private void Start()
    {
        index = Random.Range(0, sprites.Length);
        GetComponent<SpriteRenderer>().sprite = sprites[index];
        if((index == 3 || index == 5) && hanging)
        {
            GetComponent<ParticleSystem>().Play();
        }
    }
}
