using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reflector : MonoBehaviour
{
    public Vector2 vec2;
    public bool slope = true;
    private Transform spriteChild;

    // Start is called before the first frame update
    void Start()
    {
        if(slope)
            vec2 = (transform.up + -transform.right).normalized;
        else
            vec2 = transform.up.normalized;

        spriteChild = gameObject.transform.GetChild(0);
    }

    private void OnDrawGizmos()
    {
        // Draws a blue line from this transform to the target
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)vec2);
    }

    public void PushBack()
    {
        spriteChild.localPosition = slope ? new Vector3(0.25f, -0.25f, 0) : new Vector3(0, -0.25f, 0);
    }

    private void Update()
    {
        spriteChild.localPosition = Vector3.MoveTowards(spriteChild.localPosition, Vector3.zero, Time.deltaTime);
    }
}
