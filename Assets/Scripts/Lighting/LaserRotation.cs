using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserRotation : MonoBehaviour
{

    public float xRotate, yRotate, zRotate;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        zRotate = Mathf.Cos(Time.deltaTime * 10);
        transform.Rotate(xRotate, yRotate, zRotate * speed, Space.World);
    }
}
