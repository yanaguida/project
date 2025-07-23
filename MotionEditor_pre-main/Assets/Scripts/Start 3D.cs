using UnityEngine;

public class Start3D : MonoBehaviour
{
    //このスクリプトはボタンを押すと3Dモデルが動き出すようにするためのものです。
    public TheArm armScript; 
    private int isOn;
    private bool isUP;
    void Start(){
        isOn=1;
    }
    //OnClick関数はUnity上でいろいろいじらないといけないので注意（詳しくはChatGPTに）
    public void OnClick(){
        isOn+=1;
        if(isOn%2==0){
        
        }
    }
    void Update(){
        if(isOn%2==0)
        armScript.Arm();
    }
    /*
    foreach (int lane1List in lane1List)
    {
       armScript.Arm(lane1List);
    }
    */
}
