using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Playback : MonoBehaviour
{
    /*
    ボタンが押されたら
    エフェクトを流し、テキストを変更
    レーン１～４を同時に実行する。
    */
    public Functions funScript; 
    public ParticleSystem electricEffect;
    public GameObject StartButton;
    public GameObject StopButton;
    public Control control;
    public GameObject RedLineObj;
    private int checklastlane=0;
    private Coroutine redlineCoroutine;
    private bool isPlaying = false;

    public void Awake(){
        SwitchText(true);
    }

    public void OnClick(){
        if (isPlaying) return;
        isPlaying = true;
        List<MotionPartData> dataList1 = control.GetLaneData(0);
        StartCoroutine(ExecuteLane1(dataList1));
        List<MotionPartData> dataList2 = control.GetLaneData(1);
        StartCoroutine(ExecuteLane2(dataList2));
        if(RedLineObj != null){
            RedLineObj.SetActive(true);
            RectTransform RedLine = RedLineObj.GetComponent<RectTransform>();
            if(RedLine !=  null)
            redlineCoroutine = StartCoroutine(Redline(RedLine, 200f));
        }
        SwitchText(false);
    }

    IEnumerator ExecuteLane1(List<MotionPartData> dataList)
    {
        float elapsedtime = 0f;
        for (int i = 0; i < dataList.Count; i++)
        {
            MotionPartData data = dataList[i];
            float correctstart = data.start - elapsedtime;
            Debug.Log($"Index: {i}, Part: {data.partNumber},Start: {correctstart}, Time: {data.time},Value: {data.value}");
            yield return StartCoroutine(funScript.Wait(correctstart));
            yield return StartCoroutine(funScript.RightArm(data.time, data.value));
            elapsedtime += correctstart+data.time;
        }
        if(checklastlane==0)
        checklastlane++;
        else{
            SwitchText(true);
            StopRedline();
            isPlaying = false;
        }
    }

    IEnumerator ExecuteLane2(List<MotionPartData> dataList)
    {
        float elapsedtime = 0f;
        for (int i = 0; i < dataList.Count; i++)
        {
            MotionPartData data = dataList[i];
            float correctstart = data.start - elapsedtime;
            yield return StartCoroutine(funScript.Wait(correctstart));
            yield return StartCoroutine(funScript.LeftArm(data.time, data.value));
            elapsedtime += correctstart+data.time;
        }
        if(checklastlane==0)
        checklastlane++;
        else{
            SwitchText(true);
            StopRedline();
            isPlaying = false;
        }
    }

    IEnumerator Redline(RectTransform obj, float speed){
        while (true){
            obj.anchoredPosition += Vector2.right * speed * Time.deltaTime;
            yield return null;
        }
    }

    private void StopRedline(){
        if (redlineCoroutine != null){
            StopCoroutine(redlineCoroutine);
            redlineCoroutine = null;
        }
        if (RedLineObj != null){
            RedLineObj.SetActive(false);
            RectTransform redLine = RedLineObj.GetComponent<RectTransform>();
            redLine.anchoredPosition = new Vector2(320f, redLine.anchoredPosition.y); 
        }
        checklastlane = 0;
    }

    private void SwitchText(bool i){
        StartButton.SetActive(i);
        StopButton.SetActive(!i);
    }
}