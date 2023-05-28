using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dialogueTrigger : baseInteractable
{
    public TextAsset inkJson;
    public SpriteRenderer[] spritesToChange;
    public Sprite[] sprites;
    public AudioClip[] clips;

    public override void Activate()
    {
        dialogueManager.GetInstance().EnterDialogueMode(inkJson, this);
    }
}
