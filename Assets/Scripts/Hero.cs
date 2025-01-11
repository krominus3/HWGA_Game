using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Hero : MonoBehaviour
{
    [SerializeField] public float speed = 6f;
    [SerializeField] public int healthPoints = 3;
    [SerializeField] public float jumpForce = 15f;

    [SerializeField] private float knockBackForceX = 10f;
    [SerializeField] private float knockBackForceY = 10f;
    [SerializeField] private float getHitTime = 0.6f;
    [SerializeField] private float invulnerabilityTime = 1f;

    [SerializeField] private float dashingPower = 24f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;

    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private PhysicsMaterial2D deathMaterial;

    private Rigidbody2D rb;
    private Animator anim;
    private TrailRenderer tr;
    private Collider2D сollider;

    private float horizontal;
    private bool isFacingRight = true;

    private float coyoteTime = 0.15f;
    private float coyoteTimeCounter;
    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    private bool canDash = true;
    private bool isDashing;

    public bool getHit = false;
    public bool isInvulnerability = false;
    public bool isDead = false;
    private bool onGround = true;

    private SoundManager soundManager;
    public static Hero Instance { get; set; }


    private void Awake()
    {
        //-------------
        soundManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundManager>();
        //-------------
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        tr = GetComponent<TrailRenderer>();
        сollider = GetComponent<Collider2D>();
        rb.gravityScale = 3;
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        Game_manager gameManager = Game_manager.Instance;
        speed += gameManager.speedUpgrade;
        healthPoints += gameManager.healthUpgrade;
        jumpForce += gameManager.jumpUpgrade;
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        States();

        if (getHit || isDashing || isDead)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        Jump();
        Flip();

        coyoteTimeCheker();
        jumpBufferCheker();


    }

    private void FixedUpdate()
    {
        if (getHit || isDashing || isDead)
        {
            return;
        }
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private void States()
    {
        if (onGround != isGrounded() && onGround == false)
        {
            soundManager.PlayLandingSound();
        }
        onGround = isGrounded();
        anim.SetFloat("moveX", Mathf.Abs(horizontal));
        anim.SetFloat("moveY", rb.velocity.y);
        anim.SetBool("getHit", getHit);
        anim.SetBool("isDashing", isDashing);
        anim.SetBool("jumping", !onGround);

        if (healthPoints <= 0)
        {
            anim.SetBool("dying", true);
            сollider.sharedMaterial = deathMaterial;
        }
        else
            anim.SetBool("dying", false);

    }

    public void GetDamage(int DamageCount, Transform damagePosition)
    {
        if (isDead || isInvulnerability || getHit) return;
        healthPoints -= DamageCount;
        getHit = true;
        Debug.Log(healthPoints);
        soundManager.PlayGetHitSound();

        if (healthPoints > 0)
        {
            StartCoroutine(Knockback(damagePosition));
        }
        else
        {
            isDead = true;
        }
    }

    private IEnumerator Knockback(Transform damagePosition)
    {
        //Knockback
        Vector2 direction = (transform.position - damagePosition.position).normalized;
        rb.velocity = new Vector2(direction.x * knockBackForceX, knockBackForceY);
        yield return new WaitForSeconds(getHitTime);
        getHit = false;
        StartCoroutine(Invulnerability());
    }

    private IEnumerator Invulnerability()
    {
        isInvulnerability = true;

        Color _color = sr.material.color;
        for (int i = 0; i < 3; i++)
        {
            _color.a = 0.5f;
            sr.material.color = _color;
            yield return new WaitForSeconds(invulnerabilityTime / 4);
            _color.a = 1f;
            sr.material.color = _color;
            yield return new WaitForSeconds(invulnerabilityTime / 4);
        };

        isInvulnerability = false;
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    private void coyoteTimeCheker()
    {
        if (onGround)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;
    }

    private void jumpBufferCheker()
    {
        if ((Input.GetAxisRaw("Jump") > 0 && Input.GetButtonDown("Jump")) || Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            Vector2 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }


    private void Jump()
    {
        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
        {
            soundManager.PlayPreJumpSound();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

            jumpBufferCounter = 0f;
        }

        if (((Input.GetAxisRaw("Jump") == 0 && Input.GetButtonUp("Jump")) || Input.GetKeyUp(KeyCode.Space)) && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);

            coyoteTimeCounter = 0f;
        }

        if (Input.GetAxisRaw("Jump") < 0)
        {
            StartCoroutine(IgnoreLayerOff());
        }

        IEnumerator IgnoreLayerOff()
        {
            Physics2D.IgnoreLayerCollision(6, 8, true);
            yield return new WaitForSeconds(0.3f);
            Physics2D.IgnoreLayerCollision(6, 8, false);
        }
    }

    private bool isGrounded()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.3f);

        //if (Input.GetButton("Jump") && rb.velocity.y > 0f)
        //    return false;
        //else
        //    return collider.Length > 1;
        if ((Input.GetButton("Jump") || Input.GetKey(KeyCode.Space)) && rb.velocity.y > 0f)
            return false;
        else
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Ground"))
                {
                    return true;
                }
            }
        return false;
    }
}
