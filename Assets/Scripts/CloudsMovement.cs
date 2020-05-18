using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsMovement : MonoBehaviour
{
    public float windSpeed = 1.0f; 
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(0.0f, windSpeed, 0.0f);
    }
}
