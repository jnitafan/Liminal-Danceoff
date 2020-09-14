﻿using System.Collections;
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
        int currentPose = 7;//Random.Range(0, 7);
        float timePosedCounter = 0f;

        while (isON)
        {
            //set the animation pose type to the current pose playing
            robotAnimationController.SetInteger("Pose_Type", currentPose);
            robotFaceRenderer.material.SetFloat("_expressionNumber", 1);

            switch (currentPose)
            {
                case 7:

                    positionPointers(0, 0, 0, 0, 0, 0);

                    if (Vector3.Angle(leftPointer.position - playerLeftHand.position, playerLeftHand.transform.forward) < poseAngleTolerance && Vector3.Angle(rightPointer.position - playerRightHand.position, playerRightHand.transform.forward) < poseAngleTolerance)
                    {
                        timePosedCounter = timePosedCounter + Time.deltaTime;
                        if (timePosedCounter > poseHoldTime)
                        {
                            robotAnimationController.SetBool("poseRecognised", true);
                            robotFaceRenderer.material.SetFloat("_expressionNumber", 2);
                            yield return new WaitForSeconds(1.5f);
                            robotAnimationController.SetBool("poseRecognised", false);
                            timePosedCounter = 0;
                        }
                    }
                    break;
            }

            //Debug.Log(Vector3.Angle(leftPointer.position - playerLeftHand.position, playerLeftHand.transform.forward) + " " + timePosedCounter);
            //Debug.Log(Mathf.Sin(Mathf.Rad2Deg * ((playerHead.localEulerAngles.y % 90) - 90)));
            yield return null;
        }
    }

    void positionPointers(float xRightOffset, float yRightOffset, float zRightOffset, float xLeftOffset, float yLeftOffset, float zLeftOffset)
    {
        //move the pose pointers 1m perpendicularly to the head's position
        rightPointer.position = new Vector3
        (
            playerHead.position.x - Mathf.Sin(Mathf.Deg2Rad * (playerHead.localEulerAngles.y - 90)) * 10 + xRightOffset,
            playerHead.position.y + yRightOffset,
            playerHead.position.z - Mathf.Cos(Mathf.Deg2Rad * (playerHead.localEulerAngles.y - 90)) * 10 + zRightOffset
        );
        leftPointer.transform.position = new Vector3
        (
            playerHead.position.x + Mathf.Sin(Mathf.Deg2Rad * (playerHead.localEulerAngles.y - 90)) * 10 + xLeftOffset,
            playerHead.position.y + yLeftOffset,
            playerHead.position.z + Mathf.Cos(Mathf.Deg2Rad * (playerHead.localEulerAngles.y - 90)) * 10 + zLeftOffset
        );
    }
}