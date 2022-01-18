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

    private float hurtForce = 10f;
    private Text healthAmount;
    public float health;
    public float damage;
    public GameObject Player;
    [SerializeField]
    private Image health_Stats;


    public void applyDamage(float damage)
    {
        health -= damage;
    }

    public void Display_HealthStats(float health)
    {
        health /= 100f;

        health_Stats.fillAmount = health;

    }



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
 
            rb2.velocity = new Vector2(dirX * playerMoveSpeed, rb2.velocity.y);
    }

    void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        Display_HealthStats(health);

        if (Input.GetButtonDown("Jump"))
            Jump();


        UpdateAnimationState();
        AnimationChange();
        CheckGrounded();
        
  
        
        void UpdateAnimationState()
        {

           if(Input.GetKeyDown(KeyCode.Mouse0))
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
        
    }





    


    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyAI Enemy = other.gameObject.GetComponent<EnemyAI>();
        EnemyAI enemyAI = Enemy.GetComponent<EnemyAI>();
        damage = enemyAI.damage;
        


        if (other.gameObject.CompareTag("Enemy"))
        {
           
           EnemyAI Troll = other.gameObject.GetComponent<EnemyAI>();

            StartCoroutine(Damage());
            Debug.Log("EDetect");

            


            if (state == MovementState.falling)
            {
                
                
                Troll.JumpedOn();
                Jump();
            }
            else
            {
                // Deals with health and Updates UI if health is too low
              

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
                       // Deals with health and Updates UI if health is too low
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


    IEnumerator Damage()
        
    {

        applyDamage(damage);
        yield return new WaitForSeconds(0.4f);
        

    }


    
}