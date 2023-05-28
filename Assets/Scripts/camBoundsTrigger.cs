using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camBoundsTrigger : MonoBehaviour
{
    public Vector2 maxes;
    public Vector2 mins;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.transform.CompareTag("Player"))
        {
            CameraSystem cam = FindObjectOfType<CameraSystem>();
            cam.xMax = maxes.x;
            cam.yMax = maxes.y;
            cam.xMin = mins.x;
            cam.yMin = mins.y;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        Gizmos.DrawLine(new Vector2(mins.x - 12, maxes.y + 7), new Vector2(maxes.x + 12, maxes.y + 7));
        Gizmos.DrawLine(new Vector2(maxes.x + 12, maxes.y + 7), new Vector2(maxes.x + 12, mins.y - 7));
        Gizmos.DrawLine(new Vector2(maxes.x + 12, mins.y - 7), new Vector2(mins.x - 12, mins.y - 7));
        Gizmos.DrawLine(new Vector2(mins.x - 12, mins.y - 7), new Vector2(mins.x - 12, maxes.y + 7));
    }
}
