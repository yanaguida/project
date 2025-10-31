using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

static class ValueBox
{
    private static float AdjustX = 0f;
    private const float startPos = 520f;
    private const float dispersec = 100f;//１秒につき１００メートル

    public static void SetAdjustX(RectTransform LaneRect){
        AdjustX = LaneRect.sizeDelta.x/2-startPos;
    }

    public static float GetAdjustX() => AdjustX;
    public static float GetDis() => dispersec;
    public static float GetRate() => 1f/dispersec;
}


public abstract class Icons : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private ScrollRect scrollRect;
    public RectTransform laneRects;
    protected RectTransform IconRect;
    public RectTransform parentRectTransform;
    protected RectTransform trashRects;
    protected Vector2 prevPos = Vector2.zero;
    public Transform originalParent;
    protected float start;
    protected float time = 4f;
    private const float step = 1f;
    private float dtime = 0f;
    private bool isdrag = false;
    private Vector2 CurrentPos;
    private float t=0;

    protected void Awake(){
        IconRect = GetComponent<RectTransform>();
        trashRects = GameObject.Find("trash").GetComponent<RectTransform>();
        ValueBox.SetAdjustX(laneRects);
    }

    void Update(){
        if(isdrag){
            if(CurrentPos.x>=800f&&CurrentPos.y<=-600f){
                t += Time.deltaTime;
                scrollRect.horizontalNormalizedPosition += 0.001f*t;
            }
            else if(CurrentPos.x<=-1200f&&CurrentPos.y<=-600f){
                t += Time.deltaTime;
                scrollRect.horizontalNormalizedPosition -= 0.001f*t;
            }
            else t = 0;
        }
    }

    protected void SetStart(float x){
        start = (x+dtime+ValueBox.GetAdjustX())*ValueBox.GetRate();
    }

    public float GetStart() => start;

    public float GetTime() => time;

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
        //ｘ軸に対して自由配置
        localPos.x = Mathf.Round(localPos.x);
        //グリッド線にスナップ
        //localPos.x = Mathf.Round(localPos.x/ValueBox.GetDis()) * ValueBox.GetDis()+20f;
        SetStart(localPos.x);
        IconRect.anchoredPosition = localPos;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        IconRect.SetParent(originalParent, false);
        isdrag = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
        parentRectTransform, eventData.position, eventData.pressEventCamera, out localPos);
        IconRect.anchoredPosition = localPos;
        CurrentPos = localPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isdrag = false;
        if(trashRects==null) Debug.Log("trashが存在しない");
        Vector2 screenPos = eventData.position;
        if (RectTransformUtility.RectangleContainsScreenPoint(laneRects, screenPos, eventData.pressEventCamera))
        {
            SetUI(laneRects,screenPos);
            return;
        }
        else if(RectTransformUtility.RectangleContainsScreenPoint(trashRects, screenPos, eventData.pressEventCamera)){
            Destroy(this.gameObject);
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
            if (targeticon != null && targeticon.GetParent() == IconRect.parent){
                leftend = targeticon.GetStart();
            }
            float dx = Mathf.Abs(leftend - rightend);
            if (dx<=1f)  return false;
        }
        return true;
    }

    public Transform GetParent(){
        return IconRect.parent;
    }
}