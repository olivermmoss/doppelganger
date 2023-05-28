using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class screenWipeController : MonoBehaviour
{
    public bool wipingOff;
    public float timer = -1;
    public Image img;
    public float timeToWipe = 1;

    private void FixedUpdate()
    {
        if(timer > 0)
        {
            if(wipingOff)
                img.fillAmount = (1 - (Time.time - timer) / timeToWipe);
            else
                img.fillAmount = (Time.time - timer) / timeToWipe;
        }
        else
        {
            img.fillAmount = 0;
        }
    }

    public void WipeOn(bool vertical, bool posDiff)
    {
        timer = Time.time;
        wipingOff = false;
        img.fillMethod = vertical ? Image.FillMethod.Vertical : Image.FillMethod.Horizontal;
        img.fillOrigin = vertical ? (!posDiff ? (int)Image.OriginVertical.Bottom : (int)Image.OriginVertical.Top)
            : (posDiff ? (int)Image.OriginHorizontal.Left : (int)Image.OriginHorizontal.Right);
    }
    public void WipeOff(bool vertical, bool posDiff)
    {
        timer = Time.time;
        wipingOff = true;
        img.fillMethod = vertical ? Image.FillMethod.Vertical : Image.FillMethod.Horizontal;
        img.fillOrigin = vertical ? (posDiff ? (int)Image.OriginVertical.Bottom : (int)Image.OriginVertical.Top)
            : (!posDiff ? (int)Image.OriginHorizontal.Left : (int)Image.OriginHorizontal.Right);
    }

    private void Start()
    {
        //timer = 1;
        img = GetComponent<Image>();
        timeToWipe = GameObject.FindGameObjectWithTag("save").GetComponent<dontDestroySave>().wipeTIme;
    }
}
