using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;

public class UISetting : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
    [SerializeField] protected IconState iconstate;
    private RectTransform startline;
    private RectTransform trashRects;
    public RectTransform laneRects;
    protected RectTransform IconRect;
    public RectTransform parentRectTransform;
    public bool issaved = false;

    protected virtual void Awake()
    {
        startline = GameObject.Find("startline").GetComponent<RectTransform>();
        trashRects = GameObject.Find("trash").GetComponent<RectTransform>();
        if (startline == null) Debug.Log("startlineがnull");
        if (trashRects == null) Debug.Log("trashがnull");
        if (laneRects == null) Debug.Log("laneRectsがnull");
        if (parentRectTransform == null) Debug.Log("parentRectTransformがnull");
        IconRect = GetComponent<RectTransform>();
        RectTransform currentparent = transform.parent?.GetComponentInParent<RectTransform>();
        if (currentparent == parentRectTransform)
            iconstate = IconState.OnList;
        else
            iconstate = IconState.OnLane;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        IconRect.SetParent(parentRectTransform, false);
        if (iconstate == IconState.OnList)
        {
            SetScrollPos();
        }
        iconstate = IconState.Dragged;
    }

    protected virtual void SetScrollPos() { }

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
        if (RectTransformUtility.RectangleContainsScreenPoint(trashRects, screenPos, eventData.pressEventCamera))
        {
            Destroy(this.gameObject);
            return;
        }
        else if (RectOverlaps(startline, IconRect))
        {
            ResetUI();
            return;
        }
        else if (RectTransformUtility.RectangleContainsScreenPoint(laneRects, screenPos, eventData.pressEventCamera))
        {
            bool overlapped = false;
            List<IIcon> IconList = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IIcon>().ToList();

            foreach (var icons in IconList)
            {
                RectTransform IconRT = icons.GetRT();
                if (IconRT == IconRect) continue;

                if (RectOverlaps(IconRT, IconRect))
                {
                    overlapped = true;
                    break;
                }
            }

            if (overlapped)
                ResetUI();
            else
                SetUI(laneRects, screenPos);
        }
        else
            ResetUI();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (iconstate == IconState.OnLane)
        {
            // クリックされた位置をローカル座標に変換
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                IconRect,  // 自身のRectTransform
                eventData.position,
                eventData.pressEventCamera,
                out localPoint
            );

            // 中心より左か右かで伸縮方向を決定
            if (localPoint.x < 0) // 左半分クリック
                Shrink();
            else // 右半分クリック
                Extend();
        }
    }

    private void Extend()
    {
        List<IIcon> IconList = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IIcon>().ToList();
        Vector2 size = IconRect.sizeDelta;
        Vector2 pos = IconRect.anchoredPosition;
        bool overlapped = false;
        size.x += 100;
        pos.x += 50;
        IconRect.sizeDelta = size;
        IconRect.anchoredPosition = pos;
        foreach (var icon in IconList)
        {
            RectTransform IconRT = icon.GetRT();
            if (IconRT == IconRect) continue;
            if (RectOverlaps(IconRT, IconRect))
            {
                overlapped = true;
                break;
            }
        }
        if (overlapped)
        {
            size.x -= 100;
            pos.x -= 50;
            IconRect.sizeDelta = size;
            IconRect.anchoredPosition = pos;
        }
        else
        {
            issaved = false;
        }
    }

    private void Shrink()
    {
        Vector2 size = IconRect.sizeDelta;
        Vector2 pos = IconRect.anchoredPosition;
        if (size.x > 100)
        {
            size.x -= 100;
            pos.x -= 50;
            IconRect.sizeDelta = size;
            IconRect.anchoredPosition = pos;
            issaved = false;
        }
    }


    private void SetUI(RectTransform laneRect, Vector2 dropPos)
    {
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
        issaved = false;
        IconRect.anchoredPosition = localPos;
        iconstate = IconState.OnLane;
    }

    private void ResetUI()
    {
        IconRect.anchoredPosition = Vector2.zero;
        IconRect.SetParent(parentRectTransform, false);
        iconstate = IconState.OnList;
    }

    private static bool RectOverlaps(RectTransform a, RectTransform b)
    {
        Rect rectA = GetWorldRect(a);
        Rect rectB = GetWorldRect(b);

        rectA.xMin += 0.01f;
        rectA.xMax -= 0.01f;
        rectB.xMin += 0.01f;
        rectB.xMax -= 0.01f;

        return rectA.Overlaps(rectB);
    }

    private static Rect GetWorldRect(RectTransform rt)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);

        // 左下基準
        Vector2 position = corners[0];
        // 幅と高さ
        float width = corners[2].x - corners[0].x;
        float height = corners[2].y - corners[0].y;

        return new Rect(position, new Vector2(width, height));
    }
}
