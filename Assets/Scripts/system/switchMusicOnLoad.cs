using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchMusicOnLoad : MonoBehaviour
{
    public AudioClip newTrack;

    private dontDestroyMusic theAM;

    void Start()
    {
        theAM = FindObjectOfType<dontDestroyMusic>();

        if(newTrack != null)
            theAM.ChangeBGM(newTrack);
    }
}