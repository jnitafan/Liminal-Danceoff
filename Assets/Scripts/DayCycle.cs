using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycle : MonoBehaviour
{
    public float dayNightCycleDuration = 4.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame, Fixed prefix used for consistency on different machines
    void FixedUpdate()
    {
        // Rotating the sky box for day night cycle
        transform.RotateAround(Vector3.zero,Vector3.right,dayNightCycleDuration*Time.deltaTime);
        transform.LookAt(Vector3.zero);
        
    }
}
