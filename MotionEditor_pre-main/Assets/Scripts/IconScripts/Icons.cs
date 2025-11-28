using UnityEngine;
using UnityEngine.UI;

static class ValueBox
{
    private const float dispersec = 500f;//１秒につき１００メートル

    public static float GetDis() => dispersec;
    public static float GetRate() => 1f / dispersec;
}

public enum PartType
{
    RightWing = 0,
    LeftWing = 1,
    Head = 2,
    LCD = 3,
    Singing = 4
}

public enum IconState
{
    OnList,
    OnLane,
    Dragged
}


public class IconData : UISetting
{
    [SerializeField] protected ScrollRect scrollRect;
    protected PartType parttype;
    protected float start;
    protected float time;
    protected float value;
    protected Ifunc func;
    private Coroutine coroutine;
    private float t = 0;
    protected float slide_area_x_r;
    protected float slide_area_x_l;
    protected float slide_area_y;

    protected override void Awake()
    {
        base.Awake();
        IconRect = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (iconstate == IconState.Dragged)
        {
            if (IconRect.anchoredPosition.x >= slide_area_x_r && IconRect.anchoredPosition.y <= slide_area_y)
            {
                t += Time.deltaTime;
                scrollRect.horizontalNormalizedPosition += 0.001f * t;
            }
            else if (IconRect.anchoredPosition.x <= slide_area_x_l && IconRect.anchoredPosition.y <= slide_area_y)
            {
                t += Time.deltaTime;
                scrollRect.horizontalNormalizedPosition -= 0.001f * t;
            }
            else t = 0;
        }
    }

    private void SetStart()
    {
        start = (IconRect.anchoredPosition.x - IconRect.sizeDelta.x / 2f
         + laneRects.sizeDelta.x / 2f) * ValueBox.GetRate();
        if (start < 0)
            start = 0;
        start = Mathf.Round(start);
    }

    private void SetTime()
    {
        time = IconRect.sizeDelta.x * ValueBox.GetRate();
        time = Mathf.Round(time * 100f) / 100f;
    }

    protected virtual void SetValue() { }
    public float GetStart() => start;
    public float GetTime() => time;
    public float GetValue() => value;

    public RectTransform GetRT() => IconRect;

    public PartType GetPartType() => parttype;

    public IconState GetIconState() => iconstate;

    public bool GetIssaved() => issaved;

    public RectTransform GetLaneRT() => laneRects;

    public void SetIssaved(bool i)
    {
        issaved = i;
    }

    public void SetData()
    {
        SetStart();
        SetTime();
        SetValue();
    }

    public void Delete()
    {
        Destroy(this.gameObject);
    }

    public void ExecuteAction(float t)
    {
        SetData();
        if (iconstate == IconState.OnLane)
        {
            if (start + time <= t) return;
            else if (start <= t)
                coroutine = StartCoroutine(func.Action(0, time - t + start, value));
            else
                coroutine = StartCoroutine(func.Action(start - t, time, value));
        }
    }

    public void StopCoroutine()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
    }
}