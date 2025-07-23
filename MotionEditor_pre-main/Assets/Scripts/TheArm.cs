using UnityEngine;
using System.Collections;

public class TheArm : MonoBehaviour
{
  /*ミラガラの羽を上下に動かすためのスクリプト
  コルーチン型の関数RightArmとLeftArm
  第一引数で上か下か、第二引数で、どこまで上げるか
  bool型関数StopMotionで無限ループから抜ける
  */
    public Transform Right_Arm;
    public Transform Left_Arm;
    public float Right_Arm_Angle = 0f;
    public float Left_Arm_Angle = 0f;
    private float rotationSpeed = 30f;
    private float subtle=1.5f;
    private bool isRightArmMoving = false;
    private bool isLeftArmMoving = false;

    public IEnumerator RightArm(bool isUP,float desiredAngle){
        if (isRightArmMoving){
            yield break;
        }
        isRightArmMoving = true;
        while(true){
            if (!StopMotionR(desiredAngle))
                break;
        float deltaAngle = rotationSpeed * Time.deltaTime;
        if (isUP){
            Right_Arm.Rotate(Vector3.forward * -deltaAngle);//上方向に動かす
            Right_Arm_Angle += deltaAngle;//アームの可動域の制限
        }
        else{
            Right_Arm.Rotate(Vector3.forward * deltaAngle);//下方向に動かす
            Right_Arm_Angle -= deltaAngle;//アームの可動域の制限
        }
        yield return null;
        }
        isRightArmMoving = false;
    }

    public IEnumerator LeftArm(bool isUP,float desiredAngle){
        if (isLeftArmMoving){
            yield break;
        }
        isLeftArmMoving = true;
        while(true){
            if (!StopMotionL(desiredAngle))
                break;
        float deltaAngle = rotationSpeed * Time.deltaTime;
        if (isUP){
            Left_Arm.Rotate(Vector3.up * deltaAngle);//上方向に動かす
            Left_Arm_Angle += deltaAngle;//アームの可動域の制限
        }
        else{
            Left_Arm.Rotate(Vector3.up * -deltaAngle);//下方向に動かす
            Left_Arm_Angle -= deltaAngle;//アームの可動域の制限
        }
        yield return null;
        }
        isLeftArmMoving = false;
    }

    public IEnumerator Empty(float desiredTime){
        yield return new WaitForSeconds(desiredTime);
    }

    public bool StopMotionR(float desiredAngle){
        float absValue = Mathf.Abs(Right_Arm_Angle - desiredAngle);
        return absValue > subtle;

    }

    public bool StopMotionL(float desiredAngle){
        float absValue = Mathf.Abs(Left_Arm_Angle - desiredAngle);
        return absValue > subtle;
    }
}