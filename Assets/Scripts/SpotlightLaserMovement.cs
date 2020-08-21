using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightLaserMovement : MonoBehaviour
{
    float xRotation;
    float yRotation;
    float zRotation;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RotateAroundPoint();
    }
    void RotateAroundPoint()
    {
        xRotation = Mathf.Cos(Time.deltaTime * 10);
        yRotation = Mathf.Sin(Time.deltaTime * 10);
        transform.Rotate(zRotation, xRotation, yRotation, Space.World);
    }
}

