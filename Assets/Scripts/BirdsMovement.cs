using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdsMovement : MonoBehaviour
{
    public float flySpeed = 1.0f;
    public float time = 0.0f;

    public float forwardFlightDuration = 10.0f;
    public float turningFlightDuration = 5.0f;

    // Start is called before the first frame update.
    void Start()
    {

    }

    // FixedUpdate is called once per frame, consistent on all machines.
    void FixedUpdate()
    {
        time += Time.deltaTime;

        // Flying forward before the duration of flying forward is completed.
        if (time < forwardFlightDuration)
        {
            flyForward();
        }
        // Turn around after the forward flying duration is completed.
        if (time > forwardFlightDuration)
        {
            turnAround();
        }
        // After the sequences above are completed, reset time to zero to repeat sequences.
        if (time > (forwardFlightDuration + turningFlightDuration))
        {
            time = 0.0f;
        }
    }

    // Forward flying pattern.
    void flyForward()
    {
        transform.Translate(flySpeed * Vector3.forward * Time.deltaTime);
    }

    // Turning around to the right flying pattern.
    void turnAround()
    {
        transform.Translate((flySpeed * 1.5f) * Vector3.forward * Time.deltaTime);
        transform.Rotate(0.0f, 0.05f * flySpeed, 0.0f);
    }
}
