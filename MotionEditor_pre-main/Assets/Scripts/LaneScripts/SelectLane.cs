using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class SelectLane : Lanes<SelectPartData>
{
    public LED ledScript;
    public Audio musicScript;
    public stringKind stringkind;

    protected override SelectPartData CreateDataFromChild(Transform child)
    {
        SelectIcon icon = child.GetComponent<SelectIcon>();
        if (icon != null)
            return new SelectPartData(icon.GetStart(), icon.GetTime(), icon.GetEmotion());
        else
        Debug.Log("SelectIconの取得に失敗");
        return null;
    }

    protected override IEnumerator ExecuteAction(SelectPartData data)
    {
        if(stringkind == stringKind.LED)
        yield return StartCoroutine(ledScript.Toggle(data.time, data.emotion));
        else if(stringkind == stringKind.Music)
        yield return StartCoroutine(musicScript.PlayForSeconds(data.time, data.emotion));
        else
        Debug.Log("stringKindが未設定");
    }

    public List<string> ExportData(){
        var lines = new List<string>();
        foreach (var data in LaneData){
            lines.Add($"start:{data.start},time:{data.time},emotion:{data.emotion}");
        }
        return lines;
    }

    protected override float GetStart(SelectPartData data) => data.start;
    protected override float GetTime(SelectPartData data) => data.time;
}
