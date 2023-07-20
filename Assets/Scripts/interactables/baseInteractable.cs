using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class baseInteractable : MonoBehaviour
{
    public GameObject player;
    public GameObject eButton;
    public bool used = false;
    public bool active = false;
    private bool attachedAction = false;
    //we only mess with used in some programs, otherwise it's always false so it's inconsequential

    private InputActionAsset actions;

    public virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        eButton.SetActive(true);
        
        LeanTween.alpha(eButton, 0f, 0.001f);

        actions = player.GetComponent<PlayerMove>().actions;
        actions.FindActionMap("gameplay").FindAction("interact").performed += SubActivate;
        attachedAction = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !used)
        {
            active = true;
            LeanTween.cancel(eButton);
            LeanTween.alpha(eButton, 1f, 0.25f);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            active = false;
            LeanTween.cancel(eButton);
            LeanTween.alpha(eButton, 0f, 0.25f);
        }
    }


    public virtual void Activate()
    {
        //override this please
    }

    private void SubActivate(InputAction.CallbackContext context = new InputAction.CallbackContext())
    {
        if (active && !player.GetComponent<playerHealth>().dead)
        {
            Activate();
        }
        if(used)
        {
            OnTriggerExit2D(player.GetComponent<Collider2D>());
        }
    }

    private void OnDisable()
    {
        if(attachedAction)
            actions.FindActionMap("gameplay").FindAction("interact").performed -= SubActivate;
    }
}
