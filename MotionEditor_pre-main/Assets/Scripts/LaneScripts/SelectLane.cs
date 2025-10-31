using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class SelectLane : Lanes<SelectPartData>
{
    public LED ledScript;
    public Audio musicScript;
    public stringKind stringkind;

    public override void ImportData(List<string> lines){
        LaneData.Clear();

        foreach (string line in lines){
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] parts = line.Split(',');

            float start = 0f;
            float time = 0f;
            string emotion = "";

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
                    case "emotion":
                        emotion = val;
                        break;
                }
            }

            SelectPartData data = new SelectPartData(start, time, emotion);
            LaneData.Add(data);
        }
        LaneData.Sort((a, b) => GetStart(a).CompareTo(GetStart(b)));
        CreateChildFromData();
    }

    protected override void CreateChildFromData(){
        foreach (var data in LaneData){
            GameObject clone;
            if(stringkind == stringKind.LED){
                GameObject obj = FindInactiveObject(transform.root,"LCDIcon");
                if(obj != null){
                    clone = Instantiate(obj,this.transform);
                    clone.SetActive(true);
                }
                else {
                    Debug.Log("LCDIconがnull");
                    break;
                }
            }
            else{
                GameObject obj = FindInactiveObject(transform.root,"MusicIcon");
                if(obj != null){
                    clone = Instantiate(obj,this.transform);
                    clone.SetActive(true);
                }
                else {
                    Debug.Log("MusicIconがnull");
                    break;
                }
            }
            float dtime = (data.time-4f)*50f;
            Vector2 setPos;
            setPos.x = data.start*ValueBox.GetDis()-ValueBox.GetAdjustX()+dtime;
            setPos.y = 0f;
            RectTransform cloneRT = clone.GetComponent<RectTransform>();
            cloneRT.anchoredPosition = setPos;
            Vector2 size;
            size.x = data.time*ValueBox.GetDis();
            size.y = 200f;
            cloneRT.sizeDelta = size;
            TMP_Dropdown inputField = clone.GetComponentInChildren<TMP_Dropdown>(true);
            if(data.emotion=="Smile") inputField.value = 0;
            else if(data.emotion=="Sad") inputField.value = 1;
            else if(data.emotion=="Wink") inputField.value = 2;
            SelectIcon cloneScript = clone.GetComponent<SelectIcon>();
            cloneScript.SetData(data.start,data.time,data.emotion);
        }
    }

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
        if(stringkind == stringKind.LED)
        yield return StartCoroutine(ledScript.Toggle(data.time,data.emotion));
        else if(stringkind == stringKind.Music)
        yield return StartCoroutine(musicScript.PlayForSeconds(data.time, data.emotion));
        else
        Debug.Log("stringKindが未設定");
    }

    public List<string> ExportData(){
        var lines = new List<string>();
        foreach (var data in LaneData){
            lines.Add($"start:{data.start},time:{data.time},emotion:{data.emotion}");
        }
        return lines;
    }

    protected override float GetStart(SelectPartData data) => data.start;
    protected override float GetTime(SelectPartData data) => data.time;
}
