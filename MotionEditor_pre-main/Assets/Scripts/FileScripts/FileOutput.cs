using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class FileOutput : FileAbstract
{
    public GameObject confirmDialog;
    private string pendingPath;

    public override void OnClick()
    {
        base.OnClick();

        string outputDir = Application.dataPath + "/Output";
        Directory.CreateDirectory(outputDir);
        string path = Path.Combine(outputDir, fileName + ".txt");

        if (File.Exists(path))
        {
            pendingPath = path;
            confirmDialog.SetActive(true); // 警告ダイアログを表示
            return;
        }

        SaveFile(path);
    }

    public void OnConfirmOverwrite(bool overwrite)
    {
        confirmDialog.SetActive(false);

        if (overwrite)
        {
            SaveFile(pendingPath);
        }
        else
        {
            Debug.Log("上書きをキャンセルしました。");
        }
    }

    private void SaveFile(string path)
    {
        List<string> lines = new List<string>();
        SetLaneData();

        lines.AddRange(ExportData());

        File.WriteAllLines(path, lines);
        Debug.Log($"書き出し完了: {path}");
    }

    private List<string> ExportData()
    {
        var lines = new List<string>();
        bool r = true;
        bool l = true;
        bool h = true;
        bool c = true;
        bool s = true;

        foreach (var icon in IconList)
        {
            if (icon.GetPartType() == PartType.RightWing)
            {
                if (r)
                {
                    lines.Add("[Right ArmLane]");
                    r = false;
                }
                lines.Add($"start:{icon.GetStart()},time:{icon.GetTime()},value:{icon.GetValue()}");
            }
            else if (icon.GetPartType() == PartType.LeftWing)
            {
                if (l)
                {
                    lines.Add("[Left ArmLane]");
                    l = false;
                }
                lines.Add($"start:{icon.GetStart()},time:{icon.GetTime()},value:{icon.GetValue()}");
            }
            else if (icon.GetPartType() == PartType.Head)
            {
                if (h)
                {
                    lines.Add("[Head ArmLane]");
                    h = false;
                }
                lines.Add($"start:{icon.GetStart()},time:{icon.GetTime()},value:{icon.GetValue()}");
            }

            if (icon.GetPartType() == PartType.LCD)
            {
                if (c)
                {
                    lines.Add("[LED Lane]");
                    c = false;
                }
                if (icon.GetValue() == 0f)
                    lines.Add($"start:{icon.GetStart()},time:{icon.GetTime()},value:Happy");
                else if (icon.GetValue() == 1f)
                    lines.Add($"start:{icon.GetStart()},time:{icon.GetTime()},value:Sad");
                else if (icon.GetValue() == 2f)
                    lines.Add($"start:{icon.GetStart()},time:{icon.GetTime()},value:Angry");
                else if (icon.GetValue() == 3f)
                    lines.Add($"start:{icon.GetStart()},time:{icon.GetTime()},value:Enjoy");
            }
            else if (icon.GetPartType() == PartType.Singing)
            {
                if (s)
                {
                    lines.Add("[Music Lane]");
                    s = false;
                }
                if (icon.GetValue() == 0f)
                    lines.Add($"start:{icon.GetStart()},time:{icon.GetTime()},value:Happy");
                else if (icon.GetValue() == 1f)
                    lines.Add($"start:{icon.GetStart()},time:{icon.GetTime()},value:Sad");
                else if (icon.GetValue() == 2f)
                    lines.Add($"start:{icon.GetStart()},time:{icon.GetTime()},value:Angry");
                else if (icon.GetValue() == 3f)
                    lines.Add($"start:{icon.GetStart()},time:{icon.GetTime()},value:Enjoy");
            }
            icon.SetIssaved(true);
        }
        return lines;
    }

    private void SetLaneData()
    {
        IconList.Clear();
        IconList = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IIcon>().ToList();
        IconList.RemoveAll(icon => icon.GetIconState() != IconState.OnLane);
        foreach (var icon in IconList)
        {
            icon.SetData();
            //Debug.Log($"Before Sort: Part={icon.GetPartType()}, Start={icon.GetStart()}, Obj={icon}");
        }
        IconList.Sort((a, b) =>
        {
            int result = a.GetPartType().CompareTo(b.GetPartType()); // まず PartType の値でソート
            if (result == 0)
            {
                // Start が同じなら Start でソート
                result = a.GetStart().CompareTo(b.GetStart());
            }
            return result;
        });
    }
}