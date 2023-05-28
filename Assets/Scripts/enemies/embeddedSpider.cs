using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class embeddedSpider : MonoBehaviour
{
    //though it's kind of an enemy, it's also kind of not so it doesn't get base class
    public GameObject tinySpider;
    public float cooldown;
    public Transform shootPoint;
    public Animator anim;
    public float rotateBy;
    private bool shooting;
    public float attackCooldown;
    public float attackDuration;
    public bool done = false;
    public int aggroDistance = 20;
    public GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(WholeAttack());
    }

    private IEnumerator WholeAttack()
    {
        bool shootPlease = true;
        if (Vector2.Distance(player.transform.position, gameObject.transform.position) > aggroDistance)
        {
            shootPlease = false;
        }
        yield return new WaitForSeconds(attackCooldown);
        anim.SetBool("retracted", true);
        yield return new WaitForSeconds(1);
        if(shootPlease)
        {
            StartCoroutine(shootSpider());
            shooting = true;
        }
        yield return new WaitForSeconds(attackDuration);
        shooting = false;
        anim.SetBool("retracted", false);
        if (!done)
            StartCoroutine(WholeAttack());
    }

    private IEnumerator shootSpider()
    {
        Instantiate(tinySpider, shootPoint.position, shootPoint.rotation);
        yield return new WaitForSeconds(cooldown);
        shootPoint.transform.Rotate(new Vector3(0, 0, rotateBy));
        if(shooting)
            StartCoroutine(shootSpider());
    }
}
