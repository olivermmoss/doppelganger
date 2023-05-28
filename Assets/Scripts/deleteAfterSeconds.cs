using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deleteAfterSeconds : MonoBehaviour
{
    public float _seconds;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, _seconds);
    }
}
