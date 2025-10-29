using UnityEngine;
using System.IO;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class FileInput : MonoBehaviour
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

        string inputDir = Application.dataPath + "/Output";
        string path = Path.Combine(inputDir, fileName + ".txt");

        if (!File.Exists(path))
        {
            Debug.LogError($"ファイルが見つかりません: {path}");
            return;
        }

        string[] lines = File.ReadAllLines(path);
        Debug.Log($"読み込み開始: {path}");

        foreach(ILane child in allLanes){
            child.DeleteAllChild();
        }


        // 現在のラベルに対応するレーン
        ILane currentLane = null;
        List<string> buffer = new List<string>();

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            // レーン切り替えラベル
            if (line.StartsWith("["))
            {
                // 以前のレーンにバッファを渡して反映
                if (currentLane != null && buffer.Count > 0)
                {
                    currentLane.ImportData(buffer);
                    buffer.Clear();
                }

                // 新しいラベルに基づいて currentLane を切り替える
                if (line == "[Right ArmLane]")
                    currentLane = allLanes.OfType<ArmLane>().FirstOrDefault(x => x.armkind == armKind.Right);
                else if (line == "[Left ArmLane]")
                    currentLane = allLanes.OfType<ArmLane>().FirstOrDefault(x => x.armkind == armKind.Left);
                else if (line == "[Head ArmLane]")
                    currentLane = allLanes.OfType<ArmLane>().FirstOrDefault(x => x.armkind == armKind.Head);
                else if (line == "[LED Lane]")
                    currentLane = allLanes.OfType<SelectLane>().FirstOrDefault(x => x.stringkind == stringKind.LED);
                else if (line == "[Music Lane]")
                    currentLane = allLanes.OfType<SelectLane>().FirstOrDefault(x => x.stringkind == stringKind.Music);
                else
                    currentLane = null;
            }
            else
            {
                // データ行をバッファに追加
                if (currentLane != null) buffer.Add(line);
            }
        }

        // 最後のレーンの残りデータを反映
        if (currentLane != null && buffer.Count > 0){
            currentLane.ImportData(buffer);
        }

        Debug.Log("読み込み完了！");
    }

    public void TextReset()
    {
        inputField.text = string.Empty;
    }
}