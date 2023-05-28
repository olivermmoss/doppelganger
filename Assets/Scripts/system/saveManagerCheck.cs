using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class saveManagerCheck : MonoBehaviour
{
    public GameObject saveMan;

    void Awake()
    {
        if(FindObjectOfType<dontDestroySave>())
            return;
        else
            Instantiate(saveMan, transform.position, transform.rotation);
    }
}