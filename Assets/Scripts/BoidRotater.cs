using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidRotater : MonoBehaviour
{
    private float rotation;
    // Start is called before the first frame update
    public GameObject boidRotatingDevice;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        rotation = (rotation + 0.1f) + Time.deltaTime % 360f;
        boidRotatingDevice.transform.localRotation = Quaternion.Euler(new Vector3(0, rotation, 0));
    }
}
