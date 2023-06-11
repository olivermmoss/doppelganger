using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class changeSceneDoor : baseInteractable
{
    public string doorExit;
    public Vector2 NewScenePos;
    public bool facingRight;
    public Vector2 camMins;
    public Vector2 camMaxes;

    public override void Activate()
    {
        print("change scene");
        print(gameObject.name);

        dontDestroySave save = GameObject.FindGameObjectWithTag("save").GetComponent<dontDestroySave>();

        var wipe = GameObject.FindGameObjectWithTag("screenWipe").GetComponent<screenWipeController>();

        //var player = GameObject.FindGameObjectWithTag("Player").transform.position;
        var vertical = true;
        var posDiff = true;
        wipe.WipeOn(vertical, posDiff);
        save.verticalWipe = vertical;
        save.wipePosDiff = posDiff;

        save.ChangeScene(doorExit, NewScenePos, facingRight, camMins, camMaxes);
    }
}

