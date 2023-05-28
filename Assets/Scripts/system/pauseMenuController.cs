using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pauseMenuController : MonoBehaviour
{
    [SerializeField]
    private Image panel;
    public GameObject itemMenu;
    public Sprite emptyItem;
    [SerializeField]
    private Sprite[] items = new Sprite[6];
    public bool paused = false;
    public float transTime;

    private void Start()
    {
        panel = transform.parent.GetComponentInParent<Image>();
        for (int i = 0; i < 6; i++)
        {
            items[i] = itemMenu.transform.GetChild(i).GetComponent<Image>().sprite;
        }
    }
    void Update()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            if (paused)
                MenuUp();
            else
                MenuDown();
        }
    }

    void MenuDown()
    {
        Time.timeScale = 0;
        paused = true;
        iTween.MoveTo(itemMenu, iTween.Hash(
                "x", 272f,
                "y", 0f,
                "time", transTime,
                "easetype", "easeOutSine",
                "islocal", true,
                "ignoretimescale", true
            ));
        iTween.MoveTo(gameObject, iTween.Hash(
                "y", 0f,
                "time", transTime,
                "easetype", "easeOutSine",
                "islocal", true,
                "ignoretimescale", true
            ));
        var save = FindObjectOfType<dontDestroySave>();
        for(int i = 0; i < 3; i++)
        {
            if(!save.itemsGotten[i])
            {
                itemMenu.transform.GetChild(i).GetComponent<Image>().sprite = emptyItem;
            }
            else
            {
                itemMenu.transform.GetChild(i).GetComponent<Image>().sprite = items[i];
            }
        }
        ///blarg
        iTween.ValueTo(gameObject, iTween.Hash("from", panel.color.a, "to", 0.3f, "time", transTime, "onupdate", "UpdatePanelColor", "ignoretimescale", true));
    }

    public void MenuUp()
    {
        Time.timeScale = 1;
        paused = false;
        iTween.MoveTo(itemMenu, iTween.Hash(
                "x", 640f,
                "y", 0f,
                "time", transTime,
                "easetype", "easeInSine",
                "islocal", true,
                "ignoretimescale", true
            ));
        iTween.MoveTo(gameObject, iTween.Hash(
                "y", 512f,
                "time", transTime,
                "easetype", "easeInSine",
                "islocal", true,
                "ignoretimescale", true
            ));
        iTween.ValueTo(gameObject, iTween.Hash("from", panel.color.a, "to", 0f, "time", transTime, "onupdate", "UpdatePanelColor", "ignoretimescale", true));
    }

    void UpdatePanelColor(float alpha)
    {
        panel.color = new Vector4(0, 0, 0, alpha);
    }
}
