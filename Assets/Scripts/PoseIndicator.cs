using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseIndicator : MonoBehaviour
{
    public Transform Hand;
    public Transform Pointer;
    private Renderer Renderer;
    private float angle;

    // Start is called before the first frame update
    void Start()
    {
        Renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        angle = 1 - Mathf.InverseLerp(0, 180, Vector3.Angle(Pointer.position - Hand.position, Hand.transform.forward));
        Renderer.material.SetFloat("_percentage", angle);
    }
}
