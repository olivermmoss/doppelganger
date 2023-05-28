using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchOutDialogue : MonoBehaviour
{
    public string condition;
    public int index;
    public TextAsset newText;

    void Start()
    {
        var save = GameObject.FindGameObjectWithTag("save").GetComponent<dontDestroySave>();
        var trig = gameObject.GetComponent<dialogueTrigger>();

        switch(condition)
        {
            case "bossesKilled":
                if (save.bossesKilled[index])
                    trig.inkJson = newText;
                break;
            case "cutscenesWatched":
                if (save.cutscenesWatched[index])
                    trig.inkJson = newText;
                break;
            case "itemsGotten":
                if (save.itemsGotten[index])
                    trig.inkJson = newText;
                break;
            default:
                break;
        }
    }
}
