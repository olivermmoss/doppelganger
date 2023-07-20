using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
 
public class parallaxWcam : MonoBehaviour
{
    private float length, startpos, length2;
    public GameObject cam;
    private CameraSystem camSys;
    public float parallaxFactor;
    public float PixelsPerUnit;
    public bool notMoving = true;
    public float speed;
    public PixelPerfectCamera ppc;
    public bool stayInFrame = true;
    public float centerX = 0;

    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        length2 = length / 4;
        ppc = cam.GetComponent<CameraSystem>().ppc;
        camSys = cam.GetComponent<CameraSystem>();
    }
 
    void OnWillRenderObject()
    {
        float temp     = (camSys.camPos.x - centerX) * (1 - parallaxFactor);
        float distance = (camSys.camPos.x - centerX) * parallaxFactor;
        Vector3 newPosition;

        if (notMoving)
            newPosition = new Vector3(startpos + distance, transform.position.y, transform.position.z);
        else
            newPosition = new Vector3(startpos + distance + (Time.time * speed) % length2, transform.position.y, transform.position.z);

        if(parallaxFactor == 1)
        {
            newPosition = new Vector3(camSys.camPos.x, transform.position.y, transform.position.z);
        }

        transform.position = MatchCamRounding(newPosition, PixelsPerUnit);

        if (stayInFrame)
        {
            if (temp > startpos + (length / 2)) startpos += length;
            else if (temp < startpos - (length / 2)) startpos -= length;
        }
   }
 
    private Vector3 MatchCamRounding(Vector3 locationVector, float pixelsPerUnit)
    {
        if (parallaxFactor == 0)
            return locationVector;
        Vector3 vectorInPixels = camSys.camPos.x * parallaxFactor < ppc.RoundToPixel(camSys.camPos).x * parallaxFactor ? new Vector3(Mathf.CeilToInt(locationVector.x * pixelsPerUnit), 
            Mathf.CeilToInt(locationVector.y * pixelsPerUnit), Mathf.CeilToInt(locationVector.z * pixelsPerUnit)) :
            new Vector3(Mathf.FloorToInt(locationVector.x * pixelsPerUnit),
            Mathf.FloorToInt(locationVector.y * pixelsPerUnit), Mathf.FloorToInt(locationVector.z * pixelsPerUnit));
        return vectorInPixels / pixelsPerUnit;
    }
      
}