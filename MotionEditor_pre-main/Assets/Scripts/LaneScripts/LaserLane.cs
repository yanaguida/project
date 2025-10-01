using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class LaserLane : Lanes<LaserData>
{
    public Motors motorScript; 
   
    protected override LaserData CreateDataFromChild(Transform child){
        LaserIcon icon = child.GetComponent<LaserIcon>();
        if(icon!=null)
        return new LaserData(icon.GetStart(),icon.GetTime(),icon.GetValue());
        else
        Debug.Log("LaserIconの取得を失敗");
        return null;
    }

    protected override IEnumerator ExecuteAction(LaserData data){
        yield return StartCoroutine(motorScript.Motor(data.time,data.value));
    }

    protected override float GetStart(LaserData data) => data.start;
    protected override float GetTime(LaserData data) => data.time;
}