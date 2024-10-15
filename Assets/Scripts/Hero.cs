using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Hero : MonoBehaviour
{
    [SerializeField] private float speed = 6f;
    [SerializeField] private int healthPoints = 3;
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private bool isGrounded = false;
    private Rigidbody2D rb;
    private Animator anim;

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
        States();
        CheckGround();
        Flip();

        if (Input.GetButton("Horizontal")) 
            Run();
        
        if (isGrounded && Input.GetButtonDown("Jump"))
            Jump();

        //if (Input.GetKey(KeyCode.Z))
        //    State = States.dash_hero_anim;
        //if (Input.GetKey(KeyCode.X))
        //    State = States.death_hero_anim;
        //if (Input.GetKey(KeyCode.C))
        //    State = States.getHit_hero_anim;
        //if (Input.GetKey(KeyCode.V))
        //    State = States.punch_hero_anim;

    }

    public void GetDamage(int count)
    {
        healthPoints -= count;
        Debug.Log(healthPoints);
    }

    private void States()
    {
        anim.SetFloat("moveX", Mathf.Abs(Input.GetAxisRaw("Horizontal")));
        anim.SetFloat("moveY", rb.velocity.y);
        if (healthPoints <= 0)
            anim.SetBool("dying", true);

        if (isGrounded)
            anim.SetBool("jumping", false);
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

        Vector3 dir = transform.right * Input.GetAxisRaw("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);
    }

    private void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void CheckGround()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 0.3f);
        isGrounded = collider.Length > 1;
    }    

}
