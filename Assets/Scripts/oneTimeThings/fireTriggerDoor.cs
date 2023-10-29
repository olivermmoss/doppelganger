using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireTriggerDoor : MonoBehaviour
{
    public GameObject fire;
    public GameObject door;
    public float finalDoorY = 27.25f;
    public fireTriggerDoor deactivateThis;
    public bool doIt = true;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("fireball"))
        {
            fire.SetActive(true);
            if (doIt)
            {
                gameObject.GetComponent<ParticleSystem>().Play();
                LeanTween.moveLocalY(door, finalDoorY, 4f).setOnComplete(StopParticles).setEaseInOutSine();
            }
            if (deactivateThis != null)
                deactivateThis.doIt = false;
        }
    }

    void StopParticles()
    {
        gameObject.GetComponent<ParticleSystem>().Stop();
    }
}
