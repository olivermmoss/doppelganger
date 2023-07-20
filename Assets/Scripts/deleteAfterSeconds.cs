using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deleteAfterSeconds : MonoBehaviour
{
    public float _seconds;
    public bool fade = false;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, _seconds);
        if(fade)
        {
            LeanTween.alpha(gameObject, 0, _seconds);
        }
    }
}
