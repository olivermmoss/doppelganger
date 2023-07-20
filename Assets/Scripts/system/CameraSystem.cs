using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class CameraSystem : MonoBehaviour
{

    private GameObject player;
    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;
    public PixelPerfectCamera ppc;
    public float speed = 200f;
    public Vector3 camPos;

    // Start is called before the first frame update
    void Start()
    {
        if(GameObject.FindGameObjectWithTag ("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag ("Player");
            float x = Mathf.Clamp(player.transform.position.x, xMin, xMax);
            float y = Mathf.Clamp(player.transform.position.y, yMin, yMax);
            gameObject.transform.position = new Vector3(x, y, gameObject.transform.position.z);
        }

        transform.position = ppc.RoundToPixel(transform.position);
        camPos = transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(player != null)
        {
            Vector3 targetPos = new Vector3(Mathf.Clamp(player.transform.position.x, xMin, xMax), Mathf.Clamp(player.transform.position.y, yMin, yMax), transform.position.z);
            camPos = Vector3.MoveTowards(camPos, targetPos, speed * Time.deltaTime);
            transform.position = ppc.RoundToPixel(camPos);
            //transform.position = ppc.RoundToPixel(transform.position);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawLine(new Vector2(xMin - 12, yMax + 7), new Vector2(xMax + 12, yMax + 7));
        Gizmos.DrawLine(new Vector2(xMax + 12, yMax + 7), new Vector2(xMax + 12, yMin - 7));
        Gizmos.DrawLine(new Vector2(xMax + 12, yMin - 7), new Vector2(xMin - 12, yMin - 7));
        Gizmos.DrawLine(new Vector2(xMin - 12, yMin - 7), new Vector2(xMin - 12, yMax + 7));
    }

}
