using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Start3D : MonoBehaviour
{
    /*
    ボタンが押されたら
    エフェクトを流し、テキストを変更
    レーン１～４を同時に実行する。
    */
    public TheArm armScript; 
    public ParticleSystem electricEffect;
    public GameObject StartButton;
    public GameObject StopButton;
    private bool isUP_R;
    private bool isUP_L;

    public void Start(){
        SwitchText(true);
    }

    public void OnClick(){
        SwitchText(false);
        electricEffect.Play();
        for(int i = 0;i<4;i++){
            var dict = Control.instance.LaneDict(i);
            StartCoroutine(ExecuteLane(dict));
        }
    }

    IEnumerator ExecuteLane(Dictionary<int,Control.InputData> dict){
        foreach(var kvp in dict){
            int index = kvp.Key;
            Control.InputData data = kvp.Value;
            if(data.partNumber == 1||data.partNumber == 11||data.partNumber == 12){
                SetisUP_R(data.value);
                yield return StartCoroutine(armScript.RightArm(data.time,data.value,isUP_R));
            }
            if(data.partNumber == 2||data.partNumber == 21||data.partNumber == 22){
                SetisUP_L(data.value);
                yield return StartCoroutine(armScript.LeftArm(data.time,data.value,isUP_L));
            }
        }
    }

    private void SwitchText(bool i){
        StartButton.SetActive(i);
        StopButton.SetActive(!i);
    }

    public void SetisUP_R(float DesiredAngle){
        if(DesiredAngle>armScript.Right_Arm_Angle)
            isUP_R=true;
        else
            isUP_R=false;
    }
    public void SetisUP_L(float DesiredAngle){
        if(DesiredAngle>armScript.Left_Arm_Angle)
            isUP_L=true;
        else
            isUP_L=false;
    }
}