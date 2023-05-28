using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damagePlayer : MonoBehaviour
{
    public int attackDamage;
    public bool yes = true;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && yes)
        {
            other.gameObject.GetComponent<playerHealth>().TakeDamage(attackDamage, gameObject);
        }
    }
}
