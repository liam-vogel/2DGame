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

    MovementState state;

    private float dirX = 0f;

    [SerializeField] private float playerMoveSpeed = 5f;
    [SerializeField] private float playerJumpHeight = 15f;

    private enum MovementState { idle, running, jumping, falling, disappear }

    private void Start()
    {
        rb2 = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        Cursor.visible = false;
    }

    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb2.velocity = new Vector2(dirX * playerMoveSpeed, rb2.velocity.y);

        if (Input.GetButtonDown("Jump") && Mathf.Abs(rb2.velocity.y) < 0.001f)
        {
            rb2.AddForce(new Vector2(0, playerJumpHeight), ForceMode2D.Impulse);
        }

        //UpdateAnimationState();
    }

   /* private void UpdateAnimationState()
    {
        if (dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }
        if (rb2.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (rb2.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }
        anim.SetInteger("state", (int)state);
    }*/

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            state = MovementState.disappear;
        }
    }
}