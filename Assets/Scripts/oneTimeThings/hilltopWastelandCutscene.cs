using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hilltopWastelandCutscene : MonoBehaviour
{
    public GameObject[] getRidOf;
    public dontDestroyMusic theAM;
    public dontDestroySave theSM;
    public AudioClip clip;
    private void Start()
    {
        theAM = FindObjectOfType<dontDestroyMusic>();
        theSM = GameObject.FindGameObjectWithTag("save").GetComponent<dontDestroySave>();

        if(theSM.bossesKilled[0])
        {
            theAM.ChangeBGM(clip);
        }
        if(theSM.cutscenesWatched[0])
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = false;

            foreach(GameObject obj in getRidOf)
            {
                obj.SetActive(false);
            }
        }
    }
}
