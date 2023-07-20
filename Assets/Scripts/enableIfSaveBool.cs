using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enableIfSaveBool : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var save = GameObject.FindGameObjectWithTag("save").GetComponent<dontDestroySave>();
        if(!save.bossesKilled[1])
        {
            gameObject.SetActive(false);

        }
    }

}
