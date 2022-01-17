using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallThroughPlatform : MonoBehaviour
{
    private PlatformEffector2D effector;
    private Rigidbody2D rb;
    public float wait;
    [SerializeField] bool willFall;
    [SerializeField] Transform Feet;
    [SerializeField] LayerMask Falling;
    private RigidbodyConstraints2D constraints;
    private Collider2D coll;
    private Collider2D coll2;
    public GameObject WillFall;
    public bool isStatic;


    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        coll2 = GetComponentInChildren<Collider2D>();

    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            wait = 0.1f;
            effector.rotationalOffset = 0f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            if (wait <= 0)
            {
                effector.rotationalOffset = 180f;
                wait = 0.1f;
            }

            else
            {
                wait -= Time.deltaTime;
            }
        }

        if (Input.GetKey(KeyCode.Space))
        {
            effector.rotationalOffset = 0f;
        }

        if (Input.GetKey(KeyCode.W))
        {
            effector.rotationalOffset = 0f;
        }
        
       
        

           

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Physics2D.OverlapCircle(Feet.position, 0.5f, Falling))
        {
            willFall = true;
            StartCoroutine(Fall(willFall));
        }

        if (collision.gameObject.CompareTag("DestroyPlat"))
        {
            Destroy(this.gameObject);
        }
              

    }
    private IEnumerator Fall(bool willFall)
    {
        
        if(isStatic == false)
        {

          if(willFall == true)
          {
                yield return new WaitForSeconds(0.8f);
                coll.isTrigger = true;
                coll2.isTrigger = true;
                rb.bodyType = RigidbodyType2D.Dynamic;
                WillFall.SetActive(false);

          }

        }
        

    }

}



