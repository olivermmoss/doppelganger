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
            iTween.MoveBy(door, iTween.Hash(
                "y", -5.25f,
                "time", 5f,
                "easetype", "easeInOutSine",
                "onComplete", "StopParticles",
                "oncompletetarget", gameObject
            ));
        }
    }

    void StopParticles()
    {
        gameObject.GetComponent<ParticleSystem>().Stop();
    }
}
