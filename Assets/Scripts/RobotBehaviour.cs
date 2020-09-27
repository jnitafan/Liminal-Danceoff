using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBehaviour : MonoBehaviour
{
    public Animator robotAnimationController;
    public Transform playerHead;
    public Transform playerLeftHand;
    public Transform playerRightHand;
    public Transform leftPointer;
    public Transform rightPointer;
    public GameObject LightManager;
    private Lighting LightManagerScript;
    private Renderer robotFaceRenderer;
    [Space(10)]
    [SerializeField]
    private bool isON = false;
    private bool runOnce = false;
    public float poseChangeFrequency = 1f;
    public float poseHoldTime = 1f;
    public float poseAngleTolerance = 30f;
    public float PosesCompleted = 0f;
    public float PosesForGlow = 10f;
    public GameObject[] Glowsticks;
    public enum robotTypeEnum : int
    {
        Primary = 0,
        Dancing = 1,
        Stationary = 2
    }
    public enum robotStaticTypeEnum : int
    {
        Sitting,
        Lean,
        DJScratch,
        Lying,
        Headspin,
        Lean_2
    }
    [Space(10)]
    public robotTypeEnum robotType;
    public robotStaticTypeEnum robotStaticType;

    void Start()
    {
        LightManagerScript = LightManager.GetComponent<Lighting>();
        //get the face renderer
        robotFaceRenderer = this.transform.GetChild(1).GetComponent<Renderer>();

        robotAnimationController = this.GetComponent<Animator>();

        robotFaceRenderer.material.SetFloat("_expressionNumber", 0);


    }

    void Update()
    {
        isON = LightManagerScript.isON;

        if (isON && !runOnce)
        {
            runOnce = true;

            robotAnimationController.SetBool("isON", true);

            switch ((int)robotType)
            {
                case 0:
                    //if the robot is set as the primary robot
                    //preset the variables
                    robotAnimationController.SetBool("poseRecognised", false);
                    robotAnimationController.SetBool("isPrimary", true);
                    robotFaceRenderer.material.SetFloat("_randomExpression", 0);

                    //set the pose pointing positions to the default values
                    leftPointer.position = playerHead.position;
                    rightPointer.position = playerHead.position;

                    //start doing poses
                    StartCoroutine(copyMyPoses());
                    break;
                case 1:
                    //if the robot is set to dance mode
                    robotAnimationController.SetBool("isDancing", true);
                    //start doing some random dances
                    StartCoroutine(randomDance());
                    //StartCoroutine(randomFace());
                    break;
                case 2:
                    //if the robot is set to a static position
                    robotAnimationController.SetInteger("Static_Type", (int)robotStaticType);
                    robotFaceRenderer.material.SetFloat("_expressionNumber", Random.Range(1, 62));
                    robotAnimationController.SetBool("isStationary", true);
                    StartCoroutine(randomStatic());
                    break;
            }
        }

    }

    IEnumerator randomDance()
    {
        while (isON)
        {
            robotAnimationController.SetInteger("Dance_Type", Random.Range(0, 8));
            robotFaceRenderer.material.SetFloat("_expressionNumber", Random.Range(1, 62));
            yield return new WaitForSeconds(poseChangeFrequency * 3);
        }
    }

    IEnumerator randomStatic()
    {
        while (isON)
        {
            robotFaceRenderer.material.SetFloat("_expressionNumber", Random.Range(1, 62));
            yield return new WaitForSeconds(poseChangeFrequency * 3);
        }
    }


    IEnumerator copyMyPoses()
    {
        //ArmsCross = 0, Ballet = 1, Boogie = 2, Dab = 3, Smug = 4, Thinking = 5, Thrust = 6, TPose = 7
        int currentPose = Random.Range(0, 7); //Random.Range(0, 7);
        float timePosedCounter = 0f;

        while (isON)
        {
            //set the animation pose type to the current pose playing
            robotAnimationController.SetInteger("Pose_Type", currentPose);
            robotFaceRenderer.material.SetFloat("_expressionNumber", 1);

            switch (currentPose)
            {
                //I would love to put this inside a function, but i dont know how to pass coroutine return info lol
                case 0:

                    positionPointers(0, 0, 180, 180);

                    if (Vector3.Angle(leftPointer.position - playerLeftHand.position, playerLeftHand.transform.forward) < poseAngleTolerance && Vector3.Angle(rightPointer.position - playerRightHand.position, playerRightHand.transform.forward) < poseAngleTolerance)
                    {
                        timePosedCounter = timePosedCounter + Time.deltaTime;
                        if (timePosedCounter > poseHoldTime)
                        {
                            robotAnimationController.SetBool("poseRecognised", true);
                            robotFaceRenderer.material.SetFloat("_expressionNumber", 2);
                            yield return new WaitForSeconds(1.5f);
                            robotAnimationController.SetBool("poseRecognised", false);
                            currentPose = Random.Range(0, 7);
                            robotAnimationController.SetInteger("Pose_Type", currentPose);
                            timePosedCounter = 0;
                            PosesCompleted++;
                        }
                    }
                    break;

                case 1:

                    positionPointers(-1, 1, -15, -15);

                    if (Vector3.Angle(leftPointer.position - playerLeftHand.position, playerLeftHand.transform.forward) < poseAngleTolerance && Vector3.Angle(rightPointer.position - playerRightHand.position, playerRightHand.transform.forward) < poseAngleTolerance)
                    {
                        timePosedCounter = timePosedCounter + Time.deltaTime;
                        if (timePosedCounter > poseHoldTime)
                        {
                            robotAnimationController.SetBool("poseRecognised", true);
                            robotFaceRenderer.material.SetFloat("_expressionNumber", 2);
                            yield return new WaitForSeconds(1.5f);
                            robotAnimationController.SetBool("poseRecognised", false);
                            currentPose = Random.Range(0, 7);
                            robotAnimationController.SetInteger("Pose_Type", currentPose);
                            timePosedCounter = 0;
                            PosesCompleted++;
                        }
                    }
                    break;

                case 2:

                    positionPointers(1.5f, -1, 35, 55);

                    if (Vector3.Angle(leftPointer.position - playerLeftHand.position, playerLeftHand.transform.forward) < poseAngleTolerance && Vector3.Angle(rightPointer.position - playerRightHand.position, playerRightHand.transform.forward) < poseAngleTolerance)
                    {
                        timePosedCounter = timePosedCounter + Time.deltaTime;
                        if (timePosedCounter > poseHoldTime)
                        {
                            robotAnimationController.SetBool("poseRecognised", true);
                            robotFaceRenderer.material.SetFloat("_expressionNumber", 2);
                            yield return new WaitForSeconds(1.5f);
                            robotAnimationController.SetBool("poseRecognised", false);
                            currentPose = Random.Range(0, 7);
                            robotAnimationController.SetInteger("Pose_Type", currentPose);
                            timePosedCounter = 0;
                            PosesCompleted++;
                        }
                    }
                    break;

                case 3:

                    positionPointers(0.5f, 0, 0, 180);

                    if (Vector3.Angle(leftPointer.position - playerLeftHand.position, playerLeftHand.transform.forward) < poseAngleTolerance && Vector3.Angle(rightPointer.position - playerRightHand.position, playerRightHand.transform.forward) < poseAngleTolerance)
                    {
                        timePosedCounter = timePosedCounter + Time.deltaTime;
                        if (timePosedCounter > poseHoldTime)
                        {
                            robotAnimationController.SetBool("poseRecognised", true);
                            robotFaceRenderer.material.SetFloat("_expressionNumber", 2);
                            yield return new WaitForSeconds(1.5f);
                            robotAnimationController.SetBool("poseRecognised", false);
                            currentPose = Random.Range(0, 7);
                            robotAnimationController.SetInteger("Pose_Type", currentPose);
                            timePosedCounter = 0;
                            PosesCompleted++;
                        }
                    }
                    break;

                case 4:

                    positionPointers(0, 0, 180, -135);

                    if (Vector3.Angle(leftPointer.position - playerLeftHand.position, playerLeftHand.transform.forward) < poseAngleTolerance && Vector3.Angle(rightPointer.position - playerRightHand.position, playerRightHand.transform.forward) < poseAngleTolerance)
                    {
                        timePosedCounter = timePosedCounter + Time.deltaTime;
                        if (timePosedCounter > poseHoldTime)
                        {
                            robotAnimationController.SetBool("poseRecognised", true);
                            robotFaceRenderer.material.SetFloat("_expressionNumber", 2);
                            yield return new WaitForSeconds(1.5f);
                            robotAnimationController.SetBool("poseRecognised", false);
                            currentPose = Random.Range(0, 7);
                            robotAnimationController.SetInteger("Pose_Type", currentPose);
                            timePosedCounter = 0;
                            PosesCompleted++;
                        }
                    }
                    break;

                case 5:

                    positionPointers(0, -100, 180, 0);

                    if (Vector3.Angle(leftPointer.position - playerLeftHand.position, playerLeftHand.transform.forward) < poseAngleTolerance && Vector3.Angle(rightPointer.position - playerRightHand.position, playerRightHand.transform.forward) < poseAngleTolerance)
                    {
                        timePosedCounter = timePosedCounter + Time.deltaTime;
                        if (timePosedCounter > poseHoldTime)
                        {
                            robotAnimationController.SetBool("poseRecognised", true);
                            robotFaceRenderer.material.SetFloat("_expressionNumber", 2);
                            yield return new WaitForSeconds(1.5f);
                            robotAnimationController.SetBool("poseRecognised", false);
                            currentPose = Random.Range(0, 7);
                            robotAnimationController.SetInteger("Pose_Type", currentPose);
                            timePosedCounter = 0;
                            PosesCompleted++;
                        }
                    }
                    break;

                case 6:

                    positionPointers(0, 0, -85, 85);

                    if (Vector3.Angle(leftPointer.position - playerLeftHand.position, playerLeftHand.transform.forward) < poseAngleTolerance && Vector3.Angle(rightPointer.position - playerRightHand.position, playerRightHand.transform.forward) < poseAngleTolerance)
                    {
                        timePosedCounter = timePosedCounter + Time.deltaTime;
                        if (timePosedCounter > poseHoldTime)
                        {
                            robotAnimationController.SetBool("poseRecognised", true);
                            robotFaceRenderer.material.SetFloat("_expressionNumber", 2);
                            yield return new WaitForSeconds(1.5f);
                            robotAnimationController.SetBool("poseRecognised", false);
                            currentPose = Random.Range(0, 7);
                            robotAnimationController.SetInteger("Pose_Type", currentPose);
                            timePosedCounter = 0;
                            PosesCompleted++;
                        }
                    }
                    break;

                case 7:

                    positionPointers(0, 0, 0, 0);

                    if (Vector3.Angle(leftPointer.position - playerLeftHand.position, playerLeftHand.transform.forward) < poseAngleTolerance && Vector3.Angle(rightPointer.position - playerRightHand.position, playerRightHand.transform.forward) < poseAngleTolerance)
                    {
                        timePosedCounter = timePosedCounter + Time.deltaTime;
                        if (timePosedCounter > poseHoldTime)
                        {
                            robotAnimationController.SetBool("poseRecognised", true);
                            robotFaceRenderer.material.SetFloat("_expressionNumber", 2);
                            yield return new WaitForSeconds(1.5f);
                            robotAnimationController.SetBool("poseRecognised", false);
                            currentPose = Random.Range(0, 7);
                            robotAnimationController.SetInteger("Pose_Type", currentPose);
                            timePosedCounter = 0;
                            PosesCompleted++;
                        }
                    }
                    break;
            }

            if (PosesCompleted > PosesForGlow)
            {

                Glowsticks[0].SetActive(true);
                Glowsticks[1].SetActive(true);
                Glowsticks[2].SetActive(true);
                Glowsticks[3].SetActive(true);
            }


            //Debug.Log(Vector3.Angle(leftPointer.position - playerLeftHand.position, playerLeftHand.transform.forward) + " " + timePosedCounter);
            //Debug.Log(Mathf.Sin(Mathf.Rad2Deg * ((playerHead.localEulerAngles.y % 90) - 90)));
            yield return null;
        }
    }
    // height offset = arm up and down in meters, angle offset = change in angle in degrees °
    void positionPointers(float leftHandHeightOffset, float rightHandHeightOffset, float leftHandAngleOffset, float rightHandAngleOffset)
    {
        rightPointer.position = new Vector3
        (
            (playerHead.position.x + Mathf.Sin(Mathf.Deg2Rad * (playerHead.localEulerAngles.y + 90 + rightHandAngleOffset)) * 3),
            (playerHead.position.y + rightHandHeightOffset),
            (playerHead.position.z + Mathf.Cos(Mathf.Deg2Rad * (playerHead.localEulerAngles.y + 90 + rightHandAngleOffset)) * 3)
        );
        leftPointer.transform.position = new Vector3
        (
            (playerHead.position.x + Mathf.Sin(Mathf.Deg2Rad * (playerHead.localEulerAngles.y - 90 + leftHandAngleOffset)) * 3),
            (playerHead.position.y + leftHandHeightOffset),
            (playerHead.position.z + Mathf.Cos(Mathf.Deg2Rad * (playerHead.localEulerAngles.y - 90 + leftHandAngleOffset)) * 3)
        );

        //move the pose pointers 1m perpendicularly to the head's position

    }
}