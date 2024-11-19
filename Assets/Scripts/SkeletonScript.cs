using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkeletonScript : EnemyMovement
{

    [SerializeField] protected float atackRange = 1.5f;

    [SerializeField] int HP = 3;

    void Update()
    {

        Flip();
        States();

        if (doNothing) return;

        CheckChasing();

        if (CheckAtackRange())
        {
            DoAtack();
        }
        else if (isChasing && !stopChasing)
        {
            DoChasing();
        }
        else if (!isChasing)
        {
            DoPatrol();
        }

    }

    private void States()
    {
        anim.SetFloat("|velocity.x|", Mathf.Abs(rb.velocity.x));
        anim.SetInteger("health", HP);
    }

    private bool CheckAtackRange()
    {
        float distToPlayer = Vector2.Distance(transform.position, player.position);

        return distToPlayer < atackRange;

    }

    private void DoAtack()
    {
        doNothing = true;
        anim.SetTrigger("attack");
        //в анимации запускается функция EndDoNothing();
    }

    public void EndDoNothing()
    {
        doNothing = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject == Hero.Instance.gameObject) && (!Hero.Instance.getHit) && (!Hero.Instance.isInvulnerability))
            Hero.Instance.GetDamage(1, rb.transform);
    }

    private void DoChasing()
    {

        float distToPlayer = Vector2.Distance(transform.position, player.position);

        if (distToPlayer < chaseRange)
        {
            if (transform.position.x < player.position.x)
            {
                rb.velocity = new Vector2(speedMovementChase, 0);
            }
            else if (transform.position.x > player.position.x)
            {
                rb.velocity = new Vector2(-speedMovementChase, 0);
            }
        }
        else
        {
            StartCoroutine(NothingLoop());
            isChasing = false;
            rb.velocity = Vector2.zero;
        }
    }


}
