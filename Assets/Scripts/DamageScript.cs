using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScript : MonoBehaviour
{
    public float damage = 2f;
    public float radius = 0.005f;
    public LayerMask layerMask;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, layerMask);

        if(hits.Length > 0)
        {
         hits[0].gameObject.GetComponent<PlayerStats>().applyDamage();
            


          //  gameObject.SetActive(false);
        }
    }
}
