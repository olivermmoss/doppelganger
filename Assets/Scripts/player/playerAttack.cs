using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public bool canAttack = true;

    void Update()
    {
        if(Time.time >= doneAttackTime)
        {
            attackTrigger.enabled = false;
            attackTrigger2.enabled = false;
            attackTrigger.gameObject.transform.localPosition = Vector3.one * 9999f;
        }
    }

    private void Slash(InputAction.CallbackContext context = new InputAction.CallbackContext())
    {
        if (Time.time < nextAttackTime || !canAttack)
            return;

        attackTrigger.gameObject.transform.localPosition = pointPos;

        animator.SetTrigger("slash");

        attackTrigger.enabled = true;
        attackTrigger2.enabled = true;
        doneAttackTime = Time.time + 1/attackDuration;

        attackTrigger.gameObject.GetComponent<AudioSource>().Play();

        nextAttackTime = Time.time + 1/attackRate;
    }

    private void Awake()
    {
        pointPos = attackTrigger.gameObject.transform.localPosition;
    }

    void Start()
    {
        gameObject.GetComponent<PlayerMove>().actions.FindActionMap("gameplay").FindAction("attack").performed += Slash;
    }

    private void OnDisable()
    {
        // for the "jump" action, we add a callback method for when it is performed
        gameObject.GetComponent<PlayerMove>().actions.FindActionMap("gameplay").FindAction("attack").performed -= Slash;
    }
}
