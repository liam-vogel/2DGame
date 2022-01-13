using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScript : MonoBehaviour
{
    public float damage = 20f;
    public float radius = 0.005f;
    public LayerMask layerMask;
    Collider2D coll;


    // Start is called before the first frame update
    void Start()
    {
       coll = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, layerMask);

        if(hits.Length > 0)
        {
         hits[0].gameObject.GetComponent<PlayerStats>().applyDamage(damage);
            


            gameObject.SetActive(false);
        }
    }
}
