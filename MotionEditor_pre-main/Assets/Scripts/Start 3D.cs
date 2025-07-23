using UnityEngine;
using System.Collections;

public class Start3D : MonoBehaviour
{
    /*
    ボタンが押されたら
    エフェクトを流す
    レーンの長さを測定
    行ごとに区切られたlaneListを列で区切り再構成
    モーション番号と値をdictionary型変数で対応付ける（Control.csに書いた）
    同列内にあるモーションは同時に実行
    終わったら次の列へ（繰り返し処理）
    */
    public TheArm armScript; 
    public ParticleSystem electricEffect;
    public bool isUP_R;
    public bool isUP_L;
    private int isOn=1;
    private float desiredAngle;
    private float desiredTime;

    //OnClick関数はUnity上でいろいろいじらないといけないので注意（詳しくはChatGPTに）
    public void OnClick(){
        isOn++;
        if (isOn % 2 == 0){
            electricEffect.Play();
            int a = Control.instance.lane1List.Count;
            int b = Control.instance.lane2List.Count;
            int c = Control.instance.lane3List.Count;
            int d = Control.instance.lane4List.Count;
            int len =  Mathf.Max(a, b, c, d);
            // コルーチンで時間ごとに処理
            StartCoroutine(ExecuteLaneSequence(len));
        }
    }

    IEnumerator ExecuteLaneSequence(int totalColumns){
        for (int i = 0; i < totalColumns; i++){
            var dict = Control.instance.GetColumnLaneValueDict(i);
            if (dict.ContainsKey(1)){
                SetisUP_R(dict[1]);
                desiredAngle = dict[1];
                StartCoroutine(armScript.RightArm(isUP_R,desiredAngle));
            }
            if (dict.ContainsKey(11)){
                SetisUP_R(dict[11]);
                desiredAngle = dict[11];
                StartCoroutine(armScript.RightArm(isUP_R,desiredAngle));
            }
            if (dict.ContainsKey(12)){
                SetisUP_R(dict[12]);
                desiredAngle = dict[12];
                StartCoroutine(armScript.RightArm(isUP_R,desiredAngle));
            }
            if (dict.ContainsKey(2)){
                SetisUP_L(dict[2]);
                desiredAngle = dict[2];
                StartCoroutine(armScript.LeftArm(isUP_L,desiredAngle));
            }
            if (dict.ContainsKey(21)){
                SetisUP_L(dict[21]);
                desiredAngle = dict[21];
                StartCoroutine(armScript.LeftArm(isUP_L,desiredAngle));
            }
            if (dict.ContainsKey(22)){
                SetisUP_L(dict[22]);
                desiredAngle = dict[22];
                StartCoroutine(armScript.LeftArm(isUP_L,desiredAngle));
            }
            if(dict.ContainsKey(4)){
                desiredTime = dict[4];
                StartCoroutine(armScript.Empty(desiredTime));
            }
            if(dict.ContainsKey(41)){
                desiredTime = dict[41];
                StartCoroutine(armScript.Empty(desiredTime));
            }
            if(dict.ContainsKey(42)){
                desiredTime = dict[42];
                StartCoroutine(armScript.Empty(desiredTime));
            }
        yield return new WaitForSeconds(5f); // 注意５秒以内に処理が終わらなったら次の列の処理が実行されない
        }
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