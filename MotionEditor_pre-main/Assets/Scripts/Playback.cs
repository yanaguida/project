using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Playback : MonoBehaviour
{
    public Functions funScript; 
    public GameObject StartButton;
    public GameObject StopButton;
    public GameObject RedLineObj;
    private Coroutine redlineCoroutine;
    private bool isPlaying = false;
    private float redLineSpeed = 200f;
    private ArmLane rightarm;
    private ArmLane leftarm;
    private SelectLane light;
    public Functions FunScript;
    public SwitchOn switchScript;

    public void Awake(){
        SwitchText(true);
        GameObject lane1 = GameObject.Find("lane_1");
        rightarm = lane1.AddComponent<ArmLane>();
        rightarm.SetKind(armKind.Right);
        GameObject lane2 = GameObject.Find("lane_2");
        leftarm = lane2.AddComponent<ArmLane>();
        leftarm.SetKind(armKind.Left);
        GameObject lane3 = GameObject.Find("lane_3");
        light = lane3.AddComponent<SelectLane>();
        rightarm.funScript = funScript;
        leftarm.funScript = funScript;
        light.switchScript = switchScript;
    }

    public void OnClick(){
        if (isPlaying) return;
        StartPlayback();
        StartCoroutine(StopPlayback(CalculateLastLaneTime(rightarm.GetTotalTime(),leftarm.GetTotalTime(),light.GetTotalTime())));
    }

    private void StartPlayback(){
        isPlaying = true;
        rightarm.SetLaneData();
        StartCoroutine(rightarm.ExecuteLane());
        leftarm.SetLaneData();
        StartCoroutine(leftarm.ExecuteLane());
        light.SetLaneData();
        StartCoroutine(light.ExecuteLane());
        if(RedLineObj != null){
            RedLineObj.SetActive(true);
            RectTransform RedLine = RedLineObj.GetComponent<RectTransform>();
            if(RedLine !=  null)
            redlineCoroutine = StartCoroutine(Redline(RedLine, redLineSpeed));
        }
        SwitchText(false);
    }

    private IEnumerator StopPlayback(float wait){
        yield return new WaitForSeconds(wait);
        StopRedline();
        SwitchText(true);
        isPlaying = false;
    }

    private float CalculateLastLaneTime(float lane1time,float lane2time,float lane3time){
        if(lane1time>lane2time&&lane1time>lane3time){
            return lane1time;
        }
        else if(lane2time>lane3time)
        return lane2time;
        else
        return lane3time;
    }

    private IEnumerator Redline(RectTransform obj, float speed){
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
    }

    private void SwitchText(bool i){
        StartButton.SetActive(i);
        StopButton.SetActive(!i);
    }
}