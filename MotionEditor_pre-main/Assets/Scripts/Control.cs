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
            if (startObj != null)
                start = (startObj.anchoredPosition.x+5500)*0.005f;
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

    public List<MotionPartData> GetLaneData(int laneIndex){
        if (laneIndex < 0 || laneIndex >= laneData.Length) return null;
        return laneData[laneIndex];
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

    // 書き込み例
    public void UpdateInputData(int lane, int index, float newTime, float newValue)
    {
        if (lane < 0 || lane >= laneData.Length) return;
        if (index < 0 || index >= laneData[lane].Count) return;

        var data = laneData[lane][index];
        data.time = newTime;   // これもsetter化が望ましいですが一旦publicで
        data.value = newValue;
    }

    // 読み取り例
    public MotionPartData GetInputData(int lane, int index)
    {
        if (lane < 0 || lane >= laneData.Length) return null;
        if (index < 0 || index >= laneData[lane].Count) return null;
        return laneData[lane][index];
    }

    public void MovePartToLane(int targetLane, int partNumber)
{
    if (targetLane < 0 || targetLane >= laneLists.Length) return;

    // 他のレーンから削除
    for (int i = 0; i < laneLists.Length; i++)
    {
        if (i == targetLane) continue;
        laneLists[i].Remove(partNumber);
    }

    // すでに同じレーンに含まれていなければ追加
    if (!laneLists[targetLane].Contains(partNumber))
    {
        laneLists[targetLane].Add(partNumber);
    }

    // データを再読み込み（必要であれば）
    LoadLaneData(targetLane);
}

    public void AddToLane(int lane, int num){
        switch (lane)
        {
            case 0: Addlean1(num); break;
            case 1: Addlean2(num); break;
            case 2: Addlean3(num); break;
            case 3: Addlean4(num); break;
        }
    }

    public void Addlean1(int num){
        lane1List.Add(num);
    }
    public void Addlean2(int num){
        lane2List.Add(num);
    }
    public void Addlean3(int num){
        lane3List.Add(num);
    }
    public void Addlean4(int num){
        lane4List.Add(num);
    }

    public  void RemoveFromOtherLanes(int lane,int num)
    {
        for (int i = 0; i < 4; i++)
        {
            if (i == lane) continue;
            if (IsInLane(i, num))
            {
                RemoveFromLane(i, num);
            }
        }
    }

    public void RemoveFromLane(int lane, int num){
        switch (lane){
            case 0: Removelean1(num); break;
            case 1: Removelean2(num); break;
            case 2: Removelean3(num); break;
            case 3: Removelean4(num); break;
        }
    }

    public void Removelean1(int num){
        lane1List.Remove(num);
    }
    public void Removelean2(int num){
        lane2List.Remove(num);
    }
    public void Removelean3(int num){
        lane3List.Remove(num);
    }
    public void Removelean4(int num){
        lane4List.Remove(num);
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