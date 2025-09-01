using UnityEngine;
using System.Collections;

public class Functions : MonoBehaviour
{
    public Transform Right_Arm;
    public Transform Left_Arm;
    public float MaxAngle;
    public float MinAngle;
    private float Right_Arm_Angle = 0f;
    private float Left_Arm_Angle = 0f;
    private float subtle=2f;
    private bool isRightArmMoving = false;
    private bool isLeftArmMoving = false;

    public IEnumerator RightArm(float desiredTime,float desiredAngle){
        if (isRightArmMoving||desiredAngle>MaxAngle||desiredAngle<MinAngle){
            yield break;
        }
        isRightArmMoving = true;
        float distance = Mathf.Abs(desiredAngle - Right_Arm_Angle);
        float rotationSpeed = distance/desiredTime;
        if (distance <= subtle){
            yield return StartCoroutine(Wait(desiredTime));
            isRightArmMoving = false;
            yield break;
        }
        while(true){
            float absValue = Mathf.Abs(Right_Arm_Angle - desiredAngle);
            if (absValue<=subtle)
                break;
            float deltaAngle = rotationSpeed * Time.deltaTime;
            if (desiredAngle>Right_Arm_Angle){
                Right_Arm.Rotate(Vector3.forward * -deltaAngle);
                Right_Arm_Angle += deltaAngle;
            }
            else{
                Right_Arm.Rotate(Vector3.forward * deltaAngle);
                Right_Arm_Angle -= deltaAngle;
            }
            yield return null; 
        }
        isRightArmMoving = false;
    }

    public IEnumerator LeftArm(float desiredtime,float desiredAngle){
        if (isLeftArmMoving||desiredAngle>MaxAngle||desiredAngle<MinAngle){
            yield break;
        }
        isLeftArmMoving = true;
        float distance = Mathf.Abs(desiredAngle - Left_Arm_Angle);
        float rotationSpeed = distance/desiredtime;
        while(true){
            float absValue = Mathf.Abs(Left_Arm_Angle - desiredAngle);
            if (absValue<subtle)
                break;
            float deltaAngle = rotationSpeed * Time.deltaTime;
            if (desiredAngle>Left_Arm_Angle){
                Left_Arm.Rotate(Vector3.up * deltaAngle);
                Left_Arm_Angle += deltaAngle;
            }
            else{
                Left_Arm.Rotate(Vector3.up * -deltaAngle);
                Left_Arm_Angle -= deltaAngle;
            }
            yield return null;
        }
        isLeftArmMoving = false;
    }

    public IEnumerator Wait(float desiredTime){
        yield return new WaitForSeconds(desiredTime);
    }
}