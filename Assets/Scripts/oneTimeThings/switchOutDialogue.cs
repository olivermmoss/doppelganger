using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchOutDialogue : MonoBehaviour
{
    public string condition;
    public int index;
    public TextAsset newText;
    public GameObject enableThis;
    public GameObject disableThis;
    public Sprite newSprite;

    void Start()
    {
        var save = GameObject.FindGameObjectWithTag("save").GetComponent<dontDestroySave>();
        var trig = gameObject.GetComponent<dialogueTrigger>();

        switch(condition)
        {
            case "bossesKilled":
                if (save.bossesKilled[index])
                    DoSwap(trig);
                break;
            case "cutscenesWatched":
                if (save.cutscenesWatched[index])
                    DoSwap(trig);
                break;
            case "itemsGotten":
                if (save.itemsGotten[index])
                    DoSwap(trig);
                break;
            default:
                break;
        }
    }

    void DoSwap(dialogueTrigger trig)
    {
        trig.inkJson = newText;
        if(enableThis != null)
        {
            enableThis.SetActive(true);
        }
        if (disableThis != null)
        {
            disableThis.SetActive(false);
        }
        if(newSprite != null)
        {
            GetComponent<SpriteRenderer>().sprite = newSprite;
        }
    }
}
