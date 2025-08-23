using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class RightArmData
{
    public float start;
    public float time;
    public float value;

    public RightArmData(float start, float time, float value)
    {
        this.start = start;
        this.time = time;
        this.value = value;
    }
}

public class Lane1 : Lanes
{
    public Functions funScript; 
    private const float adjustX = 5480;
    private const float distanceRate = 0.005f;
    private List<RightArmData> LaneData = new List<RightArmData>();

    public override void SetLaneData(){
        base.SetLaneData();
        LaneData.Clear();
        foreach (Transform child in transform){
            RectTransform childRect = child.GetComponent<RectTransform>();
            if(childRect != null){
                start = (childRect.anchoredPosition.x+adjustX)*distanceRate;
                time = childRect.rect.width * distanceRate;
            }
             TMP_InputField inputFields = child.GetComponentInChildren<TMP_InputField>();
            if (inputFields != null && float.TryParse(inputFields.text, out float degree)){
                LaneData.Add(new RightArmData(start, time, degree));
            }
        }
    }

    public override IEnumerator ExecuteLane()
    {
        float elapsedtime = 0f;
        for (int i = 0; i < LaneData.Count; i++)
        {
            RightArmData data = LaneData[i];
            float correctstart = data.start - elapsedtime;
            Debug.Log($"Index: {i}, Start: {correctstart}, Time: {data.time},Value: {data.value}");
            yield return StartCoroutine(funScript.Wait(correctstart));
            yield return StartCoroutine(funScript.RightArm(data.time, data.value));
            elapsedtime += correctstart+data.time;
        }
    }

    public override float GetTotalTime(){
        float adjustedstart = 0f;
        float elapsedtime = 0f;
        float totaltime = 0f;
        for(int i=0; i<LaneData.Count; i++){
            RightArmData data = LaneData[i];
            adjustedstart = data.start - elapsedtime;
            elapsedtime += adjustedstart+data.time;
            totaltime = totaltime+adjustedstart+data.time;
        }
        return totaltime;
    }
}