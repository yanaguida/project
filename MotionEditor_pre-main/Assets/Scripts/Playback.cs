using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public enum armKind{Right,Left,Head}
public enum stringKind{LED,Music}

public class Playback : MonoBehaviour
{
    public Motors rightmotor;
    public Motors leftmotor;
    public LED led;
    public Audio music;
    public Rechord_Gear rechord_gear;
    public Redline redline;
    public GameObject StartText;
    public GameObject StopText;
    private Button targetButton;
    private ColorBlock cb;
    private Coroutine resetCoroutine;
    private Coroutine redlineCoroutine;
    private Coroutine gearCoroutine;
    private List<Coroutine> functionCoroutine = new List<Coroutine>();
    private bool isPlaying = false;
    private List<ILane> allLanes = new List<ILane>();

    public void Awake(){
        targetButton = GetComponent<Button>();
        cb = targetButton.colors;
        SwitchText(true);
        SwitchColor(true);
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
        if (isPlaying) StopPlayback();
        else{
            StartPlayback();
            resetCoroutine = StartCoroutine(Reset(CalculateLastLaneTime()));
        }
    }

    private void StartPlayback(){
        isPlaying = true;
        foreach (var lane in allLanes){
            lane.SetLaneData();
            functionCoroutine.Add(StartCoroutine(lane.ExecuteLane()));
        }

        Debug.Log(functionCoroutine[0]);

        if(redline==null) Debug.Log("redlineがnull");
        else redlineCoroutine = StartCoroutine(redline.StartRedline());

        if(rechord_gear==null) Debug.Log("rechord_gearがnull");
        else gearCoroutine = StartCoroutine(rechord_gear.RotateGear());

        SwitchText(false);
        SwitchColor(false);
    }

    private IEnumerator Reset(float wait){
        yield return new WaitForSeconds(wait);
        redline.ResetRedline(redlineCoroutine);
        StopCoroutine(gearCoroutine);
        SwitchText(true);
        SwitchColor(true);
        isPlaying = false;
    }

    private void StopPlayback(){
        isPlaying = false;
        StopCoroutine(gearCoroutine);
        StopCoroutine(redlineCoroutine);
        StopCoroutine(resetCoroutine);
        foreach (var func in functionCoroutine){
            StopCoroutine(func);
        }
        SwitchText(true);
        SwitchColor(true);
    }

    private float CalculateLastLaneTime(){
        float maxTime = 0f;
        foreach (var lane in allLanes){
            maxTime = Mathf.Max(maxTime, lane.GetTotalTime());
        }
        return maxTime;
    }

    private void SwitchText(bool i){
        StartText.SetActive(i);
        StopText.SetActive(!i);
    }

    private void SwitchColor(bool i){
        if(i){
            cb.normalColor = new Color(0.78f, 0.90f, 0.98f);
            cb.highlightedColor = new Color(0.5f, 0.7f, 1.0f);
            cb.pressedColor = new Color(0.5f, 0.7f, 1.0f);
            cb.selectedColor = new Color(0.5f, 0.7f, 1.0f);
            cb.disabledColor = new Color(0.5f, 0.7f, 1.0f);
            targetButton.colors = cb;
        }
        else{
            cb.normalColor = new Color(1f, 0.75f, 0.71f);
            cb.highlightedColor = new Color(1f, 0.75f, 0.71f);
            cb.pressedColor = new Color(1f, 0.75f, 0.71f);
            cb.selectedColor = new Color(1f, 0.75f, 0.71f);
            cb.disabledColor = new Color(1f, 0.75f, 0.71f);
            targetButton.colors = cb;
        }
    }
}