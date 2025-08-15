using UnityEngine;
using System.Collections;

public class Lanereader : MonoBehaviour
{
    public int laneIndex;
    public float terminalPos;
    private int WaitNum = 0;
    private const float TIME_PER_PIXEL = 0.005f;
    public float time;
    public Control control;
    private RectTransform lanelength;

    private void Awake(){
        GameObject pivot0 = GameObject.Find("pivot0");
        if(pivot0 != null){
            RectTransform pivotRect = pivot0.GetComponent<RectTransform>();
            if (pivotRect != null){
                terminalPos = pivotRect.anchoredPosition.x;
            }
        }
        lanelength = GetComponent<RectTransform>();
    }

    public void FillVoidWithWait(GameObject droppedIcon){
        if(!CheckLeftSpace(droppedIcon)){
           control.AddToLane(laneIndex,0);
           RectTransform rect = droppedIcon.GetComponent<RectTransform>();
            if (rect != null){
                float left = rect.anchoredPosition.x+5500;
                float right = left + (rect.sizeDelta.x * 0.5f);
                time = left*TIME_PER_PIXEL;
                terminalPos = right;
            }
            control.UpdateInputData(laneIndex,0, time, WaitNum);
            Debug.Log(time);
        }
    }

    private bool CheckLeftSpace(GameObject droppedIcon){
        RectTransform IconRect = droppedIcon.GetComponent<RectTransform>();
       if (IconRect != null){
            float iconLeft = IconRect.anchoredPosition.x - (IconRect.rect.width * 0.5f);
            return iconLeft > terminalPos;
        }
        else
        return false;
    }
}
