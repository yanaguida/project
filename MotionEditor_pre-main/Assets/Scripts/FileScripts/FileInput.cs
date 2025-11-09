using UnityEngine;
using System.IO;
using System.Collections.Generic;
using TMPro;

public class FileInput : FileAbstract
{
    private List<IconData> IconData = new List<IconData>();
    private PartType parttype;
    private GameObject clone;

    public override void OnClick()
    {
        base.OnClick();

        string inputDir = Application.dataPath + "/Output";
        string path = Path.Combine(inputDir, fileName + ".txt");

        if (!File.Exists(path))
        {
            Debug.LogError($"ファイルが見つかりません: {path}");
            return;
        }

        string[] lines = File.ReadAllLines(path);
        Debug.Log($"読み込み開始: {path}");


        // 現在のラベルに対応するレーン
        List<string> buffer = new List<string>();

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            // レーン切り替えラベル
            if (line.StartsWith("["))
            {
                // 以前のレーンにバッファを渡して反映
                if (buffer.Count > 0)
                {
                    ImportData(buffer);
                    buffer.Clear();
                }

                // 新しいラベルに基づいて currentLane を切り替える
                if (line == "[Right ArmLane]")
                    parttype = PartType.rightwing;
                else if (line == "[Left ArmLane]")
                    parttype = PartType.leftwing;
                else if (line == "[Head ArmLane]")
                    parttype = PartType.head;
                else if (line == "[LED Lane]")
                    parttype = PartType.lcd;
                else if (line == "[Music Lane]")
                    parttype = PartType.singing;
                else
                    Debug.Log("There are undefined tag: " + line);
            }
            else
            {
                // データ行をバッファに追加
                buffer.Add(line);
            }
        }

        // 最後のレーンの残りデータを反映
        if (buffer.Count > 0)
        {
            ImportData(buffer);
        }

        Debug.Log("読み込み完了！");
    }

    private void ImportData(List<string> lines)
    {
        IconData.Clear();

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] parts = line.Split(',');

            float start = 0f;
            float time = 0f;
            float value = 0f;

            foreach (string part in parts)
            {
                string[] kv = part.Split(':');
                if (kv.Length != 2) continue;

                string key = kv[0].Trim();
                string val = kv[1].Trim();

                switch (key)
                {
                    case "start":
                        float.TryParse(val, out start);
                        break;
                    case "time":
                        float.TryParse(val, out time);
                        break;
                    case "value":
                        if (val == "Smile") value = 0f;
                        else if (val == "Sad") value = 1f;
                        else if (val == "Wink") value = 2f;
                        else float.TryParse(val, out value);
                        break;
                }
            }

            IconData data = new IconData(parttype, start, time, value);
            IconData.Add(data);
        }
        IconData.Sort((a, b) => a.start.CompareTo(b.start));
        CreateChildFromData();
    }

    private void CreateChildFromData()
    {
        foreach (var data in IconData)
        {
            SetCloneObj();
            float dtime = (data.time - 4f) * 50f;
            Vector2 setPos;
            setPos.x = data.start * ValueBox.GetDis() - ValueBox.GetAdjustX() + dtime;
            setPos.y = 0f;
            RectTransform cloneRT = clone.GetComponent<RectTransform>();
            cloneRT.anchoredPosition = setPos;
            Vector2 size;
            size.x = data.time * ValueBox.GetDis();
            size.y = 200f;
            cloneRT.sizeDelta = size;
            if (PartClassify.Classify(data.parttype) == "onefloat")
            {
                TMP_InputField inputField = clone.GetComponentInChildren<TMP_InputField>(true);
                if (inputField == null) Debug.Log("inputfieldがnull");
                inputField.text = (data.value).ToString();
                ArmIcon cloneScript = clone.GetComponent<ArmIcon>();
                cloneScript.SetData(parttype, data.start, data.time, data.value);
            }
            else if (PartClassify.Classify(data.parttype) == "onestring")
            {
                TMP_Dropdown inputField = clone.GetComponentInChildren<TMP_Dropdown>(true);
                inputField.value = (int)data.value;
                SelectIcon cloneScript = clone.GetComponent<SelectIcon>();
                cloneScript.SetData(parttype, data.start, data.time, data.value);
            }
        }
    }

    private void SetCloneObj()
    {
        if (parttype == PartType.rightwing)
        {
            GameObject obj = FindInactiveObject(transform.root, "RightWingIcon");
            Icons icon = obj.GetComponent<Icons>();
            if (obj != null)
            {
                clone = Instantiate(obj, icon.laneRects);
                icon.issaved = true;
                clone.SetActive(true);
            }
            else
            {
                Debug.Log("RightWingIconがnull");
                return;
            }
        }
        else if (parttype == PartType.leftwing)
        {
            GameObject obj = FindInactiveObject(transform.root, "LeftWingIcon");
            Icons icon = obj.GetComponent<Icons>();
            if (obj != null)
            {
                clone = Instantiate(obj, icon.laneRects);
                icon.issaved = true;
                clone.SetActive(true);
            }
            else
            {
                Debug.Log("LeftWingIconがnull");
                return;
            }
        }
        else if (parttype == PartType.head)
        {
            GameObject obj = FindInactiveObject(transform.root, "HeadIcon");
            Icons icon = obj.GetComponent<Icons>();
            if (obj != null)
            {
                clone = Instantiate(obj, icon.laneRects);
                icon.issaved = true;
                clone.SetActive(true);
            }
            else
            {
                Debug.Log("HeadIconがnull");
                return;
            }
        }
        else if (parttype == PartType.lcd)
        {
            GameObject obj = FindInactiveObject(transform.root, "LCDIcon");
            Icons icon = obj.GetComponent<Icons>();
            if (obj != null)
            {
                clone = Instantiate(obj, icon.laneRects);
                icon.issaved = true;
                clone.SetActive(true);
            }
            else
            {
                Debug.Log("LCDIconがnull");
                return;
            }
        }
        else
        {
            GameObject obj = FindInactiveObject(transform.root, "MusicIcon");
            Icons icon = obj.GetComponent<Icons>();
            if (obj != null)
            {
                clone = Instantiate(obj, icon.laneRects);
                icon.issaved = true;
                clone.SetActive(true);
            }
            else
            {
                Debug.Log("MusicIconがnull");
                return;
            }
        }
    }

    private GameObject FindInactiveObject(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name) return child.gameObject;

            GameObject result = FindInactiveObject(child, name);
            if (result != null) return result;
        }
        return null;
    }

}