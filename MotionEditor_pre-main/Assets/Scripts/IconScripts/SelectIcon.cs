using UnityEngine;
using TMPro;

public class SelectIcon : Icons
{
    public TMP_Dropdown dropdown;
    private string emotion;

    private void Start(){
        dropdown.onValueChanged.AddListener(SetEmotion);
        SetEmotion(dropdown.value);
    }

    private void SetEmotion(int index)
    {
        string selectedText = dropdown.options[dropdown.value].text;
        emotion = selectedText;
    }

    public string GetEmotion(){
        return emotion;
    }
}
