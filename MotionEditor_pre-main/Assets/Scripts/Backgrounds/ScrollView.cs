using Unity.VisualScripting;
using UnityEngine;

public class ScrollView : MonoBehaviour
{
    private RectTransform ScrollViewRect;
    private GameObject MotionPartList;
    public GameObject ViewModeText;
    public GameObject EditModeText;
    private Vector2 prevPos;
    private Vector2 prevRec;
    private Vector2 nextPos;
    private Vector2 nextRec;
    private int i = 0;

    void Start()
    {
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
    }

    public void OnClick()
    {
        if (i % 2 == 0)
        {
            ScrollViewRect.anchoredPosition = nextPos;
            ScrollViewRect.sizeDelta = nextRec;
            MotionPartList.SetActive(false);
            ViewModeText.SetActive(false);
            EditModeText.SetActive(true);
        }
        else
        {
            ScrollViewRect.anchoredPosition = prevPos;
            ScrollViewRect.sizeDelta = prevRec;
            MotionPartList.SetActive(true);
            ViewModeText.SetActive(true);
            EditModeText.SetActive(false);
        }
        i++;
    }
}
