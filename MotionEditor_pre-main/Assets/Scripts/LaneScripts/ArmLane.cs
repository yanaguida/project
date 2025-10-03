using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class ArmLane : Lanes<ArmData>
{
    public Motors motorScript; 
    public armKind armkind;

    protected override ArmData CreateDataFromChild(Transform child){
        ArmIcon icon = child.GetComponent<ArmIcon>();
        if(icon!=null)
        return new ArmData(icon.GetStart(),icon.GetTime(),icon.GetValue());
        else
        Debug.Log("ArmIconの取得を失敗");
        return null;
    }

    protected override IEnumerator ExecuteAction(ArmData data){
        yield return StartCoroutine(motorScript.Motor(data.time,data.value));
    }

    public List<string> ExportData(){
        var lines = new List<string>();
        foreach (var data in LaneData){
            lines.Add($"start:{data.start},time:{data.time},value:{data.value}");
        }
        return lines;
    }

    protected override float GetStart(ArmData data) => data.start;
    protected override float GetTime(ArmData data) => data.time;
}