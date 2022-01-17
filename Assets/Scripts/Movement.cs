 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



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

    [SerializeField] private LayerMask Ground;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private int cherries = 0;
    [SerializeField] private Text cherryText;
    [SerializeField] private float hurtForce = 10f;
    [SerializeField] private AudioSource cherry;
    [SerializeField] private AudioSource walking;
    [SerializeField] private AudioSource jumping;
    [SerializeField] private AudioSource hurt;
    [SerializeField] private AudioSource powerup;
    [SerializeField] private AudioSource speedup;
    [SerializeField] private int health;
    [SerializeField] private Text healthAmount;

    //newDash
    // private int direction;
    private float dashTime;
    public float startDashTime;
    private float dashSpeed;
    public GameObject dashEffect;


    private enum MovementState { idle, running, jumping, falling, disappear, attacking, hurt }

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
        

       // if (!isDashing)
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
        
       /* if(direction == 0)
        {
            if(Input.GetKeyDown(KeyCode.Q))
            {
                Instantiate(dashEffect, transform.position, Quaternion.identity);
                direction = 1;

            }
            else if(Input.GetKeyDown(KeyCode.E))
            {
                Instantiate(dashEffect, transform.position, Quaternion.identity);
                direction = 2;
            }
            else if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                Instantiate(dashEffect, transform.position, Quaternion.identity);
                direction = 3;
            }
            else if(Input.GetKeyDown(KeyCode.DownArrow))
            {
                Instantiate(dashEffect, transform.position, Quaternion.identity);
                direction = 4;
            }
            else
            {
                if (dashTime <= 0)
                {
                    direction = 0;
                    dashTime = startDashTime;
                    rb2.velocity = Vector2.zero;
                }
                else
                {
                    dashTime -= Time.deltaTime;
                }
            
            
                if(direction == 1)
                {
                    rb2.velocity = Vector2.left * dashSpeed;
                }
                else if(direction == 2)
                {
                    rb2.velocity = Vector2.right * dashSpeed;
                } 
                else if(direction == 3)
                {
                    rb2.velocity = Vector2.up * dashSpeed;
                } 
                else if(direction == 4)
                {
                    rb2.velocity = Vector2.down * dashSpeed;
                }
            
            
            }
        
        
        }*/
        
        //OldDash
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (doubleTapTime > Time.time && lastKeyCode == KeyCode.A)
            {
                //Dash
                StartCoroutine(Dash(-1f));
                Debug.Log("Test");
            }

            else
            {
                doubleTapTime = Time.time + 0.2f;
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
                doubleTapTime = Time.time + 0.2f;
            }

            lastKeyCode = KeyCode.D;

        }
        
        void UpdateAnimationState()
        {

           if(Input.GetKey(KeyCode.Mouse0))
            {
                anim.SetTrigger("Attack");
            }
                
            
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



            if (state == MovementState.attacking)
            {
                  anim.SetBool("run", false);
                  anim.SetBool("jump", false);
                 anim.SetBool("attack", true);
                  anim.SetBool("idle", false);
            }
        
        
            if(state == MovementState.hurt)
            {
                anim.SetBool("run", false);
                anim.SetBool("jump", false);
                anim.SetBool("attack", false);
                anim.SetBool("idle", false);
                anim.SetBool("hurt", true);
            }
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
            Debug.Log(dashDistance);
        float gravity = rb2.gravityScale;
       // Instantiate(dashEffect, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(2f);
        isDashing = false;
        rb2.gravityScale = gravity;
       // Object.Destroy(dashEffect);
        

    }




    private void OnCollisionEnter2D(Collision2D other)
    {
        EnemyAI Enemy = other.gameObject.GetComponent<EnemyAI>();


        if (other.gameObject.tag == "Enemy")
        {

            
            EnemyAI Troll = other.gameObject.GetComponent<EnemyAI>();

            if (state == MovementState.falling)
            {
                
                
                Troll.JumpedOn();
                Jump();
            }
            else
            {
                hurt.Play(); // Deals with health and Updates UI if health is too low
                state = MovementState.hurt;
                health -= 25;
                healthAmount.text = health.ToString();
                if (health <= 0)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }

                if (other.gameObject.transform.position.x > transform.position.x)
                {
                    //Enemy is to my right therefore should be damaged and move left
                    rb2.velocity = new Vector2(-hurtForce, rb2.velocity.y);
                }
                if (other.gameObject.tag == "EnemyAI")
                {
                    if (state == MovementState.falling)
                    {
                      //  hurt.Play();
                     //   eagle.JumpedOn();
                     //   possum.JumpedOn();
                        Jump();
                    }
                    else
                    {
                        hurt.Play(); // Deals with health and Updates UI if health is too low
                        state = MovementState.hurt;
                        health -= 25;
                        healthAmount.text = health.ToString();
                        if (health <= 0)
                        {
                            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                        }
                        else
                        {
                            //Enemy is to my left therefore i Should be damaged and move right
                            rb2.velocity = new Vector2(hurtForce, rb2.velocity.y);
                        }
                    }

                }
                else
                {
                    //Enemy is to my left therefore i Should be damaged and move right
                    rb2.velocity = new Vector2(hurtForce, rb2.velocity.y);
                }

            }

        }
    }



    
}