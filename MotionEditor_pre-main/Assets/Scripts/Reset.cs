using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Reset : MonoBehaviour
{
    public GameObject ResetButton;
    public GameObject RightArmGo;
    public GameObject LeftArmGo;
    private Transform  RightArm;
    private Transform LeftArm;
    public Motors RightMoterScript;
    public Motors LeftMoterScript;

    public void Awake(){
        RightArm =RightArmGo.GetComponent<Transform>();
        LeftArm =LeftArmGo.GetComponent<Transform>();
    }

    public void ResetAdachi(){
        if(RightMoterScript.isMotorMoving ||LeftMoterScript.isMotorMoving){
            Debug.Log("モーターが動いています。");
            return; 
        } 
        RightMoterScript.CurrentAngle = 0f;
        LeftMoterScript.CurrentAngle = 0f;
        RightArm.rotation = RightMoterScript.initialRotation;
        LeftArm.rotation = LeftMoterScript.initialRotation;
    }
}
