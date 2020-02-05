using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunsetScript : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        //player = new GameObject();
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(Vector3.zero, Vector3.right, 0.5f * Time.deltaTime);
        transform.LookAt(new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z));
    }
}
