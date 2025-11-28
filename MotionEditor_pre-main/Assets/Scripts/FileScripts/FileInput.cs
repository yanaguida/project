using UnityEngine;
using System.IO;
using TMPro;

public class FileInput : FileAbstract
{
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
        string buffer = "\0";

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            // 以前のレーンにバッファを渡して反映
            if (buffer != "\0")
            {
                ImportData(buffer);
                buffer = "\0";
            }

            // レーン切り替えラベル
            if (line.StartsWith("["))
            {
                // 新しいラベルに基づいて currentLane を切り替える
                if (line == "[Right ArmLane]")
                    parttype = PartType.RightWing;
                else if (line == "[Left ArmLane]")
                    parttype = PartType.LeftWing;
                else if (line == "[Head ArmLane]")
                    parttype = PartType.Head;
                else if (line == "[LED Lane]")
                    parttype = PartType.LCD;
                else if (line == "[Music Lane]")
                    parttype = PartType.Singing;
                else
                    Debug.Log("There are undefined tag: " + line);
            }
            else
            {
                // データ行をバッファに追加
                buffer = line;
            }
        }

        // 最後のレーンの残りデータを反映
        if (buffer != "\0") ImportData(buffer);

        Debug.Log("読み込み完了！");
    }

    private void ImportData(string lines)
    {
        string[] parts = lines.Split(',');

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
                    if (val == "Happy") value = 0f;
                    else if (val == "Sad") value = 1f;
                    else if (val == "Angry") value = 2f;
                    else if (val == "Enjoy") value = 3f;
                    else float.TryParse(val, out value);
                    break;
            }
        }
        CreateChildFromData(start, time, value);
    }

    private void CreateChildFromData(float start, float time, float value)
    {
        SetCloneObj();
        RectTransform cloneRT = clone.GetComponent<RectTransform>();
        Vector2 setPos;
        Vector2 size;
        size.x = time * ValueBox.GetDis();
        size.y = 200f;
        cloneRT.sizeDelta = size;
        setPos.x = start * ValueBox.GetDis() - 6000f + cloneRT.sizeDelta.x / 2f;
        setPos.y = 0f;
        cloneRT.anchoredPosition = setPos;
        IIcon cloneScript = clone.GetComponent<IIcon>();
        cloneScript.SetIssaved(true);
        TMP_InputField inputField = clone.GetComponentInChildren<TMP_InputField>(true);
        if (inputField != null)
            inputField.text = value.ToString();
        TMP_Dropdown dropdown = clone.GetComponentInChildren<TMP_Dropdown>(true);
        if (dropdown != null)
            dropdown.value = (int)value;
    }

    private void SetCloneObj()
    {
        GameObject obj = FindInactiveObject(transform.root, parttype + "Icon");
        if (obj != null)
        {
            IIcon icon = obj.GetComponent<IIcon>();
            clone = Instantiate(obj, icon.GetLaneRT());
            clone.SetActive(true);
        }
        else
        {
            Debug.Log(parttype + "Iconがnull");
            return;
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