using System.Collections;
using UnityEngine;

public class ArcherAI : MonoBehaviour
{
    [SerializeField] private Transform patrolLeft; // Левая точка патрулирования
    [SerializeField] private Transform patrolRight; // Правая точка патрулирования
    [SerializeField] private float detectionRange = 8f; // Радиус обнаружения игрока
    [SerializeField] private float patrolSpeed = 2f; // Скорость патрулирования
    [SerializeField] private GameObject arrowPrefab; // Префаб стрелы
    [SerializeField] private Transform shootPoint; // Точка, из которой вылетают стрелы
    [SerializeField] private float shootInterval = 2f; // Интервал между выстрелами
    [SerializeField] private int maxHealth = 3; // Максимальное здоровье

    private Transform player;
    private Animator animator;
    private Transform currentTarget; // Текущая точка патрулирования
    private bool isShooting = false;
    private int currentHealth;
    private Vector3 pos, velocity;
    private bool getHit = false;
    private SoundManager soundManager;

    private enum ArcherState { Patrolling, Shooting, TakingDamage, Dead }
    private ArcherState currentState = ArcherState.Patrolling;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        currentTarget = patrolLeft; // Начинаем патрулирование с левой точки
        currentHealth = maxHealth; // Устанавливаем здоровье
        soundManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundManager>();
    }

    private void Update()
    {
        if (currentState == ArcherState.Dead || getHit == true) return; // Никаких действий, если враг мёртв

        velocity = (transform.position - pos) / Time.deltaTime;
        pos = transform.position;

        animator.SetBool("walking", Mathf.Abs(velocity.x) > 0 ? true : false);

        switch (currentState)
        {
            case ArcherState.Patrolling:
                Patrol();
                CheckForPlayer();
                break;

            case ArcherState.Shooting:
                if (!isShooting && !getHit)
                    StartCoroutine(ShootAtPlayer());
                break;

            case ArcherState.TakingDamage:
                // Здоровье обработано, ждем анимацию
                break;
        }
    }

    private void Patrol()
    {
        // Движение между точками патрулирования
        transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, patrolSpeed * Time.deltaTime);

        // Смена точки назначения, если достигли текущей
        if (Vector3.Distance(transform.position, currentTarget.position) < 0.1f)
        {
            currentTarget = currentTarget == patrolLeft ? patrolRight : patrolLeft;
        }

        // Поворот спрайта в направлении движения
        FlipSprite(currentTarget.position.x);
    }

    private void CheckForPlayer()
    {
        // Если игрок в радиусе обнаружения, переходим в состояние стрельбы
        if (player != null && 
            Vector3.Distance(transform.position, player.position) < detectionRange &&
            !Hero.Instance.isDead)
        {
            currentState = ArcherState.Shooting;
        }
    }

    private IEnumerator ShootAtPlayer()
    {
        isShooting = true;
        animator.SetTrigger("attack"); // Запуск анимации стрельбы

        // Поворот к игроку
        if (player != null)
        {
            FlipSprite(player.position.x);
        }

        yield return new WaitForSeconds(0.7f); // Задержка перед созданием стрелы (время для анимации)

        if (player != null && !Hero.Instance.isDead && !getHit)
        {
            ShootArrow();
        }

        yield return new WaitForSeconds(shootInterval - 0.5f); // Ожидание до следующего выстрела

        // Проверяем, если игрок больше не в радиусе, возвращаемся к патрулированию
        if (Vector3.Distance(transform.position, player.position) > detectionRange || Hero.Instance.isDead)
        {
            currentState = ArcherState.Patrolling;
        }

        isShooting = false;
    }

    private void ShootArrow()
    {
        // Создание стрелы и направление её к игроку (с вертикальным смещением)
        GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, Quaternion.identity);
        Vector2 direction = new Vector2(
            player.position.x - shootPoint.position.x,
            (player.position.y + 1.5f) - shootPoint.position.y
        ).normalized;

        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * 10f; // Скорость полета стрелы
        }

        Destroy(arrow, 5f); // Уничтожение стрелы через 5 секунд, если она не попала в цель
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
        if (currentState == ArcherState.Dead || currentState == ArcherState.TakingDamage) return;
            
        soundManager.PlayEnemyGetHitSound();
        currentHealth -= damage;
        animator.SetTrigger("getHit");
        getHit = true;
        if (currentHealth <= 0)
        {
            currentState = ArcherState.Dead;
            StartCoroutine(Die());
        }
        else
        {
            currentState = ArcherState.TakingDamage;
            StartCoroutine(RecoverFromDamage());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            TakeDamage(1);
            Destroy(collision.gameObject);
        }
    }

    private IEnumerator Die()
    {
        animator.SetTrigger("die");
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    private IEnumerator RecoverFromDamage()
    {
        // значение отмены стрельбы
        yield return new WaitForSeconds(0.5f);
        getHit = false;
        currentState = ArcherState.Patrolling;
    }

    private void OnDrawGizmosSelected()
    {
        // Радиус обнаружения
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);


        // Линии патрулирования
        Gizmos.color = Color.green;
        if (patrolLeft != null && patrolRight != null)
        {
            Gizmos.DrawLine(patrolLeft.position, patrolRight.position);
        }
    }
}
