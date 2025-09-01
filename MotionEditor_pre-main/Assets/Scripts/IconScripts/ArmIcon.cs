using UnityEngine;
using TMPro;

public class ArmIcon : Icons
{
    private float value;
    public TMP_InputField inputField_Arm;

    void Start()
    {
        inputField_Arm.onValueChanged.AddListener(SetValue);
    }

    public float GetValue(){
        return value;
    }

    public void SetValue(string targettime){
        if (float.TryParse(targettime, out float x))
        {
            value = x;
        }
    }
/*
    protected override void SetUI(RectTransform laneRect, Vector2 dropPos){
        IconRect.SetParent(laneRect, false);
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            laneRect,
            dropPos,
            Camera.main,
            out localPos
        );
        localPos.y = 0f;
        localPos.x = Mathf.Round(localPos.x / snapInterval) * snapInterval+dtime;
        SetStart(localPos.x);
        IconRect.anchoredPosition = localPos;
    }

    protected override void SetStart(float x){
        start = (x+dtime-20f+adjustX)*distanceRate;
    }


    public void Extend(){
        if(CheckRightSideDraggables()){
            Vector2 size = IconRect.sizeDelta;
            Vector2 pos = IconRect.anchoredPosition;
            size.x += 100;
            pos.x += 50; 
            IconRect.sizeDelta = size;
            IconRect.anchoredPosition = pos;
            time += step;
            dtime -= 50f;
        }
    }

    public void Shrink(){
        Vector2 size = IconRect.sizeDelta;
        Vector2 pos = IconRect.anchoredPosition;
        if (size.x > 100) {
            size.x -= 100;
            pos.x -= 50;
            IconRect.sizeDelta = size;
            IconRect.anchoredPosition = pos;
            time -= step;
            dtime += 50f;
        }
    }

    private bool CheckRightSideDraggables()
    {
        GameObject[] draggableObjects = GameObject.FindGameObjectsWithTag("Draggable");

        foreach (GameObject targetobj in draggableObjects)
        {
            if (targetobj == this.gameObject) continue; 
            float rightend = start+time;
            float leftend = 0f;
            Icons targeticon = targetobj.GetComponent<Icons>();
            if (targeticon != null){
                leftend = targeticon.GetStart();
            }
            float dx = leftend - rightend;
            if (dx==0)
            {
                return false;
            }
        }
        return true;
    }*/
}