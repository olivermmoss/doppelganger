using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class glowWormController : MonoBehaviour
{
    public Sprite[] sprs;
    public bool scrunched = false;
    private SpriteRenderer sr;
    public float[] yvals;
    public bool movingDown;
    private float xval;

    // Start is called before the first frame update
    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        InvokeRepeating("MoveALil", 0, 0.5f);
        xval = transform.position.x;
    }

    void MoveALil()
    {
        sr.sprite = sprs[scrunched ? 1 : 0];
        if(scrunched)
            gameObject.transform.position = new Vector2(xval, gameObject.transform.position.y + (movingDown ? -0.125f : 0.125f));
        scrunched = !scrunched;
        if((movingDown && transform.position.y <= yvals[0]) || (!movingDown && transform.position.y >= yvals[1]))
        {
            movingDown = !movingDown;
            gameObject.transform.localEulerAngles = new Vector3(0, 0, (transform.localEulerAngles.z + 180) % 360);
        }
    }
}
