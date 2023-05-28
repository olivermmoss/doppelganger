using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addForce : MonoBehaviour
{
    public bool shoot = true;
    public GameObject projectile;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(5, 5);
        if (shoot)
            InvokeRepeating("Instan", 0f, 1f);
    }

    void Instan()
    {
        for(int i = 0; i < 4; i++)
        {
            Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, i * 90 + 45));
        }
    }
}
