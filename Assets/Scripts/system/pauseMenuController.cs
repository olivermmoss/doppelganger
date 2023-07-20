using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class pauseMenuController : MonoBehaviour
{
    [SerializeField]
    private Image panel;
    public GameObject itemMenu;
    public Sprite emptyItem;
    public GameObject optionsMenu;
    [SerializeField]
    private Sprite[] items = new Sprite[6];
    public bool paused = false;
    public float transTime;

    private InputActionAsset actions;

    private void Start()
    {
        panel = transform.parent.GetComponentInParent<Image>();
        for (int i = 0; i < 6; i++)
        {
            items[i] = itemMenu.transform.GetChild(i).GetComponent<Image>().sprite;
        }

        actions = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>().actions;
        actions.FindActionMap("gameplay").FindAction("pause").performed += PauseUnpause;
    }
    void PauseUnpause(InputAction.CallbackContext context = new InputAction.CallbackContext())
    {
        if (paused)
            MenuUp();
        else
            MenuDown();
    }

    private void OnDisable()
    {
        // for the "jump" action, we add a callback method for when it is performed
        actions.FindActionMap("gameplay").FindAction("pause").performed -= PauseUnpause;
    }

    void MenuDown()
    {
        Time.timeScale = 0;
        paused = true;
        LeanTween.moveLocalX(itemMenu, 272f, transTime).setEaseOutSine().setIgnoreTimeScale(true);
        LeanTween.moveLocalY(gameObject, 0f, transTime).setEaseOutSine().setIgnoreTimeScale(true);
        /*iTween.MoveTo(itemMenu, iTween.Hash(
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
            ));*/
        var save = FindObjectOfType<dontDestroySave>();
        for(int i = 0; i < 6; i++)
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
        LeanTween.value(gameObject, UpdatePanelColor, panel.color.a, 0.3f, transTime).setIgnoreTimeScale(true);
        //iTween.ValueTo(gameObject, iTween.Hash("from", panel.color.a, "to", 0.3f, "time", transTime, "onupdate", "UpdatePanelColor", "ignoretimescale", true));
    }

    public void MenuUp()
    {
        Time.timeScale = 1;
        paused = false;
        LeanTween.moveLocalX(itemMenu, 640f, transTime).setEaseInSine().setIgnoreTimeScale(true);
        LeanTween.moveLocalY(gameObject, 512f, transTime).setEaseInSine().setIgnoreTimeScale(true);
        LeanTween.moveLocalY(optionsMenu, -512f, transTime).setEaseInSine().setIgnoreTimeScale(true);
        /*iTween.MoveTo(itemMenu, iTween.Hash(
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
        iTween.MoveTo(optionsMenu, iTween.Hash(
                "y", -512f,
                "time", transTime,
                "easetype", "easeInSine",
                "islocal", true,
                "ignoretimescale", true
            ));*/
        LeanTween.value(gameObject, UpdatePanelColor, panel.color.a, 0f, transTime).setIgnoreTimeScale(true);
        //iTween.ValueTo(gameObject, iTween.Hash("from", panel.color.a, "to", 0f, "time", transTime, "onupdate", "UpdatePanelColor", "ignoretimescale", true));
    }

    void UpdatePanelColor(float alpha)
    {
        panel.color = new Vector4(0, 0, 0, alpha);
    }

    public void OptionsUp()
    {
        LeanTween.moveLocalX(itemMenu, 640f, transTime).setEaseInSine().setIgnoreTimeScale(true);
        LeanTween.moveLocalY(gameObject, 512f, transTime).setEaseInSine().setIgnoreTimeScale(true);
        LeanTween.moveLocalY(optionsMenu, 0f, transTime).setEaseOutSine().setIgnoreTimeScale(true);
        /*iTween.MoveTo(itemMenu, iTween.Hash(
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
                "easetype", "easeOutSine",
                "islocal", true,
                "ignoretimescale", true
            ));
        iTween.MoveTo(optionsMenu, iTween.Hash(
                "y", 0f,
                "time", transTime,
                "easetype", "easeOutSine",
                "islocal", true,
                "ignoretimescale", true
            ));*/
    }

    public void OptionsAway()
    {
        LeanTween.moveLocalX(itemMenu, 272f, transTime).setEaseOutSine().setIgnoreTimeScale(true);
        LeanTween.moveLocalY(gameObject, 0f, transTime).setEaseOutSine().setIgnoreTimeScale(true);
        LeanTween.moveLocalY(optionsMenu, -512f, transTime).setEaseOutSine().setIgnoreTimeScale(true);
        /*iTween.MoveTo(itemMenu, iTween.Hash(
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
        iTween.MoveTo(optionsMenu, iTween.Hash(
                "y", -512f,
                "time", transTime,
                "easetype", "easeOutSine",
                "islocal", true,
                "ignoretimescale", true
            ));*/
    }

    public void ToMainMenu()
    {
        //var save = GameObject.FindGameObjectWithTag("save").GetComponent<dontDestroySave>();
        SceneManager.LoadScene("MainMenu");
    }
}
