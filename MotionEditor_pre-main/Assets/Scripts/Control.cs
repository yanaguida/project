using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.Linq;

public class MotionPartData
{
    public int partNumber;
    public float start;
    public float time;
    public float value;

    public MotionPartData(int partNumber, float start, float time, float value)
    {
        this.partNumber = partNumber;
        this.start = start;
        this.time = time;
        this.value = value;
    }
    public override string ToString()
{
    return $"Part: {partNumber}, Start: {start}, Time: {time}, Value: {value}";
}
}

public class InputDataLoader
{
    public static MotionPartData LoadFromGameObjects(int partNumber)
    {
        float start = 0f;
        float time = 0f;
        float value = 0f;

        GameObject startObjGO = GameObject.Find("Part" + partNumber);
        if (startObjGO != null)
        {
            RectTransform startObj = startObjGO.GetComponent<RectTransform>();
            if (startObj != null){
                start = (startObj.anchoredPosition.x+5500)*0.005f;
            }
        }

        GameObject timeObjGO = GameObject.Find("StretchIcon" + partNumber);
        if (timeObjGO != null)
        {
            RectTransform timeObj = timeObjGO.GetComponent<RectTransform>();
            if (timeObj != null)
                time = timeObj.rect.width * 0.005f;
        }

        GameObject valueObj = GameObject.Find("Input" + partNumber);
        if (valueObj != null)
        {
            var input = valueObj.GetComponent<TMPro.TMP_InputField>();
            if (input != null && float.TryParse(input.text, out float result))
                value = result;
        }

        return new MotionPartData(partNumber, start, time, value);
    }
}

public class Control : MonoBehaviour
{
    public List<MotionPartData>[] laneData;
    private List<int>[] laneLists; 
    public List<int> lane1List = new List<int>();
    public List<int> lane2List = new List<int>();
    public List<int> lane3List = new List<int>();
    public List<int> lane4List = new List<int>();

    private void Awake()
    {
        laneData = new List<MotionPartData>[4];
        for (int i = 0; i < 4; i++)
        {
            laneData[i] = new List<MotionPartData>();
        }
        laneLists = new List<int>[] { lane1List, lane2List, lane3List, lane4List };
    }

    public List<MotionPartData> GetLaneData(int lane){
        if (lane < 0 || lane >= laneData.Length) return null;
        return laneData[lane];
    }

    public void DebugLaneData()
{
    for (int lane = 0; lane < laneData.Length; lane++)
    {
        Debug.Log($"--- Lane {lane + 1} ---");

        if (laneData[lane] == null || laneData[lane].Count == 0)
        {
            Debug.Log("  No data.");
            continue;
        }

        foreach (MotionPartData data in laneData[lane])
        {
            Debug.Log(data.ToString());
        }
    }
}

    public void RefreshPartData(int partNumber)
{
    for (int lane = 0; lane < laneData.Length; lane++)
    {
        for (int i = 0; i < laneData[lane].Count; i++)
        {
            if (laneData[lane][i].partNumber == partNumber)
            {
                MotionPartData updated = InputDataLoader.LoadFromGameObjects(partNumber);
                laneData[lane][i] = updated;
                return;
            }
        }
    }
}

    // 読み込み（GameObjectからMotionPartData生成して保持）
    public void LoadLaneData(int lane)
    {
        if (lane < 0 || lane >= laneData.Length) return;

        laneData[lane].Clear();
        var laneList = laneLists[lane];
        foreach (int partNumber in laneList)
        {
            var data = InputDataLoader.LoadFromGameObjects(partNumber);
            laneData[lane].Add(data);
            laneData[lane] = laneData[lane].OrderBy(d => d.start).ToList();
        }
    }

    public void RefreshLane(int lane){
        if (lane < 0 || lane >= laneData.Length) return;
        List<int> partList = laneLists[lane];
        laneData[lane].Clear();
        foreach (int partNumber in partList){
            MotionPartData updated = InputDataLoader.LoadFromGameObjects(partNumber);
            laneData[lane].Add(updated);
            laneData[lane] = laneData[lane].OrderBy(d => d.start).ToList();
        }
    }

    public void AddToLane(int lane, int num){
        switch (lane)
        {
            case 0: lane1List.Add(num); break;
            case 1: lane2List.Add(num); break;
            case 2: lane3List.Add(num); break;
            case 3: lane4List.Add(num); break;
        }
    }

    public void RemoveFromLane(int lane, int num){
        switch (lane){
            case 0: lane1List.Remove(num); break;
            case 1: lane2List.Remove(num); break;
            case 2: lane3List.Remove(num); break;
            case 3: lane4List.Remove(num); break;
        }
    }

    public bool IsInLane(int lane, int num){
        switch (lane){
            case 0: return Checklean1(num) == 1;
            case 1: return Checklean2(num) == 1;
            case 2: return Checklean3(num) == 1;
            case 3: return Checklean4(num) == 1;
            default: return false;
        }
    }

    public int Checklean1(int num){
        if (lane1List.Contains(num) == true)
            return 1;
        else return 0;
    }
    public int Checklean2(int num){
        if (lane2List.Contains(num) == true)
            return 1;
        else return 0;
    }
    public int Checklean3(int num){
        if (lane3List.Contains(num) == true)
            return 1;
        else return 0;
    }
    public int Checklean4(int num){
        if (lane4List.Contains(num) == true)
            return 1;
        else return 0;
    }
}