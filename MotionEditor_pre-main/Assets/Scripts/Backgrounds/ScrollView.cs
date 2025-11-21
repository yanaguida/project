using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum LaneState
{
    View,
    Edit
}

public class ScrollView : MonoBehaviour
{
    private ScrollRect scrollrect;
    private RectTransform ScrollViewRect;
    private GameObject MotionPartList;
    public GameObject ViewModeText;
    public GameObject EditModeText;
    public GameObject IconDisabler;
    public RawImage targetImage;
    public RawImage staff_notation;
    public Texture metatexture;
    public Texture evatexture;
    public Playback playback;
    public Renderer redline;
    private Vector2 prevPos;
    private Vector2 prevRec;
    private Vector2 nextPos;
    private Vector2 nextRec;
    private LaneState lanestate;
    [SerializeField] private float speed = 0.05f;

    void Start()
    {
        scrollrect = this.GetComponent<ScrollRect>();
        ScrollViewRect = this.GetComponent<RectTransform>();
        MotionPartList = GameObject.Find("MotionPartsList");
        if (MotionPartList == null) Debug.Log("ScrollViewでMotionpartsListのGameObjectがnull");
        if (ViewModeText == null) Debug.Log("ScrollViewでViewModeTextのGameObjectがnull");
        if (EditModeText == null) Debug.Log("ScrollViewでEditModeTextのGameObjectがnull");
        ViewModeText.SetActive(true);
        EditModeText.SetActive(false);
        prevPos = ScrollViewRect.anchoredPosition;
        prevRec = ScrollViewRect.sizeDelta;
        nextPos.x = 1200f;
        nextPos.y = 0f;
        nextRec.x = prevRec.x;
        nextRec.y = 1325f;
        lanestate = LaneState.Edit;
    }

    void Update()
    {
        if (playback.GetModelState() == ModelState.Stop)
        {
            if (!redline.isVisible)
            {
                scrollrect.horizontalNormalizedPosition += speed * Time.deltaTime;
            }
        }
    }

    public void OnClick(bool i)
    {
        if (lanestate == LaneState.Edit)
        {
            lanestate = LaneState.View;
            ScrollViewRect.anchoredPosition = nextPos;
            ScrollViewRect.sizeDelta = nextRec;
            MotionPartList.SetActive(false);
            ViewModeText.SetActive(false);
            EditModeText.SetActive(true);
            IconDisabler.SetActive(true);
            scrollrect.horizontalNormalizedPosition = 0f;
        }
        else if (i == true)
        {
            lanestate = LaneState.Edit;
            ScrollViewRect.anchoredPosition = prevPos;
            ScrollViewRect.sizeDelta = prevRec;
            MotionPartList.SetActive(true);
            ViewModeText.SetActive(true);
            EditModeText.SetActive(false);
            IconDisabler.SetActive(false);
        }
    }

    public void RawImageActivate(bool i)
    {
        targetImage.texture = evatexture;
        targetImage.enabled = i;
        staff_notation.enabled = !i;
    }

    public void SetMetaImage()
    {
        targetImage.texture = metatexture;
        targetImage.enabled = true;
        staff_notation.enabled = false;
    }
}
