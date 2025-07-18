using UnityEngine;
using UnityEngine.EventSystems;

public class DropDownClick : MonoBehaviour, IPointerClickHandler
{

    public int num; //←←←partNによって変更する、オブジェクト側で入力可能

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Control.instance.SetCPfromClick(num);
    }
}
