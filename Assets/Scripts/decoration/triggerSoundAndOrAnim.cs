using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerSoundAndOrAnim : MonoBehaviour
{
    public Animator anim;
    public AudioSource source;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (anim != null)
                anim.SetTrigger("doit");
            if (source != null)
                source.Play();
        }
    }
}
