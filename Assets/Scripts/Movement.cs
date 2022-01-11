 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Movement : MonoBehaviour
{
    private Rigidbody2D rb2;
    private CapsuleCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;
    private GameObject knight;

    MovementState state;

    private float dirX = 0f;

    [SerializeField] private float playerMoveSpeed = 5f;
    [SerializeField] private float playerJumpHeight = 15f;
    [SerializeField] Transform Feet;
    [SerializeField] LayerMask Groundlayer;
    bool isGrounded;
    public int extraJumps = 1;
    public int jumpsCount;
    public float jumpCoolDown;

    public float dashDistance = 15f;
    bool isDashing;
    float doubleTapTime;
    KeyCode lastKeyCode;

    private enum MovementState { idle, running, jumping, falling, disappear }

    private void Start()
    {
        rb2 = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        knight = GameObject.Find("sword_man");
        Cursor.visible = false;
    }

  

    void Jump()
    {
        if (isGrounded || jumpsCount < extraJumps)
        {
            rb2.velocity = new Vector2(rb2.velocity.x, playerJumpHeight);
            jumpsCount++;
        }
    }

    private void CheckGrounded()
    {
        if (Physics2D.OverlapCircle(Feet.position, 0.5f, Groundlayer))

        {
            isGrounded = true;
            jumpsCount = 0;
            jumpCoolDown = Time.time + 0.2f;


        }
        else if (Time.time < jumpCoolDown)
        {
            isGrounded = true;

        }
        else
        {
            isGrounded = false;
        }

    }

    private void FixedUpdate()
    {
        

        if (!isDashing)
            rb2.velocity = new Vector2(dirX * playerMoveSpeed, rb2.velocity.y);
    }

    void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb2.velocity = new Vector2(dirX * playerMoveSpeed, rb2.velocity.y);

        if (Input.GetButtonDown("Jump"))
            Jump();


        UpdateAnimationState();
        AnimationChange();
        CheckGrounded();

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (doubleTapTime > Time.time && lastKeyCode == KeyCode.A)
            {
                //Dash
                StartCoroutine(Dash(-1f));
            }

            else
           {
                doubleTapTime = Time.time + 0.5f;
            }

            lastKeyCode = KeyCode.A;
        }


        if (Input.GetKeyDown(KeyCode.D))
        {
            if (doubleTapTime > Time.time && lastKeyCode == KeyCode.D)
            {
                //Dash
                StartCoroutine(Dash(1f));
            }

            else
            {
                doubleTapTime = Time.time + 0.5f;
            }

            lastKeyCode = KeyCode.D;

        }

        void UpdateAnimationState()
        {
            if (dirX > 0f)
            {
                state = MovementState.running;
                sprite.flipX = false;
                knight.transform.localScale = new Vector3(-1, 1, 1);
                // anim.SetBool("idle", false);
                // anim.SetBool("run", true);
            }
            else if (dirX < 0)
            {
                state = MovementState.running;
                sprite.flipX = true;
                knight.transform.localScale = new Vector3(1, 1, 1);
                //anim.SetBool("idle", false);
                // anim.SetBool("run", true);
            }
            else
            {
                state = MovementState.idle;
                //anim.SetBool("idle", true);
            }
            if (rb2.velocity.y > .1f)
            {
                state = MovementState.jumping;
            }
            else if (rb2.velocity.y < -.1f)
            {
                // state = MovementState.falling;
            }
            //  anim.SetInteger("state", (int)state);
        }

        void AnimationChange()
        {
            if (state == MovementState.idle)
            {
                anim.SetBool("run", false);
                anim.SetBool("jump", false);
                //anim.SetBool("fall", false);
                anim.SetBool("idle", true);
            }

            if (state == MovementState.running)
            {
                anim.SetBool("run", true);
                anim.SetBool("jump", false);
                // anim.SetBool("fall", false);
                anim.SetBool("idle", false);
            }

            if (state == MovementState.jumping)
            {
                anim.SetBool("run", false);
                anim.SetBool("jump", true);
                // anim.SetBool("fall", false);
                anim.SetBool("idle", false);
            }



            //if (state == MovementState.falling)
            //{
            //     anim.SetBool("run", false);
            //      anim.SetBool("jump", false);
            // anim.SetBool("fall", true);
            //       anim.SetBool("idle", false);
            //   }
        }
        /*void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                state = MovementState.disappear;
            }
        }*/
    }

    IEnumerator Dash(float direction)
    {
        isDashing = true;
        rb2.velocity = new Vector2(rb2.velocity.x, 0f);
        rb2.AddForce(new Vector2(dashDistance * direction, 0f), ForceMode2D.Impulse);
        float gravity = rb2.gravityScale;
        yield return new WaitForSeconds(0.4f);
        isDashing = false;
        rb2.gravityScale = gravity;


    }

}