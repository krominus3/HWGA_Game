using UnityEngine;

public class BossStateMachine : MonoBehaviour
{
    public enum BossState { Idle, Chase, Attack, JumpAttack }
    private BossState currentState;

    [SerializeField] private Transform player;
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private int damage = 1;
    [SerializeField] private int health = 10; // Boss health

    private Rigidbody2D rb;
    private bool isGrounded;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentState = BossState.Idle;
    }

    void Update()
    {
        

        switch (currentState)
        {
            case BossState.Idle:
                HandleIdleState();
                CheckForPlayer();
                CheckJumpAttack();
                break;
            case BossState.Chase:
                HandleChaseState();
                CheckAttackRange();
                CheckJumpAttack();
                break;
            case BossState.Attack:
                HandleAttackState();
                CheckForChaseAfterAttack();
                break;
            case BossState.JumpAttack:
                HandleJumpAttackState();
                CheckForChaseAfterJump();
                break;
        }
    }

    private void HandleIdleState()
    {
        // Idle logic here
    }

    private void HandleChaseState()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * chaseSpeed, rb.velocity.y);
    }

    private void HandleAttackState()
    {
        rb.velocity = Vector2.zero;
        Hero.Instance.GetDamage(damage, transform);
    }

    private void HandleJumpAttackState()
    {
        if (isGrounded)
        {
            Vector2 jumpDirection = (player.position - transform.position).normalized;
            rb.velocity = new Vector2(jumpDirection.x * chaseSpeed, jumpForce);
        }
    }

    private void CheckForPlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer > attackRange && isGrounded)
        {
            currentState = BossState.Chase;
        }
        else if (distanceToPlayer <= attackRange && isGrounded)
        {
            currentState = BossState.Attack;
        }
    }

    private void CheckJumpAttack()
    {
        if (player.position.y > transform.position.y + 1f && isGrounded)
        {
            currentState = BossState.JumpAttack;
        }
    }

    private void CheckAttackRange()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange && isGrounded)
        {
            currentState = BossState.Attack;
        }
        else if (distanceToPlayer > attackRange)
        {
            currentState = BossState.Idle;
        }
    }

    private void CheckForChaseAfterAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer > attackRange)
        {
            currentState = BossState.Chase;
        }
    }

    private void CheckForChaseAfterJump()
    {
        if (isGrounded)
        {
            currentState = BossState.Chase;
        }
    }

    private void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        rb.simulated = false;
        // Logic for boss death
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            TakeDamage(1); // Assume projectile deals 1 damage
            Destroy(collision.gameObject); // Destroy the projectile
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
