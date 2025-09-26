using UnityEngine;
using System.IO;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class FileOutput : MonoBehaviour
{
    public TMP_InputField inputField;
    private List<ILane> allLanes = new List<ILane>();

    void Awake()
    {
        // ILaneを継承しているMonoBehaviourをすべて取得
        allLanes = FindObjectsOfType<MonoBehaviour>().OfType<ILane>().ToList();
    }

    public void OnClick()
    {
        string fileName = inputField.text;
        if (string.IsNullOrEmpty(fileName))
        {
            Debug.LogWarning("ファイル名が入力されていません。");
            return;
        }

        string outputDir = Application.dataPath + "/Output";
        Directory.CreateDirectory(outputDir);
        string path = Path.Combine(outputDir, fileName + ".txt");
        List<string> lines = new List<string>();

        foreach (var lane in allLanes)
        {
            lane.SetLaneData();
            if (lane is ArmLane armLane)
            {
                string label = armLane.armkind == armKind.Right ? "[Right ArmLane]" : "[Left ArmLane]";
                lines.Add(label);
                lines.AddRange(armLane.ExportData());
                lines.Add("");
            }
            else if (lane is SelectLane selectLane)
            {
                lines.Add("[LED Lane]");
                lines.AddRange(selectLane.ExportData());
                lines.Add("");
            }
        }

        File.WriteAllLines(path, lines);
        Debug.Log($"書き出し完了: {path}");
    }

    public void TextReset()
    {
        inputField.text = string.Empty;
    }
}