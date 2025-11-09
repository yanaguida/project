using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

static class ValueBox
{
    private static float AdjustX = 0f;
    private const float startPos = 520f;
    private const float dispersec = 100f;//１秒につき１００メートル

    public static void SetAdjustX(RectTransform LaneRect)
    {
        AdjustX = LaneRect.sizeDelta.x / 2 - startPos;
    }

    public static float GetAdjustX() => AdjustX;
    public static float GetDis() => dispersec;
    public static float GetRate() => 1f / dispersec;
    public static float RoundOff(float x)
    {
        return Mathf.Round(x * 100f) / 100f;
    }
}

static class PartClassify
{
    public static string Classify(PartType parttype)
    {
        if (parttype == PartType.rightwing || parttype == PartType.leftwing || parttype == PartType.head)
            return "onefloat";
        if (parttype == PartType.lcd || parttype == PartType.singing)
            return "onestring";
        else
        {
            Debug.Log("undelared PartType");
            return "";
        }
    }
}

public enum PartType
{
    rightwing = 0,
    leftwing = 1,
    head = 2,
    lcd = 3,
    singing = 4
}

public class IconData
{
    public PartType parttype;
    public float start;
    public float time;
    public float value;

    public IconData(PartType parttype, float start, float time, float value)
    {
        this.parttype = parttype;
        if (0 <= start)
            this.start = start;
        else
            Debug.Log("start値が不正です");
        if (0 <= time)
            this.time = time;
        else
            Debug.Log("time値が不正です");
        this.value = value;
    }
}

public enum IconState
{
    OnList,
    OnLane,
    Dragged
}


