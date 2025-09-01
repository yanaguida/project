using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UIDraggableObject : MonoBehaviour, IEndDragHandler
{
    private Vector3 originalPosition;
    private RectTransform myRect;
    private Transform originalParent;
    private Canvas canvas;

    void Awake()
    {
        myRect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        originalParent = myRect.parent;
        originalPosition = Vector3.zero;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (IsOverlappingOthers())
        {
            myRect.SetParent(originalParent, false);
            transform.localPosition = originalPosition;
        }
    }

    private bool IsOverlappingOthers()
    {
        Rect thisRect = GetWorldRect(myRect);

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