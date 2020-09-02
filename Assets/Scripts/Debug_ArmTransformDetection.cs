using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_ArmTransformDetection : MonoBehaviour
{
    public Transform LeftHand;
    public Transform RightHand;
    public TextMesh LeftText;
    public TextMesh RightText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        LeftText.text = "LeftH(X,Y,Z)\nRot:\nX:" + (Mathf.Round(LeftHand.eulerAngles.x * 100)) / 100 + "\nY:" + (Mathf.Round(LeftHand.eulerAngles.y * 100)) / 100 + "\nZ:" + (Mathf.Round(LeftHand.eulerAngles.z * 100)) / 100;
        RightText.text = "(X,Y,Z)RightH\n:Rot\n" + (Mathf.Round(RightHand.eulerAngles.x * 100)) / 100 + ":X\n" + (Mathf.Round(RightHand.eulerAngles.y * 100)) / 100 + ":Y\n" + (Mathf.Round(RightHand.eulerAngles.z * 100)) / 100 + ":Z";
    }
}
