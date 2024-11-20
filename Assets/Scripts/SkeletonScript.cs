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
    private Transform currentTarget; // “екуща€ точка патрулировани€
    private Vector3 pos, velocity;

    private bool isAttacking = false; // ‘лаг, который будет указывать, что враг атакует

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        initialPosition = transform.position;
        currentTarget = patrolRight; // Ќачинаем патрулирование от левой к правой точке
        pos = transform.position;
    }

    private void Update()
    {
        velocity = (transform.position - pos) / Time.deltaTime;
        pos = transform.position;

        animator.SetBool("walking", Mathf.Abs(velocity.x) > 0 ? true : false);

        if (isAttacking) return; // ≈сли враг атакует, игнорируем другие действи€

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
                // ¬ызов анимации и переход в другое состо€ние через корутину
                break;

            case EnemyState.Dead:
                // Ћогика смерти
                break;
        }
    }

    private void Patrol()
    {
        // ѕроверка на игрока в зоне агрессии
        if (Vector3.Distance(transform.position, player.position) < agroRange && !Hero.Instance.isDead)
        {
            currentState = EnemyState.Chasing;
            return; // ѕрекращаем патрулирование, если начали преследовать игрока
        }

        // ƒвигаемс€ к текущей точке
        MoveTowards(currentTarget.position, speedMovementPatrol);

        // ≈сли враг достиг текущей цели, переключаем точку
        if (Vector3.Distance(transform.position, currentTarget.position) < 0.1f)
        {
            currentTarget = currentTarget == patrolLeft ? patrolRight : patrolLeft;
        }
    }

    private void CheckForPlayer()
    {
        // ≈сли игрок мертв, не провер€ем его
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

        // ќграничиваем движение врага только по оси X в пределах области погони
        target = new Vector3(Mathf.Clamp(target.x, chaseLeft.position.x, chaseRight.position.x), transform.position.y, transform.position.z);

        MoveTowards(target, speedMovementChase);

        // ≈сли игрок вышел за пределы chaseRange, враг возвращаетс€ к патрулю
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
        isAttacking = true; // ¬раг начал атаку

        animator.SetTrigger("attack"); // «апуск анимации атаки

        // ¬раг наносит урон, если игрок входит в зону атаки (в Trigger)
        // ћы делаем это через OnTriggerEnter2D, чтобы урон был нанесен, как только игрок попадет в коллайдер атаки
        StartCoroutine(EndAttack()); // «авершаем атаку после задержки
    }

    private IEnumerator EndAttack()
    {
        yield return new WaitForSeconds(1f); // «адержка на продолжительность анимации атаки


        if (Vector3.Distance(transform.position, player.position) < chaseRange && !Hero.Instance.isDead)
        {
            currentState = EnemyState.Chasing; // ≈сли игрок в пределах зоны преследовани€, продолжаем его преследовать
        }
        else
        {
            currentState = EnemyState.Patrolling; // ≈сли игрок слишком далеко, возвращаемс€ к патрулированию
        }

        isAttacking = false; // «авершаем атаку, враг теперь может двигатьс€
    }

    private void ReturnToPatrol()
    {
        MoveTowards(initialPosition, speedMovementPatrol);

        // ≈сли игрок в зоне агрессии, начинаем преследование
        if (Vector3.Distance(transform.position, player.position) < agroRange)
        {
            currentState = EnemyState.Chasing;
            return; // ѕрекращаем возвращение, если начинаем преследовать
        }

        // ≈сли враг вернулс€ на начальную точку, начинаем патрулирование
        if (Vector3.Distance(transform.position, initialPosition) < 0.1f)
        {
            currentState = EnemyState.Patrolling;
        }
    }

    private void MoveTowards(Vector3 target, float speed)
    {
        // —охран€ем текущую позицию по оси Y
        Vector3 newPosition = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        transform.position = new Vector3(newPosition.x, transform.position.y, transform.position.z);

        // ѕоворот спрайта при движении
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
        if (currentState == EnemyState.Dead) return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentState = EnemyState.Dead;
            StartCoroutine(Die());
        }
        else
        {
            currentState = EnemyState.TakingDamage;
            StartCoroutine(RecoverFromDamage());
        }
    }

    private IEnumerator Die()
    {
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

            // ≈сли игрок входит в зону атаки, он получает урон
            Hero.Instance.GetDamage(1, this.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //isPlayerInRange = false;
        }
    }
}
