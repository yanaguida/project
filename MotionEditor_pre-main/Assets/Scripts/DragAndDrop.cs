using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragandDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    public RectTransform[] laneRects = new RectTransform[4]; // 0〜3: lane1〜4
   

    public GameObject normalIcon;
    public GameObject droppedIcon;

    public Vector2 prevPos;
    private RectTransform rectTransform;
    private RectTransform parentRectTransform;
    public RectTransform contentRectTransform;
    private Transform originalParent;
    public ScrollRect scrollRect;

    public int num;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentRectTransform = rectTransform.parent as RectTransform;
        originalParent = rectTransform.parent;

        scrollRect = GameObject.Find("Scroll View").GetComponent<ScrollRect>();
    }

    private void Start()
    {
        SwitchIcons(true);
        prevPos = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        SwitchIcons(true);
        rectTransform.SetParent(originalParent, false);
    }

    public void OnDrag(PointerEventData eventData)
    {
    Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
        parentRectTransform, eventData.position, eventData.pressEventCamera, out localPos);
        rectTransform.anchoredPosition = localPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 screenPos = eventData.position;
        Vector2 localDropPos = GetLocalPosition(screenPos,eventData.pressEventCamera);

        for (int i = 0; i < laneRects.Length; i++)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(laneRects[i], screenPos, eventData.pressEventCamera))
            {
                if (!IsLaneVisible(i)) { ResetParts(); return; }

                AddToLane(i);
                RemoveFromOtherLanes(i);
                SwitchIcons(false);
                ChangePositionToLane(i,screenPos);
                Control.instance.SortLane(i);
                return;
            }
        }

        // どのレーンにも入っていない
        ResetParts();
    }

    public void OnDrop(PointerEventData eventData) { }

    private Vector2 GetLocalPosition(Vector2 screenPosition, Camera cam)
{
    RectTransformUtility.ScreenPointToLocalPointInRectangle(
        parentRectTransform, // ←ここを contentRectTransform から修正！
        screenPosition,
        cam,
        out var result
    );
    return result;
}

    private Vector2 GetLocalYFromRect(RectTransform target)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform,
            RectTransformUtility.WorldToScreenPoint(Camera.main, target.position),
            Camera.main, out var localPos);
        return localPos;
    }

    private void ChangePositionToLane(int laneIndex, Vector2 dropPos)
{
    rectTransform.SetParent(laneRects[laneIndex], false); // 親変更が先

    RectTransformUtility.ScreenPointToLocalPointInRectangle(
        laneRects[laneIndex],
        dropPos,
        Camera.main,
        out var localPos
    );

    localPos.y = 0f; // レーン内でのY位置（中央に合わせるなど）
    rectTransform.anchoredPosition = localPos;
}

    private void AddToLane(int laneIndex)
    {
        if (!Control.instance.IsInLane(laneIndex, num))
        {
            Control.instance.AddToLane(laneIndex, num);
        }
    }

    private void RemoveFromOtherLanes(int currentLane)
    {
        for (int i = 0; i < 4; i++)
        {
            if (i == currentLane) continue;
            if (Control.instance.IsInLane(i, num))
            {
                Control.instance.RemoveFromLane(i, num);
            }
        }
    }

    private bool IsLaneVisible(int laneIndex)
    {
        float threshold = laneIndex switch
        {
            0 => 0.52f,
            1 => 0.10f,
            2 => 0.85f,
            3 => 0.40f,
            _ => 0f
        };

        return laneIndex switch
        {
            0 or 1 => scrollRect.verticalNormalizedPosition >= threshold,
            2 or 3 => scrollRect.verticalNormalizedPosition <= threshold,
            _ => true
        };
    }

    private void ResetParts()
    {
        for (int i = 0; i < 4; i++)
        {
            if (Control.instance.IsInLane(i, num))
                Control.instance.RemoveFromLane(i, num);
        }
        rectTransform.anchoredPosition = prevPos;
        rectTransform.SetParent(originalParent, false);
        SwitchIcons(true);
    }

    private void SwitchIcons(bool isDropped)
    {
        normalIcon.SetActive(isDropped);
        droppedIcon.SetActive(!isDropped);
    }
}