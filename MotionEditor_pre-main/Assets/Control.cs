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
    private List<int> lane1List = new List<int>();
    private List<int> lane2List = new List<int>();
    private List<int> lane3List = new List<int>();
    private List<int> lane4List = new List<int>();

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


    void Update()
    {
        //なにもしない
    }


    //引数にあるパーツの番号を現在選択中のパーツとして変数に保存するセッター
    public void SetChoiceParts(int num)
    {
        choiceParts = num;
        Debug.Log(choiceParts);
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

        Debug.Log(string.Join(", ", lane1List.Select(obj => obj.ToString())));
    }
    public void Addlean2(int num)
    {
        lane2List.Add(num);//lane2に追加
        Debug.Log(string.Join(", ", lane2List.Select(obj => obj.ToString())));
    }
    public void Addlean3(int num)
    {
        lane3List.Add(num);//lane3に追加
        Debug.Log(string.Join(", ", lane3List.Select(obj => obj.ToString())));
    }
    public void Addlean4(int num)
    {
        lane4List.Add(num);//lane4に追加
        Debug.Log(string.Join(", ", lane4List.Select(obj => obj.ToString())));
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

        Debug.Log(string.Join(", ", values.Select(obj => obj.ToString())));
        return string.Join(", ", values.Select(obj => obj.ToString()));
    }


    public void SetCPfromClick(int num)
    {
        choiceParts = num;
    }


}
