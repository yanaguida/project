using UnityEngine;
using TMPro;

public class SelectPartData
{
    public float start;
    public float time;
    public string emotion;

    public SelectPartData(float start, float time, string emotion){
        if(0<=start)
        this.start = start;
        else
        Debug.Log("start値が不正です");
        if(0<time)
        this.time = time;
        else
        Debug.Log("time値が不正です");
        this.emotion = emotion;
    }
}

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
