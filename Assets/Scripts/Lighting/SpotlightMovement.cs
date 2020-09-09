using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightMovement : MonoBehaviour
{
    float xRotation;
    float yRotation;
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
        xRotation = Mathf.Cos(Time.deltaTime * 5);
        yRotation = Mathf.Sin(Time.deltaTime * 5);
        transform.Rotate(0, xRotation, yRotation, Space.World);
    }
}
