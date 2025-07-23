using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using System.Linq;
using NUnit.Framework;

public class Control : MonoBehaviour

{
    //レーン4つ分のリスト
    public List<int> lane1List = new List<int>();
    public List<int> lane2List = new List<int>();
    public List<int> lane3List = new List<int>();
    public List<int> lane4List = new List<int>();

    private List<int>[] laneLists; // array to store lane**List

    private int choiceParts = 0; //選択されているパーツ


    public static Control instance;

    private void Awake()
    {

        laneLists = new List<int>[] { lane1List, lane2List, lane3List, lane4List };


        //インスタンス宣言？
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }

    //引数にあるパーツの番号を現在選択中のパーツとして変数に保存するセッター
    public void SetChoiceParts(int num)
    {
        choiceParts = num;
    }

    //ゲッター
    public int GetChoiceParts()
    {
        return choiceParts;
    }



    //対象レーンのリストに引数にあるパーツの番号を追加する
    public void Addlean1(int num)
    {
        lane1List.Add(num);//lane1に追加

    }
    public void Addlean2(int num)
    {
        lane2List.Add(num);//lane2に追加
    }
    public void Addlean3(int num)
    {
        lane3List.Add(num);//lane3に追加
    }
    public void Addlean4(int num)
    {
        lane4List.Add(num);//lane4に追加
    }


    //対象レーンのリストにおいて引数にあるパーツの番号が何番目にあるかを返す
    public int Countlean1(int num)
    {
        int x = lane1List.IndexOf(num);
        return x;
    }
    public int Countlean2(int num)
    {
        int x = lane2List.IndexOf(num);
        return x;
    }
    public int Countlean3(int num)
    {
        int x = lane3List.IndexOf(num);
        return x;
    }
    public int Countlean4(int num)
    {
        int x = lane4List.IndexOf(num);
        return x;
    }


    //対象レーンのリストに引数にあるパーツの番号が含まれているかを確認して、あれば1無ければ0を返す
    public int Checklean1(int num)
    {
        if (lane1List.Contains(num) == true)
            return 1;
        else return 0;
    }

    public int Checklean2(int num)
    {

        if (lane2List.Contains(num) == true)
            return 1;
        else return 0;
    }

    public int Checklean3(int num)
    {

        if (lane3List.Contains(num) == true)
            return 1;
        else return 0;
    }

    public int Checklean4(int num)
    {

        if (lane4List.Contains(num) == true)
            return 1;
        else return 0;
    }


    //対象レーンのリストから引数にあるパーツの番号を取り除く
    public void Removelean1(int num)
    {
        lane1List.Remove(num);
    }
    public void Removelean2(int num)
    {
        lane2List.Remove(num);
    }
    public void Removelean3(int num)
    {
        lane3List.Remove(num);
    }
    public void Removelean4(int num)
    {
        lane4List.Remove(num);
    }

    public string Getlean(int laneNum)
    {
        return string.Join(", ", laneLists[laneNum - 1].Select(obj => obj.ToString()));
    }


    public string GetValues(int laneNum)
    {
        List<string> values = new List<string>(); // レーンに並んだパーツの値を保存するリスト
        List<int> targetList = new List<int>(laneLists[laneNum - 1]); // 指定されたレーンリストの中身をtargetListにコピー
        foreach (int x in targetList) // targetListの全要素からpart** object を見つける
        {
            TMPro.TMP_InputField targetInput = GameObject.Find("Input" + x).GetComponent<TMPro.TMP_InputField>();
            if (targetInput.text.Length <= 0)
            {
                values.Add("NULL");
            }
            else
            {
                values.Add(targetInput.text);
            }
        }
        return string.Join(", ", values.Select(obj => obj.ToString()));
    }


    public void SetCPfromClick(int num)
    {
        choiceParts = num;
    }
       public List<int> GetColumnLanes(int columnIndex){
        List<int> column = new List<int>();
        foreach (var row in laneLists){
            if (columnIndex < row.Count)
                column.Add(row[columnIndex]);
            else
        column.Add(0); // データが不足している場合は0で埋める（調整可）
        }
        return column;
    }

    public List<float> GetColumnValues(int columnIndex){
        List<float> columnValues = new List<float>();
        List<int>[] laneLists = new List<int>[] { lane1List, lane2List, lane3List, lane4List };
        foreach (List<int> lane in laneLists){
            if (columnIndex < lane.Count){
                int inputIndex = lane[columnIndex];
                GameObject obj = GameObject.Find("Input" + inputIndex);
                if (obj == null){
                    columnValues.Add(float.NaN);
                    continue;
                }
                TMPro.TMP_InputField input = obj.GetComponent<TMPro.TMP_InputField>();
                if (input == null || string.IsNullOrWhiteSpace(input.text)){
                    columnValues.Add(float.NaN);
                }
                else{
                    if (float.TryParse(input.text, out float value)){
                        columnValues.Add(value);
                    }
                    else{
                        columnValues.Add(float.NaN);
                    }
                }
            }
            else{
                columnValues.Add(float.NaN); // 要素がなければ NaN 補完
            }
        }
        return columnValues;
    }
    public Dictionary<int, float> GetColumnLaneValueDict(int columnIndex){
        List<int> lanes = GetColumnLanes(columnIndex);
        List<float> values = GetColumnValues(columnIndex);
        Dictionary<int, float> result = new Dictionary<int, float>();
        for (int i = 0; i < Mathf.Min(lanes.Count, values.Count); i++){
            int partId = lanes[i];
            float value = values[i];
        if (!float.IsNaN(value))
        {
            result[partId] = value;
        }
        }
        return result;
    }
}