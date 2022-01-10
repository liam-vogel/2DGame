using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallThroughPlatform : MonoBehaviour
{
    private PlatformEffector2D effector;
    public float wait;

    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
    }

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.DownArrow))
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


    }
}
