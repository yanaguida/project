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

    private List<int>[] laneLists; 

    private int choiceParts = 0; //選択されているパーツ
    public static Control instance;

    private void Awake()
    {
        laneLists = new List<int>[] { lane1List, lane2List, lane3List, lane4List };
        if (instance == null){
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else{
            Destroy(this.gameObject);
        }
    }

    public class InputData{
        public int partNumber;
        public float time;
        public float value;
        public InputData(int partNumber){
            this.partNumber = partNumber;
            time = float.NaN;
            GameObject timeObj = GameObject.Find("TimeInput" + partNumber);
            if (timeObj != null){
                TMPro.TMP_InputField timeInput = timeObj.GetComponent<TMPro.TMP_InputField>();
                if (timeInput != null && float.TryParse(timeInput.text, out float parsedTime)){
                    time = parsedTime;
                }
            }
           value = float.NaN;
           GameObject valueObj = GameObject.Find("Input" + partNumber);
            if (valueObj != null){
                var valueInput = valueObj.GetComponent<TMPro.TMP_InputField>();
                if (valueInput != null && float.TryParse(valueInput.text, out float parsedValue)){
                    value = parsedValue;
                }
            }
        }
    }

    public Dictionary<int, InputData> LaneDict(int lane){
        switch (lane)
        {
            case 0: return Lane1Dict(); 
            case 1: return Lane2Dict(); 
            case 2: return Lane3Dict(); 
            case 3: return Lane4Dict();
            default: return new Dictionary<int, InputData>();
        }
    }

    public Dictionary<int, InputData> Lane1Dict(){
        Dictionary<int, InputData> lane1Dict = new Dictionary<int, InputData>();
        for (int i = 0; i < lane1List.Count; i++){
            int partNumber = lane1List[i]; 
            lane1Dict[i] = new InputData(partNumber); 
        }
        return lane1Dict;
    }

    public Dictionary<int, InputData> Lane2Dict(){
        Dictionary<int, InputData> lane2Dict = new Dictionary<int, InputData>();
        for (int i = 0; i < lane2List.Count; i++){
            int partNumber = lane2List[i]; 
            lane2Dict[i] = new InputData(partNumber); 
        }
        return lane2Dict;
    }

    public Dictionary<int, InputData> Lane3Dict(){
        Dictionary<int, InputData> lane3Dict = new Dictionary<int, InputData>();
        for (int i = 0; i < lane3List.Count; i++){
            int partNumber = lane3List[i]; 
            lane3Dict[i] = new InputData(partNumber); 
        }
        return lane3Dict;
    }
    
    public Dictionary<int, InputData> Lane4Dict(){
        Dictionary<int, InputData> lane4Dict = new Dictionary<int, InputData>();
        for (int i = 0; i < lane4List.Count; i++){
            int partNumber = lane4List[i]; 
            lane4Dict[i] = new InputData(partNumber); 
        }
        return lane4Dict;
    }

    //引数にあるパーツの番号を現在選択中のパーツとして変数に保存するセッター
    public void SetChoiceParts(int num){
        choiceParts = num;
    }

    //ゲッター
    public int GetChoiceParts(){
        return choiceParts;
    }

     //リストに値を追加する関数
    public void AddToLane(int lane, int num){
        switch (lane)
        {
            case 0: Addlean1(num); break;
            case 1: Addlean2(num); break;
            case 2: Addlean3(num); break;
            case 3: Addlean4(num); break;
        }
    }

    public void Addlean1(int num){
        lane1List.Add(num);
    }
    public void Addlean2(int num){
        lane2List.Add(num);
    }
    public void Addlean3(int num){
        lane3List.Add(num);
    }
    public void Addlean4(int num){
        lane4List.Add(num);
    }

    //対象レーンのリストにおいて引数にあるパーツの番号が何番目にあるかを返す
    public int Countlean1(int num){
        int x = lane1List.IndexOf(num);
        return x;
    }
    public int Countlean2(int num){
        int x = lane2List.IndexOf(num);
        return x;
    }
    public int Countlean3(int num){
        int x = lane3List.IndexOf(num);
        return x;
    }
    public int Countlean4(int num){
        int x = lane4List.IndexOf(num);
        return x;
    }

    //対象レーンのリストに引数にあるパーツの番号が含まれているかを確認して、あれば1無ければ0を返す
    public bool IsInLane(int lane, int num){
        switch (lane){
            case 0: return Checklean1(num) == 1;
            case 1: return Checklean2(num) == 1;
            case 2: return Checklean3(num) == 1;
            case 3: return Checklean4(num) == 1;
            default: return false;
        }
    }

    public int Checklean1(int num){
        if (lane1List.Contains(num) == true)
            return 1;
        else return 0;
    }
    public int Checklean2(int num){
        if (lane2List.Contains(num) == true)
            return 1;
        else return 0;
    }
    public int Checklean3(int num){
        if (lane3List.Contains(num) == true)
            return 1;
        else return 0;
    }
    public int Checklean4(int num){
        if (lane4List.Contains(num) == true)
            return 1;
        else return 0;
    }

    //対象レーンのリストから引数にあるパーツの番号を取り除く
    public void RemoveFromLane(int lane, int num){
        switch (lane){
            case 0: Removelean1(num); break;
            case 1: Removelean2(num); break;
            case 2: Removelean3(num); break;
            case 3: Removelean4(num); break;
        }
    }

    public void Removelean1(int num){
        lane1List.Remove(num);
    }
    public void Removelean2(int num){
        lane2List.Remove(num);
    }
    public void Removelean3(int num){
        lane3List.Remove(num);
    }
    public void Removelean4(int num){
        lane4List.Remove(num);
    }

    //対象レーンをX軸で並べ替える
    public void SortLane(int lane){
         switch (lane){
            case 0: Sortlean1(); break;
            case 1: Sortlean2(); break;
            case 2: Sortlean3(); break;
            case 3: Sortlean4(); break;
        }
    }

    public void Sortlean1(){
        lane1List.Sort((a, b) => {
            float xa = GetX(a);
            float xb = GetX(b);
            return xa.CompareTo(xb);
        });
    }
     public void Sortlean2(){
        lane2List.Sort((a, b) => {
            float xa = GetX(a);
            float xb = GetX(b);
            return xa.CompareTo(xb);
        });
    }
     public void Sortlean3(){
        lane3List.Sort((a, b) => {
            float xa = GetX(a);
            float xb = GetX(b);
            return xa.CompareTo(xb);
        });
    }
     public void Sortlean4(){
        lane4List.Sort((a, b) => {
            float xa = GetX(a);
            float xb = GetX(b);
            return xa.CompareTo(xb);
        });
    }

    float GetX(int partID){
        GameObject obj = GameObject.Find("Part" + partID);
        if (obj == null) return float.MaxValue; 
            RectTransform rt = obj.GetComponent<RectTransform>();
        return rt.anchoredPosition.x;
    }

    //FileOutPut用関数
    public string Getlean(int laneNum){
        return string.Join(", ", laneLists[laneNum - 1].Select(obj => obj.ToString()));
    }

    public string GetValues(int laneNum){
        List<string> values = new List<string>();
        List<int> targetList = new List<int>(laneLists[laneNum - 1]);
        foreach (int x in targetList){
            GameObject obj = GameObject.Find("Input" + x);
            if (obj == null){
                values.Add("NULL");
                continue;
            }
            TMPro.TMP_InputField input = obj.GetComponent<TMPro.TMP_InputField>();
            if (input == null || string.IsNullOrEmpty(input.text)){
                values.Add("NULL");
            }
            else{
                values.Add(input.text);
            }
        }   
        return string.Join(", ", values);
    }
}