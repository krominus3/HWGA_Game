using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Hero : MonoBehaviour
{
    [SerializeField] private float speed = 6f;
    [SerializeField] public int healthPoints = 3;
    [SerializeField] private float jumpForce = 15f;

    [SerializeField] private float knockBackForceX = 10f;
    [SerializeField] private float knockBackForceY = 10f;
    [SerializeField] private float getHitTime = 0.6f;
    [SerializeField] private float invulnerabilityTime = 1f;

    [SerializeField] private float dashingPower = 24f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;

    [SerializeField] SpriteRenderer sr;

    private Rigidbody2D rb;
    private Animator anim;
    private TrailRenderer tr;

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
    private bool isDeath = false;

    public static Hero Instance { get; set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        tr = GetComponent<TrailRenderer>();
        rb.gravityScale = 3;
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        States();

        if (getHit || isDashing || isDeath)
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
        if (getHit || isDashing || isDeath)
        {
            return;
        }
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private void States()
    {
        anim.SetFloat("moveX", Mathf.Abs(Input.GetAxisRaw("Horizontal")));
        anim.SetFloat("moveY", rb.velocity.y);
        anim.SetBool("getHit", getHit);
        anim.SetBool("isDashing", isDashing);

        if (healthPoints <= 0)
            anim.SetBool("dying", true);
        anim.SetBool("jumping", !isGrounded());

    }

    public void GetDamage(int DamageCount, Transform damagePosition)
    {
        healthPoints -= DamageCount;
        getHit = true;
        Debug.Log(healthPoints);

        if (healthPoints > 0)
        {
            StartCoroutine(Knockback(damagePosition));
        }
        else
        {
            isDeath = true;
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
        if (isGrounded())
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;
    }

    private void jumpBufferCheker()
    {
        if (Input.GetButtonDown("Jump"))
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
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

            jumpBufferCounter = 0f;
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);

            coyoteTimeCounter = 0f;
        }
    }

    private bool isGrounded()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.3f);

        //if (Input.GetButton("Jump") && rb.velocity.y > 0f)
        //    return false;
        //else
        //    return collider.Length > 1;
        if (Input.GetButton("Jump") && rb.velocity.y > 0f)
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
