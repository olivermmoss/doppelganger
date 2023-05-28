using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioManagerCheck : MonoBehaviour
{
    public GameObject audioMan;

    void Awake()
    {
        if(FindObjectOfType<dontDestroyMusic>())
            return;
        else
            Instantiate(audioMan, transform.position, transform.rotation);
    }
}