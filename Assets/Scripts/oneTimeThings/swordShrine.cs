using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class swordShrine : MonoBehaviour
{
    public GameObject eButton;
    public TextMeshProUGUI dialogueText;
    public Sprite newSprite;
    public TextAsset newJson;
    public int itemToGet = 0;
    public string item = "sword";
    public GameObject activateThis;

    public void DoIt(bool yayOrNay)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
        gameObject.GetComponent<dialogueTrigger>().inkJson = newJson;
        GameObject.FindGameObjectWithTag("save").GetComponent<dontDestroySave>().itemsGotten[itemToGet] = true;

        if(yayOrNay && gameObject.GetComponent<AudioSource>() != null)
            gameObject.GetComponent<AudioSource>().Play();

        switch(item)
        {
            case "sword":
                GameObject.FindGameObjectWithTag("Player").GetComponent<playerAttack>().enabled = true;
                break;
            case "fireball":
                print(yayOrNay);

                GameObject.FindGameObjectWithTag("Player").GetComponent<playerFire>().enabled = true;
                activateThis.gameObject.SetActive(true);
                activateThis.transform.GetChild(0).gameObject.SetActive(true);
                break;
            case "doubleJump":
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>().doubleJump = true;
                gameObject.GetComponent<Animator>().enabled = false;
                gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
                break;
            default:
                break;
        }
    }
}
