using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Playback : MonoBehaviour
{
    public Functions funScript; 
    public GameObject StartButton;
    public GameObject StopButton;
    public GameObject RedLineObj;
    public Lane1 lane1;
    public Lane2 lane2;
    private Coroutine redlineCoroutine;
    private bool isPlaying = false;
    private float redLineSpeed = 200f;

    public void Awake(){
        SwitchText(true);
    }

    public void OnClick(){
        if (isPlaying) return;
        StartPlayback();
        StartCoroutine(StopPlayback(CalculateLastLaneTime(lane1.GetTotalTime(),lane2.GetTotalTime())));
    }

    private void StartPlayback(){
        isPlaying = true;
        lane1.SetLaneData();
        StartCoroutine(lane1.ExecuteLane());
        lane2.SetLaneData();
        StartCoroutine(lane2.ExecuteLane());
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

    private float CalculateLastLaneTime(float lane1time,float lane2time){
        if(lane1time>lane2time){
            return lane1time;
        }
        else
        return lane2time;
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