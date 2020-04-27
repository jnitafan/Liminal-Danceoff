using UnityEngine;
using System.Collections;

public class MoveObject : MonoBehaviour
{
    public float speed = 0.5f;
    Vector3 pointA;
    Vector3 pointB;

    void Start()
    {
        pointA = new Vector3(395, -41, 1161);
        pointB = new Vector3(-185, -41, 1047);
    }

    void Update()
    {
        //PingPong between 0 and 1
        float time = Mathf.PingPong(Time.time * speed, 1);
        transform.position = Vector3.Lerp(pointA, pointB, time);
    }
}