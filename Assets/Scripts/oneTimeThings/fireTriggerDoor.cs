using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireTriggerDoor : MonoBehaviour
{
    public GameObject fire;
    public GameObject door;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("fireball"))
        {
            fire.SetActive(true);
            gameObject.GetComponent<ParticleSystem>().Play();
            LeanTween.moveLocalY(door, 27.25f, 4f).setOnComplete(StopParticles).setEaseInOutSine();
        }
    }

    void StopParticles()
    {
        gameObject.GetComponent<ParticleSystem>().Stop();
    }
}
