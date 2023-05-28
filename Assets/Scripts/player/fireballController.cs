using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireballController : basicProjectile
{
    public ParticleSystem goneParticle;
    public ParticleSystem selfParticle;
    public GameObject glow;
    public AudioSource reflectSource;
    public AudioSource poofSource;

    public override void FixedUpdate()
    {
        gameObject.transform.Translate(speed * Vector2.up);
        if (Time.time - deathTime >= timer && speed != 0)
        {
            Poof();
        }
        if (Time.time - deathTime * 3 >= timer && speed == 0)
        {
            Destroy(gameObject);
        }
    }

    public void Poof()
    {
        poofSource.Play();
        selfParticle.Stop();
        goneParticle.Play();
        GetComponent<SpriteRenderer>().enabled = false;
        speed = 0;
        GetComponent<Collider2D>().enabled = false;
        timer = Time.time;
        glow.SetActive(false);
    }

    public override void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("ground"))
        {
            reflector reflect = other.gameObject.GetComponent<reflector>();
            if (reflect != null)
            {
                //print(Angle(Vector2.Reflect(transform.up, reflect.vec2)));
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, Angle(Vector2.Reflect(transform.up, reflect.vec2))));
                reflectSource.pitch = (Angle(reflect.vec2) - 180f)/720f + 1f;
                reflectSource.Play();
                reflect.PushBack();
            }
            else
            {
                Poof();
            }
        }
    }

    public static float Angle(Vector2 vector2)
    {
            return (360 - 10 * Mathf.Round(Mathf.Atan2(vector2.x, vector2.y) * Mathf.Rad2Deg / 10)) % 360;
    }
}
