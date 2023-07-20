using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingPlatform : MonoBehaviour
{
    public Vector2[] points;
    private int curPoint = 0;
    public float speed;
    private bool moveHands;
    public Transform[] hands;

    // Start is called before the first frame update
    void Start()
    {
        SuperMoveToNext();
        moveHands = hands.Length != 0;
    }

    void SuperMoveToNext()
    {
        curPoint = (curPoint + 1) % points.Length;
        MoveToNextPoint();
    }

    public void MoveToNextPoint()
    {
        float dist = Vector2.Distance(gameObject.transform.position, points[curPoint]);

        LeanTween.moveLocal(gameObject, points[curPoint], dist * speed).setOnComplete(SuperMoveToNext);

        if (moveHands)
            MoveHands(true);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        for(int i = 1; i < points.Length; i++)
        {
            Gizmos.DrawLine(points[i - 1], points[i]);
        }
        Gizmos.DrawLine(points[0], points[points.Length - 1]);
    }

    private void Update()
    {
        if(moveHands)
            MoveHands(false);
    }

    private void MoveHands(bool reset)
    {
        if(reset)
        {
            for (int i = 0; i < 2; i++)
            {
                Transform hand = hands[i];
                Vector2 moveDir = (points[curPoint] - (Vector2)transform.position).normalized;

                //LeanTween.pause(hand.gameObject);
                Vector2 targetPos = (Vector2)transform.position + moveDir * (i * (0.75f) + 1f) + new Vector2(-moveDir.y, moveDir.x) * 0.125f * (2 * i - 1);
                LeanTween.move(hand.gameObject, targetPos, 0.1f);
            }

            return;
        }

        for(int i = 0; i < 2; i++)
        {
            Transform hand = hands[i];

            if(Vector2.Distance(hand.position, transform.position) < 0.25f)
            {
                Vector2 moveDir = (points[curPoint] - (Vector2)transform.position).normalized;
                Vector2 targetPos = (Vector2)transform.position + moveDir * 1.75f + new Vector2(-moveDir.y, moveDir.x) * 0.125f * (2 * i - 1);
                LeanTween.move(hand.gameObject, targetPos, 0.1f);
            }
        }
    }
}
