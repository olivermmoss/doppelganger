using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class logPeopleController : MonoBehaviour
{
    public Sprite[] sprites;
    public TextAsset[] texts;
    public int index = -1;
    public logPeopleController cont;
    public int updateTimer;
    public dialogueTrigger trig;

    private void Randomize()
    {
        dontDestroySave save = FindObjectOfType<dontDestroySave>();
        if(save.cutscenesWatched[0])
        {
            if(cont.index != -1)
            {
                index = Random.Range(0, sprites.Length - 1);
                if (index >= cont.index)
                {
                    index++;
                }
            }
            else
            {
                index = Random.Range(0, sprites.Length);
            }
        }
        else
        { index = 0; }

        GetComponent<SpriteRenderer>().sprite = sprites[index];
        if(index != 0 && index != 1)
        {
            trig.enabled = true;
            GetComponent<BoxCollider2D>().enabled = true;
        }
    }
    private void SetText()
    {
        dontDestroySave save = FindObjectOfType<dontDestroySave>();

        switch(index)
        {
            case 2:
                if (cont.index == 0 || cont.index == 1)
                    trig.inkJson = texts[2];
                else
                    trig.inkJson = texts[3];
                break;
            case 3:
                trig.inkJson = texts[1];
                break;
            case 4:
                trig.inkJson = texts[0];
                break;
            default:
                trig.inkJson = null;
                break;
        }
    }

    private void Update()
    {
        if(updateTimer == 0)
        {
            Randomize();
        }
        if (updateTimer <= -10)
        {
            SetText();
            this.enabled = false;
        }
        else
        {
            updateTimer--;
        }
    }
}
