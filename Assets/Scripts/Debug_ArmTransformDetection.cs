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
        LeftText.text = "LeftH(X,Y,Z,W)\nRot:\nX:" + (Mathf.Round(LeftHand.rotation.x * 100)) / 100 + "\nY:" + (Mathf.Round(LeftHand.rotation.y * 100)) / 100 + "\nZ:" + (Mathf.Round(LeftHand.rotation.z * 100)) / 100 + "\nW:" + (Mathf.Round(LeftHand.rotation.w * 100)) / 100;
        RightText.text = "(X,Y,Z)RightH\n:Rot\n" + (Mathf.Round(RightHand.rotation.x * 100)) / 100 + ":X\n" + (Mathf.Round(RightHand.rotation.y * 100)) / 100 + ":Y\n" + (Mathf.Round(RightHand.rotation.z * 100)) / 100 + ":Z\n" + (Mathf.Round(RightHand.rotation.w * 100)) / 100;
    }
}
