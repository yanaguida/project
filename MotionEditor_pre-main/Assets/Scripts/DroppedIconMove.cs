using UnityEngine;
using TMPro;

public class DroppedIconMove : MonoBehaviour
{
    private RectTransform droppedIconRect;
    public int partNumber;

     private void Awake()
    {
        droppedIconRect = transform.Find("DroppedIcon"+partNumber)?.GetComponent<RectTransform>();
    }

    public void ExtendLeft()
    {
        if (droppedIconRect != null)
        {
            Vector2 size = droppedIconRect.sizeDelta;
            Vector2 pos = droppedIconRect.anchoredPosition;

            size.x += 100;
            pos.x -= 50; 

            droppedIconRect.sizeDelta = size;
            droppedIconRect.anchoredPosition = pos;
        }
    }

    public void ExtendRight()
    {
        if (droppedIconRect != null)
        {
            Vector2 size = droppedIconRect.sizeDelta;
            Vector2 pos = droppedIconRect.anchoredPosition;

            size.x += 100;
            pos.x += 50; 

            droppedIconRect.sizeDelta = size;
            droppedIconRect.anchoredPosition = pos;
        }
    }

    public void ShrinkLeft()
    {
        if (droppedIconRect != null)
        {
            Vector2 size = droppedIconRect.sizeDelta;
            Vector2 pos = droppedIconRect.anchoredPosition;

            if (size.x > 100)
            {
                size.x -= 100;
                pos.x += 50;

                droppedIconRect.sizeDelta = size;
                droppedIconRect.anchoredPosition = pos;
            }
        }
    }

    public void ShrinkRight()
    {
        if (droppedIconRect != null)
        {
            Vector2 size = droppedIconRect.sizeDelta;
            Vector2 pos = droppedIconRect.anchoredPosition;

            if (size.x > 100) 
            {
                size.x -= 100;
                pos.x -= 50;

                droppedIconRect.sizeDelta = size;
                droppedIconRect.anchoredPosition = pos;
            }
        }
    }
}