using UnityEngine;
using System.Collections;

public enum XYZ{
    up,
    forward,
    right
}

public class Motors : MonoBehaviour
{
    private Transform motor;
    public float MaxAngle;
    public float MinAngle;
    public XYZ xyz;
    public int direction;
    private float CurrentAngle = 0f;
    private float subtle=2f;
    private bool isMotorMoving = false;

    void Start(){
        GameObject motorGO = this.gameObject;
        motor = motorGO.GetComponent<Transform>();
    }

    public IEnumerator Motor(float desiredTime,float desiredAngle){
        if (isMotorMoving||desiredAngle>MaxAngle||desiredAngle<MinAngle){
            yield break;
        }
        isMotorMoving = true;
        float distance = Mathf.Abs(desiredAngle - CurrentAngle);
        float rotationSpeed = distance/desiredTime;
        if (distance <= subtle){
            yield return new WaitForSeconds(desiredTime);
            isMotorMoving = false;
            yield break;
        }
        while(true){
            distance = Mathf.Abs(CurrentAngle - desiredAngle);
            if (distance<=subtle)
                break;
            float deltaAngle = rotationSpeed * Time.deltaTime;
            if (desiredAngle>CurrentAngle){
                MoveMotor(deltaAngle,xyz,direction);
                CurrentAngle += deltaAngle;
            }
            else{
                MoveMotor(deltaAngle,xyz,direction*-1);
                CurrentAngle -= deltaAngle;
            }
            yield return null; 
        }
        isMotorMoving = false;
    }

    private void MoveMotor(float deltaAngle,XYZ xyz,int direction){
        if(direction==1||direction==-1){
            if(xyz==XYZ.up){
                motor.Rotate(Vector3.up * deltaAngle * direction);
            }
            if(xyz==XYZ.forward){
                motor.Rotate(Vector3.forward * deltaAngle * direction);
            }
            if(xyz==XYZ.right){
                motor.Rotate(Vector3.right * deltaAngle * direction);
            }
        }
        else
        Debug.Log("directionに1か-1以外の数字が入っています");
    }
}
