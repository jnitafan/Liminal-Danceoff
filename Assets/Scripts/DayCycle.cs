using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycle : MonoBehaviour
{
    public float timeOfDayDuration = 1.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // FixedUpdate is called once per frame, consistent on all machines
    void FixedUpdate()
    {
        // Rotating skybox for day night cycle illustration
        transform.RotateAround(Vector3.zero,Vector3.right,timeOfDayDuration*Time.deltaTime);
        transform.LookAt(Vector3.zero);
            
        

    }
}
