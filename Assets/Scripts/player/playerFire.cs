using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    void Start()
    {
        fireIcon.sprite = iconSprites[1];
        fireIcon.fillAmount = Mathf.Lerp(0.125f, 0.875f, charge) - 0.02f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A) && charge >= 1)
        {
            Instantiate(firePrefab, gameObject.transform.position, Quaternion.Euler(new Vector3(0, 0, gameObject.transform.localScale.x > 0 ? 270 : 90)));
            charge = 0;
            blinked = false;
            draining = 1;
        }
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
                readySource.Play();
                fireIcon.sprite = iconSprites[0];
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
