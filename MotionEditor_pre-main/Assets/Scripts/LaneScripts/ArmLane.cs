using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class ArmLane : Lanes<ArmData>
{
    public Motors motorScript; 
    public armKind armkind;

    public override void ImportData(List<string> lines){
        LaneData.Clear();

        foreach (string line in lines){
            if (string.IsNullOrWhiteSpace(line))  continue;

            string[] parts = line.Split(',');

            float start = 0f;
            float time = 0f;
            float value = 0f;

            foreach (string part in parts){
                string[] kv = part.Split(':');
                if (kv.Length != 2)  continue;

                string key = kv[0].Trim();
                string val = kv[1].Trim();

                switch (key){
                    case "start":
                        float.TryParse(val, out start);
                        break;
                    case "time":
                        float.TryParse(val, out time);
                        break;
                    case "value":
                        float.TryParse(val, out value);
                        break;
                }
            }

            ArmData data = new ArmData(start, time, value);
            LaneData.Add(data);
        }
        LaneData.Sort((a, b) => GetStart(a).CompareTo(GetStart(b)));
        CreateChildFromData();
    }

    protected override void CreateChildFromData(){
        foreach (var data in LaneData){
            GameObject clone;
            if(armkind == armKind.Right){
                GameObject obj = FindInactiveObject(transform.root,"RightWingIcon");
                if(obj != null)  {
                    clone = Instantiate(obj,this.transform);
                    clone.SetActive(true);
                }
                else {
                    Debug.Log("RightWingIconがnull");
                    break;
                }
            }
            else if(armkind == armKind.Left){
                GameObject obj = FindInactiveObject(transform.root,"LeftWingIcon");
                if(obj != null){
                    clone = Instantiate(obj,this.transform);
                    clone.SetActive(true);
                }
                else {
                    Debug.Log("LeftWingIconがnull");
                    break;
                }
            }
            else{
                GameObject obj = FindInactiveObject(transform.root,"HeadIcon");
                if(obj != null){
                    clone = Instantiate(obj,this.transform);
                    clone.SetActive(true);
                }
                else {
                    Debug.Log("HeadIconがnull");
                    break;
                }
            }
            float dtime = (data.time-4f)*50f;
            Vector2 setPos;
            setPos.x = data.start*100f-5480f+dtime;
            setPos.y = 0f;
            RectTransform cloneRT = clone.GetComponent<RectTransform>();
            cloneRT.anchoredPosition = setPos;
            Vector2 size;
            size.x = data.time*100f;
            size.y = 200f;
            cloneRT.sizeDelta = size;
            TMP_InputField inputField = clone.GetComponentInChildren<TMP_InputField>(true);
            if(inputField==null) Debug.Log("inputfieldがnull");
            inputField.text = (data.value).ToString();
            ArmIcon cloneScript = clone.GetComponent<ArmIcon>();
            cloneScript.SetData(data.start,data.time,data.value);
        }
    }


    protected override ArmData CreateDataFromChild(Transform child){
        ArmIcon icon = child.GetComponent<ArmIcon>();
        if(icon!=null)
        return new ArmData(icon.GetStart(),icon.GetTime(),icon.GetValue());
        else
        Debug.Log("ArmIconの取得を失敗");
        return null;
    }

    protected override IEnumerator ExecuteAction(ArmData data){
        yield return StartCoroutine(motorScript.Motor(data.time,data.value));
    }

    public List<string> ExportData(){
        var lines = new List<string>();
        foreach (var data in LaneData){
            lines.Add($"start:{data.start},time:{data.time},value:{data.value}");
        }
        return lines;
    }

    protected override float GetStart(ArmData data) => data.start;
    protected override float GetTime(ArmData data) => data.time;
}