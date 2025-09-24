using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class SelectLane : Lanes<SelectPartData>
{
    public LED ledScript;

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
        yield return StartCoroutine(ledScript.Toggle(data.time, data.emotion));
    }

    protected override float GetStart(SelectPartData data) => data.start;
    protected override float GetTime(SelectPartData data) => data.time;
}
