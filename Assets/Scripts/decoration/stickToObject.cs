using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stickToObject : MonoBehaviour
{
    public GameObject sticky;
    public GameObject stickee;
    public Vector3 offset;
    public bool isMirror = false;
    public float mirrorCenter;

    private SpriteRenderer stickeeSr;
    private Transform squashNstretch;
    private SpriteRenderer sr;

    private void LateUpdate()
    {
        if(isMirror)
            sticky.transform.position = new Vector2(2 * mirrorCenter - (stickee.transform.position.x + offset.x), stickee.transform.position.y + offset.y);
        else
            sticky.transform.position = stickee.transform.position + offset;

        if(isMirror)
        {
            sticky.transform.localScale = new Vector3(-squashNstretch.localScale.x * stickee.transform.localScale.x, squashNstretch.localScale.y);
            sr.sprite = stickeeSr.sprite;
        }
    }

    private void Start()
    {
        squashNstretch = stickee.transform.GetChild(1);
        stickeeSr = squashNstretch.GetChild(0).GetComponent<SpriteRenderer>();
        sr = sticky.GetComponent<SpriteRenderer>();
    }
}
