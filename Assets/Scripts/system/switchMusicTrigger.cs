using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchMusicTrigger : MonoBehaviour
{
    public AudioClip newTrack;

    private dontDestroyMusic theAM;

    void Start()
    {
        theAM = FindObjectOfType<dontDestroyMusic>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Triggered()
    {
        if(newTrack != null)
            theAM.ChangeBGM(newTrack);
    }
}