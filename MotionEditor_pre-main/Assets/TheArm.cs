using UnityEngine;

public class TheArm : MonoBehaviour
{
  /*ミラガラの羽を上下に動かすためのスクリプト
  Right_Arm,Left_ArmはUnity上で動かしたい部位を指定する必要がある。
  Right_Arm_Angle,Left_Arm_Angle,maxAngle,minAngle←これらで羽の可動域を制限
  isUpで上げ下げの切り替えを制御
  */
    public Transform Right_Arm;
    public Transform Left_Arm;
    public float Right_Arm_Angle = 0f;
    public float Left_Arm_Angle = 0f;
    public float maxAngle = 120f;
    public float rotationSpeed = 30f;
    public float  minAngle = 0f; 
    private float elapsedTime = 0f;     // 経過時間
    private bool isUP=true;

public void Arm()
    {
        float deltaAngle = rotationSpeed * Time.deltaTime;
        //現在の角度と入力された角度を比較、上下方向を設定
        if(Control.instance.Getlean(1)=="1"){
        if(float.Parse(Control.instance.GetValues(1))>Right_Arm_Angle)
        isUP=true;
        else if(float.Parse(Control.instance.GetValues(1))<Right_Arm_Angle)
        isUP=false;
        }
        if(Control.instance.Getlean(1)=="2"){
        if(float.Parse(Control.instance.GetValues(1))>Left_Arm_Angle)
        isUP=true;
        else if(float.Parse(Control.instance.GetValues(1))<Left_Arm_Angle)
        isUP=false;
        }
        //右腕部分
    if (Control.instance.Getlean(1)=="1"&&isUP==true&&float.Parse(Control.instance.GetValues(1))>Right_Arm_Angle){
        Right_Arm.Rotate(Vector3.forward * -deltaAngle);//上方向に動かす
        Right_Arm_Angle += deltaAngle;//アームの可動域の制限
        if(float.Parse(Control.instance.GetValues(1))==Right_Arm_Angle)
       return ;
    }
    
   if (Control.instance.Getlean(1)=="1"&&isUP==false&&float.Parse(Control.instance.GetValues(1))<Right_Arm_Angle){
        Right_Arm.Rotate(Vector3.forward * deltaAngle);//下方向に動かす
        Right_Arm_Angle -= deltaAngle;//アームの可動域の制限
        if(float.Parse(Control.instance.GetValues(1))==Right_Arm_Angle)
        return ;
        }

    
    //左腕部分
    if (Control.instance.Getlean(1)=="2"&&isUP==true&&float.Parse(Control.instance.GetValues(1))>Left_Arm_Angle){
        Left_Arm.Rotate(Vector3.up * deltaAngle);//上方向に動かす
        Left_Arm_Angle += deltaAngle;//アームの可動域の制限
        if(float.Parse(Control.instance.GetValues(1))==Left_Arm_Angle)
    return ;
    }
    
   if (Control.instance.Getlean(1)=="2"&&isUP==false&&float.Parse(Control.instance.GetValues(1))<Left_Arm_Angle){
        Left_Arm.Rotate(Vector3.up * -deltaAngle);//下方向に動かす
        Left_Arm_Angle -= deltaAngle;//アームの可動域の制限
        if(float.Parse(Control.instance.GetValues(1))==Left_Arm_Angle)
    return ;
    
    }
}
}