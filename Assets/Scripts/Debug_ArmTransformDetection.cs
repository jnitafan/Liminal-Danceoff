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
        LeftText.text = "LeftH(X,Y,Z,W)\nPos:" + LeftHand.position.x + "," + LeftHand.position.y + "," + LeftHand.position.z + "\nRot:\nX:" + LeftHand.rotation.x + "\nY:" + LeftHand.rotation.y + "\nZ:" + LeftHand.rotation.z + "\nW:" + LeftHand.rotation.w;
        RightText.text = "(X,Y,Z)RightH\n" + LeftHand.position.x + "," + LeftHand.position.y + "," + LeftHand.position.z + ":Pos\n:Rot\n" + LeftHand.rotation.x + ":X\n" + LeftHand.rotation.y + ":Y\n" + LeftHand.rotation.z + ":Z\n" + LeftHand.rotation.w;
    }
}
