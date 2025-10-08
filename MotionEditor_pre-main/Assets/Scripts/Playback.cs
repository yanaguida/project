using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum armKind{Right,Left,Head}
public enum stringKind{LED,Music}

public class Playback : MonoBehaviour
{
    public Motors rightmotor;
    public Motors leftmotor;
    public LED led;
    public Audio music;
    public GameObject StartButton;
    public GameObject StopButton;
    public GameObject RedLineObj;
    private Coroutine redlineCoroutine;
    private bool isPlaying = false;
    private float redLineSpeed = 100f;
    private List<ILane> allLanes = new List<ILane>();

    public void Awake(){
        SwitchText(true);
        allLanes = new List<ILane>(FindObjectsOfType<MonoBehaviour>().OfType<ILane>());
        foreach (var lane in allLanes){
            if (lane is ArmLane armLane){
                if(armLane.armkind == armKind.Right) armLane.motorScript = rightmotor;
                else if(armLane.armkind == armKind.Left) armLane.motorScript = leftmotor;
                else Debug.Log("ArmLane型laneの取得に失敗");
            }
            else if (lane is SelectLane selectLane){
                if(selectLane.stringkind == stringKind.LED) selectLane.ledScript = led;               
                else if(selectLane.stringkind == stringKind.Music) selectLane.musicScript = music;
                else Debug.Log("SelectLane型laneの取得に失敗");
            }
        }
    }

    public void OnClick(){
        if (isPlaying) return;
        StartPlayback();
        StartCoroutine(StopPlayback(CalculateLastLaneTime()));
    }

    private void StartPlayback(){
        isPlaying = true;
        foreach (var lane in allLanes){
            lane.SetLaneData();
            StartCoroutine(lane.ExecuteLane());
        }
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

    private float CalculateLastLaneTime(){
        float maxTime = 0f;
        foreach (var lane in allLanes){
            maxTime = Mathf.Max(maxTime, lane.GetTotalTime());
        }
        return maxTime;
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