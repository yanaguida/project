using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class ArmData
{
    public float start;
    public float time;
    public float value;

    public ArmData(float start, float time, float value)
    {
        if(0<start)
        this.start = start;

        if(0<time)
        this.time = time;

        if(0<value&&value<360)
        this.value = value;
    }
}

public enum armKind{Right,Left}

public class ArmLane : Lanes
{
    private armKind armkind;
    private List<ArmData> LaneData = new List<ArmData>();

    public void SetKind(armKind kind){
        this.armkind = kind;
    }

    public override void SetLaneData(){
        LaneData.Clear();
        foreach (Transform child in transform){
            ArmIcon Arm = child.GetComponent<ArmIcon>();
            LaneData.Add(new ArmData(Arm.GetStart(),Arm.GetTime(),Arm.GetValue()));
        }
        LaneData.Sort((a, b) => a.start.CompareTo(b.start));
    }

    public override IEnumerator ExecuteLane()
    {
        float elapsedtime = 0f;
        for (int i = 0; i < LaneData.Count; i++)
        {
            ArmData data = LaneData[i];
            float correctstart = data.start - elapsedtime;
            
            Debug.Log($"Index: {i}, Start: {correctstart}, Time: {data.time},Value: {data.value}");

            yield return StartCoroutine(funScript.Wait(correctstart));

            if(armkind == armKind.Right)
            yield return StartCoroutine(funScript.RightArm(data.time, data.value));

            else if(armkind == armKind.Left)
            yield return StartCoroutine(funScript.LeftArm(data.time, data.value));

            elapsedtime += correctstart+data.time;
        }
    }

    public override float GetTotalTime(){
        float adjustedstart = 0f;
        float elapsedtime = 0f;
        float totaltime = 0f;
        for(int i=0; i<LaneData.Count; i++){
            ArmData data = LaneData[i];
            adjustedstart = data.start - elapsedtime;
            elapsedtime += adjustedstart+data.time;
            totaltime = totaltime+adjustedstart+data.time;
        }
        return totaltime;
    }
}