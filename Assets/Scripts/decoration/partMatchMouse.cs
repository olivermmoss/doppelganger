using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Linq;

public class partMatchMouse : MonoBehaviour
{
    public float divisor;
    // assign the actions asset to this field in the inspector:
    public InputActionAsset actions;
    public InputAction moveAction;
    private Camera cam;
    public Sprite[] sprites;

    private void Start()
    {
        moveAction = actions.FindActionMap("gameplay").FindAction("mousePos");
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        var save = GameObject.FindGameObjectWithTag("save").GetComponent<dontDestroySave>();
        var sr = GetComponent<SpriteRenderer>();
        print(new String(save.stasisScene.Where(Char.IsLetter).ToArray()));
        switch(new String(save.stasisScene.Where(Char.IsLetter).ToArray()))
        {
            case "Wasteland":
                sr.sprite = sprites[0];
                break;
            case "WastelandCaves":
                sr.sprite = sprites[1];
                break;
            case "DarkCastle":
                sr.sprite = sprites[2];
                break;
            case "BigSpider":
                sr.sprite = sprites[1];
                break;
            case "Mountain":
                sr.sprite = sprites[3];
                break;
            case "Jungle":
                sr.sprite = sprites[4];
                break;
            case "ShroomCaves":
                sr.sprite = sprites[5];
                break;
            default:
                sr.sprite = sprites[2];
                break;
        }
    }
    void Update()
    {
        gameObject.transform.position = cam.ScreenToWorldPoint(moveAction.ReadValue<Vector2>()) / divisor;
    }
}
