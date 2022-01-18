using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class EnemyAI : MonoBehaviour
{
    public float damage = 20f;
    //waypoints should have a minimum of 2 to move between.
    public List<Transform> points;
    public int nextID = 0;
    public float speed = 2f;
    public GameObject Waypoints;
    private int idChangeValue = 1;

    private Rigidbody2D rb2;
    private Animator anim;

    private enum MovementState { disappear }

    // Start is called before the first frame update
    private void Start()
    {
        anim = GetComponent<Animator>();
        rb2 = GetComponent<Rigidbody2D>();
    }

    private void Reset()
    {
        {
            Init();
        }
    }

    void Init()
    {
        //set BC2 to trigger
        GetComponent<BoxCollider2D>().isTrigger = true;

        //create root object
        GameObject root = new GameObject(name + "_Root");
        //reset root pos to game obj (enemy)
        root.transform.position = transform.position;
        //set enemy as child of root
        transform.SetParent(root.transform);
        //make waypoint objs + reset pos to root + make waypoing obj child of root
        GameObject waypoints = new GameObject("Waypoints");
        waypoints.transform.SetParent(root.transform);
        waypoints.transform.position = root.transform.position;
        //create two points and reset pos  to waypoint objs
        GameObject p1 = new GameObject("EnemyWP1");p1.transform.SetParent(waypoints.transform);p1.transform.position = root.transform.position;
        GameObject p2 = new GameObject("EnemyWP2");p2.transform.SetParent(waypoints.transform);p2.transform.position = root.transform.position;
        //make points child of waypoint obj

        //Init points list, add points to it
        points = new List<Transform>();
        points.Add(p1.transform);
        points.Add(p2.transform);
    }

    private void Update()
    {
        MoveToNextpoint();
    }

    void MoveToNextpoint()
    {
        //get next point transform
        Transform nextGoal = points[nextID];
        //flip transform to change sprite
        if(nextGoal.transform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(-.75f, .75f, 1);
            // used to be Vector3(-1, 1, 1)
        }
        else
        {
            transform.localScale = new Vector3(.75f, .75f, 1);
            // used to be Vector3(1, 1, 1)
        }
        //move the enemy towards next waypoint
        transform.position = Vector2.MoveTowards(transform.position, nextGoal.position, speed * Time.deltaTime);
        //check distance between enemy + waypoint, triggers next point.
        if (Vector2.Distance(transform.position, nextGoal.position)<1f)
        {
            //check if enemy is at end of line, make change -1
            if (nextID == points.Count - 1)
            {
                idChangeValue = -1;
            }
            //check if enemy is at start of line, make change + 1
            if (nextID == 0)
            {
                idChangeValue = 1;
            }
            //apply change on nextID
            nextID += idChangeValue;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //deathanim();
        }
    }

    private void deathanim()
    {
        anim.SetTrigger("enemydie");
        Destroy(Waypoints.gameObject);
    }

    public void JumpedOn()
    {
        anim.SetTrigger("Death");
       // explosion.Play();
    }
    private void Die()
    {
        Destroy(this.gameObject);
    }
}