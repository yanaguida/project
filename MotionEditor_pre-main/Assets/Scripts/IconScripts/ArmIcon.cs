using UnityEngine;
using TMPro;

public class ArmData
{
    public float start;
    public float time;
    public float value;

    public ArmData(float start, float time, float value)
    {
        if(0 <= start)
        this.start = start;
        else
        Debug.Log("start値が不正です");
        if(0 <= time)
        this.time = time;
        else
        Debug.Log("time値が不正です");
        if(0<=value&&value<=360)
        this.value = value;
        else
        Debug.Log("value値が不正です");
    }
}

public class ArmIcon : Icons
{
    private float value;
    public TMP_InputField inputField_Arm;

    void Start()
    {
        inputField_Arm.onValueChanged.AddListener(SetValue);
    }

    public float GetValue(){
        return value;
    }

    public void SetValue(string targettime){
        if (float.TryParse(targettime, out float x))
        {
            value = x;
        }
    }
}