using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
    public float CurrentAngle = 0f;
    private float subtle=2f;
    public bool isMotorMoving = false;
    public Quaternion initialRotation;

    void Start(){
        GameObject motorGO = this.gameObject;
        motor = motorGO.GetComponent<Transform>();
        initialRotation = motor.rotation;
    }

    public IEnumerator Motor(float desiredTime, float desiredAngle)
{
    if (isMotorMoving || desiredAngle > MaxAngle || desiredAngle < MinAngle)
    {
        yield break;
    }
    isMotorMoving = true;

    float distance = Mathf.Abs(desiredAngle - CurrentAngle);
    if (distance <= subtle)
    {
        yield return new WaitForSeconds(desiredTime);
        isMotorMoving = false;
        yield break;
    }

    // 最大速度（回転速度）を距離/desiredTime で算出
    float maxSpeed = distance / desiredTime;

    // 加速度の値（任意で調整してください）
    float acceleration = maxSpeed * 2f; // 例：maxSpeedを2秒で達成する想定

    float currentSpeed = 0f;

    while (true)
    {
        distance = Mathf.Abs(CurrentAngle - desiredAngle);
        if (distance <= subtle)
            break;

        // 減速開始距離を決める（距離の20%以下で減速開始）
        float decelDistance = distance * 0.2f;

        // 加速・減速ロジック
        if (distance > decelDistance)
        {
            // 加速フェーズ
            currentSpeed += acceleration * Time.deltaTime;
            if (currentSpeed > maxSpeed) currentSpeed = maxSpeed;
        }
        else
        {
            // 減速フェーズ
            currentSpeed -= acceleration * Time.deltaTime;
            if (currentSpeed < subtle) currentSpeed = subtle; // 最低速度を subtle に設定
        }

        float deltaAngle = currentSpeed * Time.deltaTime;

        if (desiredAngle > CurrentAngle)
        {
            MoveMotor(deltaAngle, xyz, direction);
            CurrentAngle += deltaAngle;
        }
        else
        {
            MoveMotor(deltaAngle, xyz, direction * -1);
            CurrentAngle -= deltaAngle;
        }

        yield return null;
    }

    isMotorMoving = false;
}


   /* public IEnumerator Motor(float desiredTime,float desiredAngle){
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
    }*/

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
