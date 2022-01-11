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

    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb2.velocity = new Vector2(dirX * playerMoveSpeed, rb2.velocity.y);

        if (Input.GetButtonDown("Jump") && Mathf.Abs(rb2.velocity.y) < 0.001f)
        {
            rb2.AddForce(new Vector2(0, playerJumpHeight), ForceMode2D.Impulse);
        }

        UpdateAnimationState();
        AnimationChange();
    }

    private void UpdateAnimationState()
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

    public void AnimationChange()
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