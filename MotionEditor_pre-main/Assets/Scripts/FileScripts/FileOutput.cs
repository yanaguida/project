using UnityEngine;
using System.IO;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class FileOutput : FileAbstract
{

    public override void OnClick()
    {
        base.OnClick();

        string outputDir = Application.dataPath + "/Output";
        Directory.CreateDirectory(outputDir);
        string path = Path.Combine(outputDir, fileName + ".txt");
        List<string> lines = new List<string>();

        foreach (var lane in allLanes)
        {
            lane.SetLaneData();
            if (lane is ArmLane armLane)
            {
                string label;
                if(armLane.armkind == armKind.Right) label = "[Right ArmLane]";
                else if(armLane.armkind == armKind.Left) label = "[Left ArmLane]";
                else label = "[Head ArmLane]";
                lines.Add(label);
                lines.AddRange(armLane.ExportData());
            }
            else if (lane is SelectLane selectLane)
            {
                string label = selectLane.stringkind == stringKind.LED ? "[LED Lane]" : "[Music Lane]";
                lines.Add(label);
                lines.AddRange(selectLane.ExportData());
            }
        }

        File.WriteAllLines(path, lines);
        Debug.Log($"書き出し完了: {path}");
    }
}