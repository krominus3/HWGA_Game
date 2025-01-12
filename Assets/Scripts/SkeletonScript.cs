using System.Collections;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    [SerializeField] private Transform patrolLeft;
    [SerializeField] private Transform patrolRight;
    [SerializeField] private Transform chaseLeft;
    [SerializeField] private Transform chaseRight;

    [SerializeField] private float agroRange = 5f;
    [SerializeField] private float chaseRange = 6f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float speedMovementPatrol = 2f;
    [SerializeField] private float speedMovementChase = 4f;
    [SerializeField] private int maxHealth = 3;
    private Animator animator;

    private enum EnemyState { Patrolling, Chasing, Attacking, Returning, TakingDamage, Dead }
    private EnemyState currentState = EnemyState.Patrolling;

    private Transform player;
    private int currentHealth;
    private Vector3 initialPosition;
    //private bool isPlayerInRange;
    private Transform currentTarget; // ������� ����� ��������������
    private Vector3 pos, velocity;

    private bool isAttacking = false; // ����, ������� ����� ���������, ��� ���� �������

    private SoundManager soundManager;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        initialPosition = transform.position;
        currentTarget = patrolRight; // �������� �������������� �� ����� � ������ �����
        pos = transform.position;
        soundManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundManager>();
    }

    private void Update()
    {
        velocity = (transform.position - pos) / Time.deltaTime;
        pos = transform.position;

        animator.SetBool("walking", Mathf.Abs(velocity.x) > 0 ? true : false);

        if (isAttacking) return; // ���� ���� �������, ���������� ������ ��������

        switch (currentState)
        {
            case EnemyState.Patrolling:
                Patrol();
                CheckForPlayer();
                break;

            case EnemyState.Chasing:
                Chase();
                CheckAttackRange();
                break;

            case EnemyState.Attacking:
                Attack();
                break;

            case EnemyState.Returning:
                ReturnToPatrol();
                break;

            case EnemyState.TakingDamage:
                break;

            case EnemyState.Dead:
                // ������ ������
                break;
        }
    }

    private void Patrol()
    {
        // �������� �� ������ � ���� ��������
        if (Vector3.Distance(transform.position, player.position) < agroRange && !Hero.Instance.isDead)
        {
            currentState = EnemyState.Chasing;
            return; // ���������� ��������������, ���� ������ ������������ ������
        }

        // ��������� � ������� �����
        MoveTowards(currentTarget.position, speedMovementPatrol);

        // ���� ���� ������ ������� ����, ����������� �����
        if (Vector3.Distance(transform.position, currentTarget.position) < 0.1f)
        {
            currentTarget = currentTarget == patrolLeft ? patrolRight : patrolLeft;
        }
    }

    private void CheckForPlayer()
    {
        // ���� ����� �����, �� ��������� ���
        if (Hero.Instance.isDead) return;

        if (player != null && Vector3.Distance(transform.position, player.position) < agroRange)
        {
            currentState = EnemyState.Chasing;
        }
    }

    private void Chase()
    {
        if (Hero.Instance.isDead) return;

        Vector3 target = player.position;

        // ������������ �������� ����� ������ �� ��� X � �������� ������� ������
        target = new Vector3(Mathf.Clamp(target.x, chaseLeft.position.x, chaseRight.position.x), transform.position.y, transform.position.z);

        MoveTowards(target, speedMovementChase);

        // ���� ����� ����� �� ������� chaseRange, ���� ������������ � �������
        if (Vector3.Distance(transform.position, player.position) > chaseRange)
        {
            currentState = EnemyState.Returning;
        }
    }

    private void CheckAttackRange()
    {
        if (player != null && Vector3.Distance(transform.position, player.position) < attackRange)
        {
            currentState = EnemyState.Attacking;
        }
    }

    private void Attack()
    {
        isAttacking = true; // ���� ����� �����

        animator.SetTrigger("attack"); // ������ �������� �����

        // ���� ������� ����, ���� ����� ������ � ���� ����� (� Trigger)
        // �� ������ ��� ����� OnTriggerEnter2D, ����� ���� ��� �������, ��� ������ ����� ������� � ��������� �����
        StartCoroutine(EndAttack()); // ��������� ����� ����� ��������
    }

    private IEnumerator EndAttack()
    {
        yield return new WaitForSeconds(1f); // �������� �� ����������������� �������� �����


        if (Vector3.Distance(transform.position, player.position) < chaseRange && !Hero.Instance.isDead)
        {
            currentState = EnemyState.Chasing; // ���� ����� � �������� ���� �������������, ���������� ��� ������������
        }
        else
        {
            currentState = EnemyState.Patrolling; // ���� ����� ������� ������, ������������ � ��������������
        }

        isAttacking = false; // ��������� �����, ���� ������ ����� ���������
    }

    private void ReturnToPatrol()
    {
        MoveTowards(initialPosition, speedMovementPatrol);

        // ���� ����� � ���� ��������, �������� �������������
        if (Vector3.Distance(transform.position, player.position) < agroRange)
        {
            currentState = EnemyState.Chasing;
            return; // ���������� �����������, ���� �������� ������������
        }

        // ���� ���� �������� �� ��������� �����, �������� ��������������
        if (Vector3.Distance(transform.position, initialPosition) < 0.1f)
        {
            currentState = EnemyState.Patrolling;
        }
    }

    private void MoveTowards(Vector3 target, float speed)
    {
        // ��������� ������� ������� �� ��� Y
        Vector3 newPosition = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        transform.position = new Vector3(newPosition.x, transform.position.y, transform.position.z);

        // ������� ������� ��� ��������
        FlipSprite(target.x);
    }

    private void FlipSprite(float targetX)
    {
        if ((targetX < transform.position.x && transform.localScale.x > 0) ||
            (targetX > transform.position.x && transform.localScale.x < 0))
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    public void TakeDamage(int damage)
    {
        if (currentState == EnemyState.Dead || currentState == EnemyState.TakingDamage) return;

        currentHealth -= damage;
        soundManager.PlayEnemyGetHitSound();

        //animator.SetTrigger("hit"); // ��������� �������� ��������� �����

        if (currentHealth <= 0)
        {
            currentState = EnemyState.Dead;
            StartCoroutine(Die());
            animator.SetTrigger("die");
        }
        else
        {
            animator.SetTrigger("getHit");
            currentState = EnemyState.TakingDamage;
            StartCoroutine(RecoverFromDamage());
        }
    }


    private IEnumerator Die()
    {
        Game_manager.Instance.AddKillCoins(10);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    private IEnumerator RecoverFromDamage()
    {
        yield return new WaitForSeconds(0.5f);
        currentState = EnemyState.Patrolling;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //isPlayerInRange = true;

            // ���� ����� ������ � ���� �����, �� �������� ����
            Hero.Instance.GetDamage(1, this.transform);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ���� ������������ �� ��������
        if (collision.gameObject.CompareTag("Projectile"))
        {
            TakeDamage(1); // ���� �������� ����
            Destroy(collision.gameObject); // ���������� ������ ����� ���������
        }
    }

}
