using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Hero : MonoBehaviour
{
    [SerializeField] private float speed = 6f;
    [SerializeField] private int lives = 5;
    [SerializeField] private float jumpForce = 15f;

    private bool isGrounded = false;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;

    public enum States
    {
        idle_hero_anim,
        run_hero_anim,
        jump_hero_anim,
        fall_hero_anim
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb.gravityScale = 3;
    }

    private States State
    {
        get { return (States)anim.GetInteger("State"); }
        set { anim.SetInteger("State", (int)value); }
    }

    private void Flip()
    {
        if (Input.GetAxisRaw("Horizontal") == 1)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (Input.GetAxisRaw("Horizontal") == -1)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    private void Run()
    {
        if (isGrounded) State = States.run_hero_anim;
        Vector3 dir = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);

        //sprite.flipX = dir.x < 0.0f;
    }

    private void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void CheckGround()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 0.3f);
        isGrounded = collider.Length > 1;

        //if (!isGrounded) State = States.jump_hero_anim;
    }    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckGround();

        if (Input.GetAxis("Horizontal") != 0)
        {
            Flip();
        }

        if (rb.velocity.y > 0.0f)
        {
            if (!isGrounded) State = States.jump_hero_anim;
        }
        else
        {
            if (!isGrounded) State = States.fall_hero_anim;
        }
        if(isGrounded) State = States.idle_hero_anim;

        if (Input.GetButton("Horizontal")) 
            Run();
        
        if (isGrounded && Input.GetButtonDown("Jump"))
            Jump();

    }
}
