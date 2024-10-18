using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Hero : MonoBehaviour
{
    [SerializeField] private float speed = 6f;
    [SerializeField] private int healthPoints = 3;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float knockBackForceX = 10f;
    [SerializeField] private float knockBackForceY = 10f;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private Rigidbody2D rb;
    private Animator anim;

    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;
    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;
    private float horizontal;

    public bool getHit = false;

    public static Hero Instance { get; set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rb.gravityScale = 3;
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (!getHit)
        {
            Jump();
            Run();
            Flip();
        }
        States();
        coyoteTimeCheker();
        jumpBufferCheker();

    }

    public void GetDamage(int DamageCount, Transform damagePosition)
    {
        //Damage player
        healthPoints -= DamageCount;
        getHit = true;
        Debug.Log(healthPoints);

        //Knockback
        Vector2 direction = (transform.position - damagePosition.position).normalized;
        rb.velocity = new Vector2(direction.x * knockBackForceX, knockBackForceY);

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

    private void States()
    {
        anim.SetFloat("moveX", Mathf.Abs(Input.GetAxisRaw("Horizontal")));
        anim.SetFloat("moveY", rb.velocity.y);
        anim.SetBool("getHit", getHit);

        if (healthPoints <= 0)
            anim.SetBool("dying", true);

        if (isGrounded())
        {
            //getHit = false;
            anim.SetBool("jumping", false);
        }
        else
            anim.SetBool("jumping", true);
    }

    private void Flip()
    {
        if (Input.GetAxisRaw("Horizontal") < 0)
            _spriteRenderer.flipX = true;
        else if (Input.GetAxisRaw("Horizontal") > 0)
            _spriteRenderer.flipX = false;
    }

    private void Run()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
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
        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 0.3f);
        return collider.Length > 1;
    }
    

}
