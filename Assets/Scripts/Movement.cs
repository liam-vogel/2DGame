 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class Movement : MonoBehaviour
{
    private Rigidbody2D rb2;
    private BoxCollider2D coll;
    private BoxCollider2D collT;
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
    private int jumpsCount;
    private float jumpCoolDown;
    public float health;
    public float damage;
    private float Edamage = 10f;
    public GameObject Player;
    [SerializeField]
    private Image health_Stats;
    private float nextAttackTime = 0f;
    public Transform attackPoint;
    public float attackRange;
    public float attackRate;
    public LayerMask enemyLayers;
    private GameObject Enemy;
    private GameObject EnemyAI;
   


    void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        Display_HealthStats(health);

        if (Input.GetButtonDown("Jump"))
            Jump();


        UpdateAnimationState();
        AnimationChange();
        CheckGrounded();
        StartCoroutine(Pdie());
      
            

    }


    private IEnumerator Pdie()
     {
        if (health <= 0)
        {
            anim.SetBool("die", true);
            yield return new WaitForSeconds(0.5f); 
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void FixedUpdate()
    {

        rb2.velocity = new Vector2(dirX * playerMoveSpeed, rb2.velocity.y);
    }




    private void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    void Attack()
   {
        anim.SetTrigger("Attack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyAI>().TakeDamage(damage);
        }

           
    
    
    }
    public void applyDamage(float Edamage)
    {
        health -= Edamage;
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
        coll = GetComponent<BoxCollider2D>();
        collT = GetComponent<BoxCollider2D>();
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
        if (Physics2D.OverlapCircle(Feet.position, 0.1f, Groundlayer))

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


    void AnimationChange()
    {
        if (state == MovementState.idle)
        {
            anim.SetBool("run", false);
            anim.SetBool("jump", false);
            anim.SetBool("idle", true);

        }

        if (state == MovementState.running)
        {
            anim.SetBool("run", true);
            anim.SetBool("jump", false);
            anim.SetBool("idle", false);

        }

        if (state == MovementState.jumping)
        {
            anim.SetBool("run", false);
            anim.SetBool("jump", true);
            anim.SetBool("idle", false);

        }



        if (state == MovementState.attacking)
        {
            anim.SetBool("run", false);
            anim.SetBool("jump", false);

            anim.SetBool("idle", false);
        }


        if (state == MovementState.hurt)
        {
            anim.SetBool("run", false);
            anim.SetBool("jump", false);

            anim.SetBool("idle", false);
            anim.SetBool("hurt", true);
        }
    }


    void UpdateAnimationState()
    {
        if(Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
       


        if (dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
            knight.transform.localScale = new Vector3(-1, 1, 1);

        }
        else if (dirX < 0)
        {
            state = MovementState.running;
            sprite.flipX = true;
            knight.transform.localScale = new Vector3(1, 1, 1);

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
            // state = MovementState.falling;
        }
        
        
       

 
            

          


    }


    





    


    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyAI Enemy = other.gameObject.GetComponent<EnemyAI>();
        EnemyAI enemyAI = Enemy.GetComponent<EnemyAI>();
        Edamage = enemyAI.damage;



        if (other.gameObject.CompareTag("Enemy"))
        {

            //EnemyAI Troll = other.gameObject.GetComponent<EnemyAI>();
            // if(Time.time >= nextHitTime)
            // {
            applyDamage(Edamage);
            //nextHitTime = Time.time + 0.6f;
            // }



        }
        else if (other.gameObject.CompareTag("spikes")) 
        {
            applyDamage(Edamage);



        }

    }
}


    


    
