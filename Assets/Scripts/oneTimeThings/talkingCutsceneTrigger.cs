using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class talkingCutsceneTrigger : MonoBehaviour
{
    public dontDestroyMusic theAM;
    public GameObject player;
    public AudioClip clip;
    public TextAsset inkJson;
    public SpriteRenderer[] spritesToChange;
    public Sprite[] sprites;
    public sbyte cutsceneNum;
    public dontDestroySave theSM;
    public AudioClip[] clips;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.transform.CompareTag("Player"))
        {
            theSM = GameObject.FindGameObjectWithTag("save").GetComponent<dontDestroySave>();
            theAM = FindObjectOfType<dontDestroyMusic>();
            player = collision.gameObject;

            if(clip != null)
                theAM.ChangeBGM(clip);
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            if (cutsceneNum != -1)
                theSM.cutscenesWatched[cutsceneNum] = true;
            
            dialogueManager.GetInstance().EnterDialogueMode(inkJson, null, this);
        }
    }
}
