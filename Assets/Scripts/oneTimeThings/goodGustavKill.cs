using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class goodGustavKill : MonoBehaviour
{
    public GameObject corpse;
    public GameObject eButton;
    public TextMeshProUGUI dialogueText;
    public GameObject door;
    public Sprite doorSprite;

    public void DoIt()
    {
        corpse.SetActive(true);
        Destroy(gameObject);
        door.GetComponent<SpriteRenderer>().sprite = doorSprite;
        door.GetComponent<doorController>().enabled = true;
        GameObject.FindGameObjectWithTag("save").GetComponent<dontDestroySave>().gustavKilled = true;
    }
}
