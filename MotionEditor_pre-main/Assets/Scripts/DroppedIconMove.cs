using UnityEngine;
using TMPro;

public class DroppedIconMove : MonoBehaviour
{
    private RectTransform droppedIconRect;
    private float widthvalue=125;
    public int num;

     private void Awake()
    {
        droppedIconRect = transform.Find("front")?.GetComponent<RectTransform>();
        GameObject inputObj = GameObject.Find("TimeInput" + num);
        TMP_InputField inputField = inputObj.GetComponent<TMP_InputField>();
        if (inputField != null){
            inputField.onValueChanged.AddListener(OnInputChanged);
        }
    }

    private void OnInputChanged(string newValue)
    {
        if (float.TryParse(newValue, out float time)){
            ChangeChildWidth(time * widthvalue);
        }
    }

    private void ChangeChildWidth(float newWidth)
    {
        if (droppedIconRect != null){
            var size = droppedIconRect.sizeDelta;
            size.x = newWidth;
            droppedIconRect.sizeDelta = size;
        }
    }
}
