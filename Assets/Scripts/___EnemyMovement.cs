using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform patrolLeft;
    [SerializeField] private Transform patrolRight;
    [SerializeField] private Transform chaseLeft;
    [SerializeField] private Transform chaseRight;


    [SerializeField] float agroRange = 5;
    [SerializeField] protected float chaseRange = 6;
    [SerializeField] private float speedMovementPatrol = 2f;
    [SerializeField] protected float speedMovementChase = 4f;

    protected Rigidbody2D rb;
    protected Animator anim;
    private Transform currentPoint;

    protected bool doNothing = false;
    protected bool isChasing = false;
    protected bool stopChasing = true;
    private bool isFacingRight = true;

    protected Transform player;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentPoint = patrolRight;
        player = Hero.Instance.transform;
    }

    //void Update()
    //{
        
    //    Flip();
        
    //    if (doNothing) return;

    //    CheckChasing();

    //    if (isChasing && !stopChasing)
    //    {
    //        DoChasing();
    //        return;
    //    }
    //    if (!isChasing)
    //    {
    //        DoPatrol();
    //    }

    //}
    protected void CheckChasing()
    {
        if (Vector2.Distance(transform.position, chaseRight.position) < 0.5f || Vector2.Distance(transform.position, chaseLeft.position) < 0.5f)
        {
            stopChasing = true;
            rb.velocity = Vector2.zero;
            //StartCoroutine(NothingLoop());
        }

        float distToPlayer = Vector2.Distance(transform.position, player.position);

        if (transform.position.x < chaseRight.position.x && transform.position.x > chaseLeft.position.x && distToPlayer > chaseRange)
        {
            stopChasing = false;
        }

        if (distToPlayer < agroRange)
        {
            isChasing = true;
        }
        else if (distToPlayer > chaseRange)
        {
            isChasing = false;
        }
    }

    protected void DoPatrol()
    {
        if (currentPoint == patrolRight)
        {
            rb.velocity = new Vector2(speedMovementPatrol, 0);

            if (transform.position.x > currentPoint.position.x)
            {
                rb.velocity = Vector2.zero;
                currentPoint = patrolLeft;
            }
        }
        else
        {
            rb.velocity = new Vector2(-speedMovementPatrol, 0);
            if (transform.position.x < currentPoint.position.x)
            {
                rb.velocity = Vector2.zero;
                currentPoint = patrolRight;
            }
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f)
        {
            StartCoroutine(NothingLoop());
        }
    }


    protected void Flip()
    {
        float horizontal = rb.velocity.x;

        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            Vector2 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    protected IEnumerator NothingLoop()
    {
        doNothing = true;
        yield return new WaitForSeconds(1f);
        doNothing = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(patrolLeft.position, 0.3f);
        Gizmos.DrawWireSphere(patrolRight.position, 0.3f);
        //Gizmos.DrawLine(patrolLeft.position, patrolRight.position);

        Gizmos.DrawWireCube(chaseLeft.position, new Vector2(0.5f, 0.5f));
        Gizmos.DrawWireCube(chaseRight.position, new Vector2(0.5f, 0.5f));
        //Gizmos.DrawLine(chaseLeft.position, chaseRight.position);

        Gizmos.DrawLine(chaseLeft.position, transform.position);
        Gizmos.DrawLine(chaseRight.position, transform.position);
        Gizmos.DrawLine(patrolLeft.position, transform.position);
        Gizmos.DrawLine(patrolRight.position, transform.position);

        Gizmos.DrawWireSphere(transform.position, 5f);
        Gizmos.DrawWireSphere(transform.position, 6f);
        Gizmos.DrawWireSphere(transform.position, 1.5f);
    }

}
