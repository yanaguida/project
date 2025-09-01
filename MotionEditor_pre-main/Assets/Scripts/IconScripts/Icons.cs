using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class Icons : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public RectTransform laneRects;
    protected RectTransform IconRect;
    protected RectTransform parentRectTransform;
    protected Vector2 prevPos;
    protected Transform originalParent;
    protected float snapInterval;
    protected const float adjustX = 5480;
    protected const float distanceRate = 0.005f;
    protected float start;
    protected float time;
    private const float defaulttime = 2f;
    private const float step = 0.5f;
    private float dtime = 20f;

    protected void Awake(){
        IconRect = GetComponent<RectTransform>();
        parentRectTransform = IconRect.parent as RectTransform;
        prevPos = IconRect.anchoredPosition;
        originalParent = IconRect.parent;
        snapInterval = 100f;
        time = defaulttime;
    }

    protected void SetStart(float x){
        start = (x+dtime-20f+adjustX)*distanceRate;
    }

    public float GetStart(){
        return start;
    }

     public float GetTime(){
        return time;
    }

    protected void ResetUI(){
        IconRect.anchoredPosition = prevPos;
        IconRect.SetParent(originalParent, false);
        start = -1f;
    }

    protected void SetUI(RectTransform laneRect, Vector2 dropPos){
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

    public void OnBeginDrag(PointerEventData eventData)
    {
        IconRect.SetParent(originalParent, false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
        parentRectTransform, eventData.position, eventData.pressEventCamera, out localPos);
        IconRect.anchoredPosition = localPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 screenPos = eventData.position;
        if (RectTransformUtility.RectangleContainsScreenPoint(laneRects, screenPos, eventData.pressEventCamera))
        {
            SetUI(laneRects,screenPos);
            return;
        }
        else
        ResetUI();
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
    }
}