using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdsMovement : MonoBehaviour
{
    public float flySpeed = 1.0f;
    public float time = 0.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (time<10)
        {
            flyForward();
            time = time + Time.deltaTime;
        }
        if (time>10)
        {
            turnAround();
            time = time + Time.deltaTime;
        }
        if (time>15)
        {
            time = 0.0f;
        }
    }

    void flyForward()
    {
        transform.Translate(flySpeed * Vector3.forward * Time.deltaTime);

    }

    void turnAround()
    {
        transform.Translate((flySpeed*1.5f) * Vector3.forward * Time.deltaTime);
        transform.Rotate(0.0f, 0.05f*flySpeed, 0.0f);

    }
}
