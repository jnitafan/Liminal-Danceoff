using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBehaviour : MonoBehaviour
{
    public Animator robotAnimationController;
    public Transform LeftHand;
    public Transform RightHand;
    [Space(10)]
    public bool isON = true;
    public float poseChangeFrequency = 1f;
    public float poseHoldTime = 1f;
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
        robotAnimationController.SetBool("isON", true);

        switch ((int)robotType)
        {
            case 0:
                robotAnimationController.SetBool("poseRecognised", false);
                robotAnimationController.SetBool("isPrimary", true);
                StartCoroutine(copyMyPoses());
                break;
            case 1:
                robotAnimationController.SetBool("isDancing", true);
                StartCoroutine(randomDance());
                break;
            case 2:
                robotAnimationController.SetInteger("Static_Type", (int)robotStaticType);
                robotAnimationController.SetBool("isStationary", true);
                break;
        }

    }

    void Update()
    {

    }

    IEnumerator randomDance()
    {
        while (isON)
        {
            robotAnimationController.SetInteger("Dance_Type", Random.Range(0, 8));
            yield return new WaitForSeconds(poseChangeFrequency);
        }
    }

    IEnumerator copyMyPoses()
    {
        //ArmsCross = 0, Ballet = 1, Boogie = 2, Dab = 3, Smug = 4, Thinking = 5, Thrust = 6, TPose = 7
        int currentPose = 0;//Random.Range(0, 7);
        float timeTakenToPose = 0f;

        while (isON)
        {
            robotAnimationController.SetInteger("Pose_Type", currentPose);
            switch (currentPose)
            {
                case 0:
                    if ((LeftHand.rotation.x > -0.2 && LeftHand.rotation.x < 0) || (LeftHand.rotation.x > -0.7 && LeftHand.rotation.x < -0.3))
                    {
                        //i wish i could just put all of this into one function, but the yield return has to be in an IEnumerator
                        timeTakenToPose = timeTakenToPose + Time.deltaTime;
                        if (timeTakenToPose > poseHoldTime)
                        {
                            robotAnimationController.SetBool("poseRecognised", true);
                            yield return new WaitForSeconds(1.5f);
                            robotAnimationController.SetBool("poseRecognised", false);
                            timeTakenToPose = 0;
                        }
                    };
                    break;
            }
            yield return null;
        }
    }
}