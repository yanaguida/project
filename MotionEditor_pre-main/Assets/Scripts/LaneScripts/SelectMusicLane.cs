using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class SelectMusicLane : Lanes<SelectMusicPartData>
{
    public Audio musicScript;  // Audio.cs をアタッチした GameObject

    protected override SelectMusicPartData CreateDataFromChild(Transform child)
    {
        SelectMusicIcon icon = child.GetComponent<SelectMusicIcon>();
        if (icon != null)
            return new SelectMusicPartData(icon.GetStart(), icon.GetTime(), icon.GetMusic());
        else
            Debug.Log("SelectIconの取得に失敗");
        return null;
    }

    protected override IEnumerator ExecuteAction(SelectMusicPartData data)
    {
        // Audio.cs の PlayForSeconds を呼ぶ
       if (data == null)
    {
        Debug.LogError("ExecuteAction に null データが渡された");
        yield break;
    }

    if (musicScript == null)
    {
        Debug.LogError("musicScript が Inspector で設定されていない");
        yield break;
    }

        yield return StartCoroutine(musicScript.PlayForSeconds(data.time, data.music));
    }

    public List<string> ExportData()
    {
        var lines = new List<string>();
        foreach (var data in LaneData)
        {
            lines.Add($"start : {data.start}, time : {data.time}, music : {data.music}");
        }
        return lines;
    }

    protected override float GetStart(SelectMusicPartData data) => data.start;
    protected override float GetTime(SelectMusicPartData data) => data.time;
}
