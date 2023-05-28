using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class dontDestroyMusic : MonoBehaviour
{
    public AudioSource BGM;

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        if(FindObjectsOfType<dontDestroyMusic>().Length > 1)
        {
            Destroy(gameObject);
        }
    }

    public void ChangeBGM(AudioClip music)
    {
        if(BGM.clip.name == music.name)
            return;

        BGM.Stop();
        BGM.clip = music;
        BGM.Play();
    }
    public void StopBGM()
    {
        BGM.Stop();
    }
    public void ChangeBGMSeamless(AudioClip music)
    {
        if(BGM.clip.name == music.name)
            return;
        
        float clipTime = BGM.time;
        BGM.Stop();
        BGM.clip = music;
        BGM.Play();
        BGM.time = clipTime;
    }
}