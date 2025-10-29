using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface ILane
{
    void ImportData(List<string> lines);
    void SetLaneData();
    IEnumerator ExecuteLane();
    float GetTotalTime();
    void DeleteAllChild();
}


public abstract class Lanes<TData> : MonoBehaviour , ILane
{
    protected List<TData> LaneData = new List<TData>();

    protected abstract TData CreateDataFromChild(Transform child);
    protected abstract IEnumerator ExecuteAction(TData data);
    protected abstract void CreateChildFromData();
    public virtual void ImportData(List<string> lines){}

    public virtual void SetLaneData()
    {
        LaneData.Clear();
        foreach (Transform child in transform)
        {
            TData data = CreateDataFromChild(child);
            if (data != null)
                LaneData.Add(data);
        }
        LaneData.Sort((a, b) => GetStart(a).CompareTo(GetStart(b)));
    }

    public virtual IEnumerator ExecuteLane()
    {
        float elapsedTime = 0f;
        for (int i = 0; i < LaneData.Count; i++)
        {
            TData data = LaneData[i];
            float correctStart = GetStart(data) - elapsedTime;

            yield return new WaitForSeconds(correctStart);
            yield return ExecuteAction(data);

            elapsedTime += correctStart + GetTime(data);
        }
    }

    public virtual float GetTotalTime()
    {
        float adjustedStart = 0f;
        float elapsedTime = 0f;
        float totalTime = 0f;

        for (int i = 0; i < LaneData.Count; i++)
        {
            TData data = LaneData[i];
            adjustedStart = GetStart(data) - elapsedTime;
            elapsedTime += adjustedStart + GetTime(data);
            totalTime += adjustedStart + GetTime(data);
        }

        return totalTime;
    }

    protected GameObject FindInactiveObject(Transform parent, string name){
        foreach (Transform child in parent){
            if (child.name == name)  return child.gameObject;

            GameObject result = FindInactiveObject(child, name);
            if (result != null)  return result;
        }
        return null;
    }

    public void DeleteAllChild(){
        foreach(Transform child in transform){
            Destroy(child.gameObject);
        }
    }

    protected abstract float GetStart(TData data);
    protected abstract float GetTime(TData data);
}