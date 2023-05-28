using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baseInteractable : MonoBehaviour
{
    public GameObject player;
    public GameObject eButton;
    public bool used = false;
    public bool active = false;
    //we only mess with used in some programs, otherwise it's always false so it's inconsequential

    public virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        eButton.SetActive(true);
        iTween.FadeTo(eButton, 0f, 0.001f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !used)
        {
            active = true;
            iTween.Stop(eButton);
            iTween.FadeTo(eButton, 1f, 0.25f);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            active = false;
            iTween.Stop(eButton);
            iTween.FadeTo(eButton, 0f, 0.25f);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && active)
        {
            Activate();
        }
    }

    public virtual void Activate()
    {
        //override this please
    }
}
