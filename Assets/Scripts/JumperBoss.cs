using UnityEngine;
using System.Collections;

public class BossStateMachine : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private int damage = 1;
    [SerializeField] private int health = 10;
    [SerializeField] private float knockbackForce = 1f;
    [SerializeField] private float stunDuration = 0.3f;

    public enum BossState { Idle, Chase, Attack, JumpAttack, Stunned, Dying }
    private BossState currentState;

    private Rigidbody2D rb;
    private bool isGrounded;
    private Animator anim;
    private bool isFacingRight = true;

    private SoundManager soundManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentState = BossState.Idle;
        soundManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundManager>();
    }

    void Update()
    {
        if (!rb.simulated || currentState == BossState.Stunned || currentState == BossState.Dying) return;

        States();
        Flip();
        print(currentState);
        switch (currentState)
        {
            case BossState.Idle:
                HandleIdleState();
                CheckForPlayer();
                break;
            case BossState.Chase:
                HandleChaseState();
                CheckJumpAttack();
                break;
            case BossState.JumpAttack:
                HandleJumpAttackState();
                CheckForChaseAfterJump();
                break;
        }
    }

    private void States()
    {
        anim.SetFloat("velocityY", rb.velocity.y);
        anim.SetBool("walking", Mathf.Abs(rb.velocity.x) > 0);
        anim.SetBool("isGrounded", isGrounded);
    }

    private void Flip()
    {
        if (isFacingRight && rb.velocity.x < 0f || !isFacingRight && rb.velocity.x > 0f)
        {
            Vector2 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
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

    private void CheckForChaseAfterJump()
    {
        if (isGrounded)
        {
            currentState = BossState.Chase;
        }
    }

    public void TakeDamage(int damage)
    {
        if (currentState == BossState.Stunned || currentState == BossState.Dying) return;

        soundManager.PlayEnemyGetHitSound();
        health -= damage;
        if (health > 0)
        {
            StartCoroutine(HandleStun());
        }
        else
        {
            StartCoroutine(HandleDeath());
        }
    }

    private IEnumerator HandleStun()
    {
        currentState = BossState.Stunned;
        anim.SetTrigger("getHit");

        // Knockback logic
        Vector2 knockbackDirection = (transform.position - player.position).normalized;
        rb.velocity = new Vector2(knockbackDirection.x * knockbackForce, rb.velocity.y);

        yield return new WaitForSeconds(stunDuration);
        anim.SetTrigger("exitGetHit");
        currentState = BossState.Idle;
    }

    private IEnumerator HandleDeath()
    {
        currentState = BossState.Dying;
        rb.velocity = Vector2.zero;
        rb.simulated = false;
        anim.SetTrigger("die");
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Projectile":
                TakeDamage(1); // Assume projectile deals 1 damage
                Destroy(collision.gameObject); // Destroy the projectile
                return;
            case "Ground":
                isGrounded = true;
                return;
            case "Player":
                Hero.Instance.GetDamage(damage, rb.transform);
                return;
            default:
                return;
            
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
