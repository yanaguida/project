using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragandDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    public RectTransform[] laneRects = new RectTransform[4];
    public GameObject normalIcon;
    public GameObject droppedIcon;
    private Vector2 prevPos;
    private ScrollRect scrollRect;
    private RectTransform rectTransform;
    private RectTransform parentRectTransform;
    private Transform originalParent;
    public RectTransform stretchIcon;
    public Control control;
    public int partNumber;
    private float snapInterval = 100f;

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
        SwitchIcons(false);
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
                ChangePositionToLane(i,screenPos);
                SwitchIcons(false);
                control.RemoveFromLane(i,partNumber);
                control.AddToLane(i,partNumber);
                control.LoadLaneData(i);
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
        parentRectTransform, 
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
    rectTransform.SetParent(laneRects[laneIndex], true); 
    Vector2 localPos;
    RectTransformUtility.ScreenPointToLocalPointInRectangle(
        laneRects[laneIndex],
        dropPos,
        Camera.main,
        out localPos
    );

    localPos.y = 0f;
    localPos.x = Mathf.Round(localPos.x / snapInterval) * snapInterval;
    rectTransform.anchoredPosition = localPos;
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
            if (control.IsInLane(i, partNumber))
                control.RemoveFromLane(i, partNumber);
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


    public void ExtendRight(int lane)
    {
        if (stretchIcon != null)
        {
            Vector2 size = stretchIcon.sizeDelta;
            Vector2 pos = stretchIcon.anchoredPosition;

            size.x += 100;
            pos.x += 50; 

            stretchIcon.sizeDelta = size;
            stretchIcon.anchoredPosition = pos;

            control.RefreshLane(lane);
        }
    }

    public void ShrinkRight(int lane)
    {
        if (stretchIcon != null)
        {
            Vector2 size = stretchIcon.sizeDelta;
            Vector2 pos = stretchIcon.anchoredPosition;

            if (size.x > 100) 
            {
                size.x -= 100;
                pos.x -= 50;

                stretchIcon.sizeDelta = size;
                stretchIcon.anchoredPosition = pos;

                control.RefreshLane(lane);
            }
        }
    }
}