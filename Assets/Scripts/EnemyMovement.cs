using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform patrolLeft; //pointA
    [SerializeField] private Transform patrolRight; //PointB
    [SerializeField] private Transform chaseLeft;
    [SerializeField] private Transform chaseRight;

    [SerializeField] float agroRange = 5;
    [SerializeField] float chaseRange = 6;
    [SerializeField] private float speedMovementChase = 4f;
    [SerializeField] private float speedMovementPatrol = 2f;

    private Rigidbody2D rb;
    //private Animator anim;
    private Transform currentPoint;

    private bool doNothing = false;
    private bool isChasing = false;
    private bool stopChasing = true;

    private Transform player;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //anim
        currentPoint = patrolRight;
        player = Hero.Instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (doNothing) return;

        CheckChasing();
        if (isChasing && !stopChasing)
        {
            DoChasing();
            return;
        }
        if (!isChasing)
        {
            DoPatrol();
        }

    }

    private void CheckChasing()
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
        }else if (distToPlayer > chaseRange)
        {
            isChasing = false;
        }
    }

    private void DoPatrol()
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

    private IEnumerator NothingLoop()
    {
        doNothing = true;
        yield return new WaitForSeconds(2f);
        doNothing = false;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(patrolLeft.position, 0.3f);
    //    Gizmos.DrawWireSphere(patrolRight.position, 0.3f);
    //    //Gizmos.DrawLine(patrolLeft.position, patrolRight.position);

    //    Gizmos.DrawWireCube(chaseLeft.position, new Vector2(0.5f, 0.5f));
    //    Gizmos.DrawWireCube(chaseRight.position, new Vector2(0.5f, 0.5f));
    //    //Gizmos.DrawLine(chaseLeft.position, chaseRight.position);

    //    Gizmos.DrawLine(chaseLeft.position, transform.position);
    //    Gizmos.DrawLine(chaseRight.position, transform.position);
    //    Gizmos.DrawLine(patrolLeft.position, transform.position);
    //    Gizmos.DrawLine(patrolRight.position, transform.position);

    //    Gizmos.DrawWireSphere(transform.position, 5f);
    //    Gizmos.DrawWireSphere(transform.position, 6f);
    //}

}
