using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class Icons : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public RectTransform laneRects;
    protected RectTransform IconRect;
    protected RectTransform parentRectTransform;
    private Vector2 prevPos;
    protected Transform originalParent;
    private float snapInterval;

    protected void Awake(){
        IconRect = GetComponent<RectTransform>();
        parentRectTransform = IconRect.parent as RectTransform;
        prevPos = IconRect.anchoredPosition;
        originalParent = IconRect.parent;
        snapInterval = 100f;
    }

    protected void ResetUI(){
        IconRect.anchoredPosition = prevPos;
        IconRect.SetParent(originalParent, false);
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
        localPos.x = Mathf.Round(localPos.x / snapInterval) * snapInterval+20f;
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
}

public interface IFlexible{
    void flex();
}