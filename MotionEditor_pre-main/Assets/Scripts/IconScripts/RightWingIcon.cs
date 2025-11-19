using UnityEngine;
using TMPro;

public interface IIcon
{
    public PartType GetPartType();
    public RectTransform GetRT();
    public IconState GetIconState();
    public bool GetIssaved();
    public void Delete();
    public void ExecuteAction(float t);
    public void StopCoroutine();
    public void SetData();
    public float GetStart();
    public float GetTime();
    public float GetValue();
    public void SetIssaved(bool i);
    public RectTransform GetLaneRT();
}

public class RightWingIcon : IconData, IIcon
{
    public TMP_InputField inputField_Arm;

    void Start()
    {
        parttype = PartType.RightWing;
        inputField_Arm.onValueChanged.AddListener(ReadValue);
        GameObject funcobj = GameObject.Find("Right Joint");
        if (funcobj != null)
        {
            func = funcobj.GetComponent<Ifunc>();
        }
        else Debug.Log("Right Jointがない");
        slide_area_x_r = 800f;
        slide_area_x_l = -1200f;
        slide_area_y = -600f;
    }

    protected override void SetScrollPos()
    {
        scrollRect.verticalNormalizedPosition = 1.0f;
    }

    private void ReadValue(string targettime)
    {
        if (float.TryParse(targettime, out float x))
        {
            value = x;
            issaved = false;
        }
    }

    protected override void SetValue()
    {
        if (float.TryParse(inputField_Arm.text, out float x))
            value = x;
    }
}
