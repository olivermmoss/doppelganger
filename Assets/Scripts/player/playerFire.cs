using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class playerFire : MonoBehaviour
{
    //0 to 1
    public float charge;
    public Image fireIcon;
    public Sprite[] iconSprites;
    public float chargeRate;
    private float whiteTimer;
    private bool blinked;
    public float draining = 0;
    public GameObject firePrefab;
    public AudioSource readySource;
    public bool canFire = true;

    private InputActionAsset actions;

    void Start()
    {
        fireIcon.sprite = iconSprites[1];
        fireIcon.fillAmount = Mathf.Lerp(0.125f, 0.875f, charge) - 0.02f;

        actions = gameObject.GetComponent<PlayerMove>().actions;
        actions.FindActionMap("gameplay").FindAction("fireball").performed += ShootFire;
    }

    private void ShootFire(InputAction.CallbackContext context = new InputAction.CallbackContext())
    {
        if(charge >= 1 && canFire)
        {
            Instantiate(firePrefab, gameObject.transform.position, Quaternion.Euler(new Vector3(0, 0, gameObject.transform.localScale.x > 0 ? 270 : 90)));
            charge = 0;
            blinked = false;
            draining = 1;
        }
    }

    private void OnDisable()
    {
        // for the "jump" action, we add a callback method for when it is performed
        actions.FindActionMap("gameplay").FindAction("fireball").performed -= ShootFire;
    }

    // Update is called once per frame
    void Update()
    {
        if (charge < 1)
        {
            
            //converts charge to rate that looks nice
            if(draining > 0)
            {
                fireIcon.fillAmount = Mathf.Lerp(0.125f, 0.875f, draining);
                draining -= chargeRate * Time.deltaTime * 50;
            }
            else
            {
                fireIcon.fillAmount = Mathf.Lerp(0.125f, 0.875f, charge) - 0.02f;
                charge += chargeRate * Time.deltaTime;
            }
            
        }
        if (charge >= 1)
        {
            if(!blinked)
            {
                //sets to white
                if (Time.timeSinceLevelLoad > 0.1f)
                {
                    readySource.Play();
                    fireIcon.sprite = iconSprites[0];
                }
                blinked = true;
                whiteTimer = Time.time + 0.2f;
            }
            else if(whiteTimer < Time.time)
            {
                //sets to not white
                fireIcon.sprite = iconSprites[1];
            }
        }
    }
}
