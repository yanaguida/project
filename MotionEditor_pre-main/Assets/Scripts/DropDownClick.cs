using UnityEngine;
using UnityEngine.EventSystems;

public class DropDownClick : MonoBehaviour, IPointerClickHandler
{

    public int num; //←←←partNによって変更する、オブジェクト側で入力可能

    public void OnPointerClick(PointerEventData eventData)
    {
        Control.instance.SetChoiceParts(num);
    }
}