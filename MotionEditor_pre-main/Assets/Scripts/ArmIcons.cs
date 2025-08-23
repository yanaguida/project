using UnityEngine;
using TMPro;

public class ArmIcons : Icons, IFlexible
{
    public void flex(){
        ExtendRight();
        ShrinkRight();
    }

    public void ExtendRight(){
        Vector2 size = IconRect.sizeDelta;
        Vector2 pos = IconRect.anchoredPosition;
        size.x += 100;
        pos.x += 50; 
        IconRect.sizeDelta = size;
        IconRect.anchoredPosition = pos;
    }

    public void ShrinkRight(){
        Vector2 size = IconRect.sizeDelta;
        Vector2 pos = IconRect.anchoredPosition;
        if (size.x > 100) {
            size.x -= 100;
            pos.x -= 50;
            IconRect.sizeDelta = size;
            IconRect.anchoredPosition = pos;
        }
    }
}