public class Icons : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] protected PartType parttype;
    [SerializeField] private ScrollRect scrollRect;
    protected List<Ifunc> Func = new List<Ifunc>();
    protected Coroutine coroutine;
    public RectTransform laneRects;
    protected RectTransform IconRect;
    public RectTransform parentRectTransform;
    protected RectTransform trashRects;
    protected Vector2 prevPos = Vector2.zero;
    protected float start;
    protected float time = 4f;
    protected float value;
    private const float step = 1f;
    private float dtime = 0f;
    [SerializeField] protected IconState iconstate;
    private Vector2 CurrentPos;
    private float t = 0;
    public bool issaved = false;

    protected void Awake()
    {
        IconRect = GetComponent<RectTransform>();
        trashRects = GameObject.Find("trash").GetComponent<RectTransform>();
        Func = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<Ifunc>().ToList();
        ValueBox.SetAdjustX(laneRects);
        RectTransform currentparent = transform.parent?.GetComponentInParent<RectTransform>();
        if (currentparent == parentRectTransform)
            iconstate = IconState.OnList;
        else
            iconstate = IconState.OnLane;
    }

    void Update()
    {
        if (iconstate == IconState.Dragged)
        {
            if (CurrentPos.x >= 800f && CurrentPos.y <= -600f)
            {
                t += Time.deltaTime;
                scrollRect.horizontalNormalizedPosition += 0.001f * t;
            }
            else if (CurrentPos.x <= -1200f && CurrentPos.y <= -600f)
            {
                t += Time.deltaTime;
                scrollRect.horizontalNormalizedPosition -= 0.001f * t;
            }
            else t = 0;
        }
    }

    protected void SetStart(float x)
    {
        start = (x + dtime + ValueBox.GetAdjustX()) * ValueBox.GetRate();
    }

    public PartType GetPartType() => parttype;
    public float GetStart() => start;
    public float GetTime() => time;
    public IconState GetIconState() => iconstate;
    public float GetStartAndTime()
    {
        if (iconstate == IconState.OnLane) return start + time;
        else return 0;
    }

    public void SetData(PartType p, float s, float t, float v)
    {
        parttype = p;
        start = s;
        time = t;
        value = v;
        issaved = true;
    }

    public IconData GetData()
    {
        return new IconData(GetPartType(), GetStart(), GetTime(), GetValue());
    }

    public float GetValue() => value;

    public void Delete()
    {
        Destroy(this.gameObject);
    }

    public void ExecuteAction(float t)
    {
        if (iconstate == IconState.OnLane)
        {
            foreach (var func in Func)
            {
                if (func.CorrespondPart() == parttype)
                {
                    if (start + time <= t) return;
                    else if (start <= t)
                        coroutine = StartCoroutine(func.Action(0, time - t + start, value));
                    else
                        coroutine = StartCoroutine(func.Action(start - t, time, value));
                }
            }
        }
    }

    public void StopCoroutine()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
    }

    protected void ResetUI()
    {
        IconRect.anchoredPosition = prevPos;
        IconRect.SetParent(parentRectTransform, false);
        start = -1f;
        iconstate = IconState.OnList;
    }

    protected void SetUI(RectTransform laneRect, Vector2 dropPos)
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
        SetStart(localPos.x);
        issaved = false;
        IconRect.anchoredPosition = localPos;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        IconRect.SetParent(parentRectTransform, false);
        iconstate = IconState.Dragged;
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
        if (trashRects == null) Debug.Log("trashが存在しない");
        Vector2 screenPos = eventData.position;
        if (RectTransformUtility.RectangleContainsScreenPoint(laneRects, screenPos, eventData.pressEventCamera) && !IsOverlappingOthers())
        {
            SetUI(laneRects, screenPos);
            iconstate = IconState.OnLane;
            return;
        }
        else if (RectTransformUtility.RectangleContainsScreenPoint(trashRects, screenPos, eventData.pressEventCamera))
        {
            Destroy(this.gameObject);
            return;
        }
        else if (IsOverlappingOthers())
        {
            ResetUI();
        }
        else
            ResetUI();
    }

    public void Extend()
    {
        if (CheckRightSideDraggables())
        {
            Vector2 size = IconRect.sizeDelta;
            Vector2 pos = IconRect.anchoredPosition;
            size.x += 100;
            pos.x += 50;
            IconRect.sizeDelta = size;
            IconRect.anchoredPosition = pos;
            time += step;
            dtime -= 50f;
            issaved = false;
        }
    }

    public void Shrink()
    {
        Vector2 size = IconRect.sizeDelta;
        Vector2 pos = IconRect.anchoredPosition;
        if (size.x > 100)
        {
            size.x -= 100;
            pos.x -= 50;
            IconRect.sizeDelta = size;
            IconRect.anchoredPosition = pos;
            time -= step;
            dtime += 50f;
            issaved = false;
        }
    }

    private bool CheckRightSideDraggables()
    {
        GameObject[] draggableObjects = GameObject.FindGameObjectsWithTag("Draggable");

        foreach (GameObject targetobj in draggableObjects)
        {
            if (targetobj == this.gameObject) continue;
            float rightend = start + time;
            float leftend = 0f;
            Icons targeticon = targetobj.GetComponent<Icons>();
            if (targeticon != null && targeticon.GetPartType() == parttype)
            {
                leftend = targeticon.GetStart();
            }
            float dx = Mathf.Abs(leftend - rightend);
            if (dx <= 1f) return false;
        }
        return true;
    }

    private bool IsOverlappingOthers()
    {
        Rect thisRect = GetWorldRect(IconRect);

        // 同じタグ「Draggable」を持つ他のオブジェクトと比較
        GameObject[] others = GameObject.FindGameObjectsWithTag("Draggable");

        foreach (GameObject obj in others)
        {
            if (obj == gameObject) continue;

            RectTransform otherRect = obj.GetComponent<RectTransform>();
            if (otherRect == null) continue;

            Rect otherWorldRect = GetWorldRect(otherRect);

            if (thisRect.Overlaps(otherWorldRect))
            {
                return true;
            }
        }

        return false;
    }

    private Rect GetWorldRect(RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        Vector3 bottomLeft = corners[0];
        Vector3 topRight = corners[2];

        return new Rect(bottomLeft, topRight - bottomLeft);
    }
}