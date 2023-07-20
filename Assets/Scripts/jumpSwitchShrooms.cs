using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpSwitchShrooms : MonoBehaviour
{
    public Sprite[] sprites;
    public bool inWall;
    private SpriteRenderer sr;
    private BoxCollider2D bc;
    private Coroutine anim;
    private bool isMoving;
    private movingPlatform movPlat;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>().jumpDelegate += SwapInOutWall;
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();

        sr.sprite = sprites[inWall ? 2 : 0];
        bc.enabled = !inWall;

        movPlat = GetComponent<movingPlatform>();
        isMoving = movPlat != null;

        if (inWall && isMoving)
        {
            LeanTween.pause(gameObject);
        }
    }

    public void SwapInOutWall()
    {
        inWall = !inWall;
        bc.enabled = !inWall;

        if (anim != null)
            StopCoroutine(anim);

        anim = StartCoroutine(swapAnim());

        if(isMoving)
        {
            if(inWall)
            {
                LeanTween.pause(gameObject);
                foreach(Transform hand in movPlat.hands)
                {
                    hand.gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
                }
            }
            else
            {
                movPlat.MoveToNextPoint();
                foreach (Transform hand in movPlat.hands)
                {
                    hand.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                }
            }
        }
    }

    private IEnumerator swapAnim()
    {
        sr.sprite = sprites[1];
        yield return new WaitForSeconds(0.083333f);
        sr.sprite = sprites[inWall ? 2 : 0];
    }
}
