using UnityEngine;
using TMPro;

public class SelectMusicPartData
{
    public float start;
    public float time;
    public string music;

    public SelectMusicPartData(float start, float time, string music){
        if(0<=start)
        this.start = start;
        else
        Debug.Log("start値が不正です");
        if(0<time)
        this.time = time;
        else
        Debug.Log("time値が不正です");
        this.music = music;
    }
}

public class SelectMusicIcon : Icons
{
    public TMP_Dropdown dropdown;
    private string music;

    private void Start(){
        dropdown.onValueChanged.AddListener(SetMusic);
        SetMusic(dropdown.value);
    }

    private void SetMusic(int index)
    {
        string selectedText = dropdown.options[dropdown.value].text;
        music = selectedText;
    }

    public string GetMusic(){
        return music;
    }
}
