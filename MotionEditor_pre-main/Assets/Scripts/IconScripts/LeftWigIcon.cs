using UnityEngine;
using TMPro;

public class LeftWingIcon : IconData, IIcon
{
    public TMP_InputField inputField_Arm;

    void Start()
    {
        parttype = PartType.LeftWing;
        inputField_Arm.onValueChanged.AddListener(ReadValue);
        GameObject funcobj = GameObject.Find("Left Joint");
        if (funcobj != null)
        {
            func = funcobj.GetComponent<Ifunc>();
        }
        else Debug.Log("Left Jointがない");
        slide_area_x_r = 500f;
        slide_area_x_l = -1600f;
        slide_area_y = -600f;
    }

    protected override void SetScrollPos()
    {
        scrollRect.verticalNormalizedPosition = 0.8f;
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