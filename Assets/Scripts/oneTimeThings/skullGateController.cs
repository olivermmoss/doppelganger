using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skullGateController : MonoBehaviour
{
    public SpriteRenderer[] srs;
    public Sprite[] openSprites;
    public int skullNum;
    public Animator[] enemies;
    public dontDestroySave save;

    // Start is called before the first frame update
    void Start()
    {
        save = FindObjectOfType<dontDestroySave>();
        if(save.skullGate[skullNum])
        {
            Open();
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Animator enemy in enemies)
        {
            if (enemy != null && !enemy.GetBool("isDead"))
            { return; }
        }
        save.skullGate[skullNum] = true;
        Open();
    }

    void Open()
    {
        srs[0].sprite = openSprites[0];
        srs[1].sprite = openSprites[1];
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        this.enabled = false;
    }
}
