using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class doorController : baseInteractable
{
    public Vector2 doorExit;
    public CameraSystem cam;
    public Vector2 maxCam;
    public Vector2 minCam;

    override public void Activate()
    {
        player.transform.position = doorExit;
        cam.xMin = minCam.x;
        cam.xMax = maxCam.x;
        cam.yMin = minCam.y;
        cam.yMax = maxCam.y;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(new Vector2(minCam.x - 12, maxCam.y + 7), new Vector2(maxCam.x + 12, maxCam.y + 7));
        Gizmos.DrawLine(new Vector2(maxCam.x + 12, maxCam.y + 7), new Vector2(maxCam.x + 12, minCam.y - 7));
        Gizmos.DrawLine(new Vector2(maxCam.x + 12, minCam.y - 7), new Vector2(minCam.x - 12, minCam.y - 7));
        Gizmos.DrawLine(new Vector2(minCam.x - 12, minCam.y - 7), new Vector2(minCam.x - 12, maxCam.y + 7));
    }
}
