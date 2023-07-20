using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class canvasController : MonoBehaviour
{
    private PixelPerfectCamera cam;
    private CanvasScaler scale;

    private void Start()
    {
        cam = gameObject.GetComponent<Canvas>().worldCamera.GetComponent<PixelPerfectCamera>();
        scale = GetComponent<CanvasScaler>();
    }

    void Update()
    {
        scale.scaleFactor = cam.pixelRatio / 4f;
    }
}
