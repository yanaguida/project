using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class SelectPartData
{
    public float start;
    public float time;
    public string emotion;

    public SelectPartData(float start, float time, string emotion){
        if(0<start)
        this.start = start;
        if(0<time)
        this.time = time;
        this.emotion = emotion;
    }
}

public class SelectLane : Lanes
{
    public SwitchOn switchScript;
    private List<SelectPartData> LaneData = new List<SelectPartData>(); 

    public override void SetLaneData(){
        LaneData.Clear();
        foreach (Transform child in transform){
            SelectIcon Icon = child.GetComponent<SelectIcon>();
            LaneData.Add(new SelectPartData(Icon.GetStart(),Icon.GetTime(),Icon.GetEmotion()));
        }
        LaneData.Sort((a, b) => a.start.CompareTo(b.start));
    }

    public override IEnumerator ExecuteLane()
    {
        float elapsedtime = 0f;
        for (int i = 0; i < LaneData.Count; i++)
        {
            SelectPartData data = LaneData[i];
            float correctstart = data.start - elapsedtime;
            
            Debug.Log($"Index: {i}, Start: {correctstart}, Time: {data.time},Value: {data.emotion}");

            yield return new WaitForSeconds(correctstart);

            yield return StartCoroutine(switchScript.Toggle(data.time, data.emotion));

            elapsedtime += correctstart+data.time;
        }
    }

    public override float GetTotalTime(){
        float adjustedstart = 0f;
        float elapsedtime = 0f;
        float totaltime = 0f;
        for(int i=0; i<LaneData.Count; i++){
            SelectPartData data = LaneData[i];
            adjustedstart = data.start - elapsedtime;
            elapsedtime += adjustedstart+data.time;
            totaltime = totaltime+adjustedstart+data.time;
        }
        return totaltime;
    }
}
