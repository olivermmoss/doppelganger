using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAttack : MonoBehaviour
{
    public Animator animator;
    //public Transform attackPoint;
    public Collider2D attackTrigger;
    public Collider2D attackTrigger2;
    //public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 10;
    public float attackRate = 2;
    public float attackDuration = 3;
    private float nextAttackTime = 0;
    private float doneAttackTime = 0;
    private Vector3 pointPos;

    void Update()
    {
        if(Time.time >= nextAttackTime)
        {
            if(Input.GetKeyDown(KeyCode.X))
            {
                Slash();
                nextAttackTime = Time.time + 1/attackRate;
            }
        }
        if(Time.time >= doneAttackTime)
        {
            attackTrigger.enabled = false;
            attackTrigger2.enabled = false;
            attackTrigger.gameObject.transform.localPosition = Vector3.one * 9999f;
        }
    }

    void Slash()
    {
        attackTrigger.gameObject.transform.localPosition = pointPos;

        animator.SetTrigger("slash");

        attackTrigger.enabled = true;
        attackTrigger2.enabled = true;
        doneAttackTime = Time.time + 1/attackDuration;

        attackTrigger.gameObject.GetComponent<AudioSource>().Play();

        /*Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<enemyController>().TakeDamage(attackDamage);
        }*/
    }

    /*private void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
            return;
        
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }*/

    private void Awake()
    {
        pointPos = attackTrigger.gameObject.transform.localPosition;
    }
}
