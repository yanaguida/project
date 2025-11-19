using UnityEngine;
using TMPro;

public class LCDIcon : IconData, IIcon
{
    public TMP_Dropdown inputField_Arm;

    void Start()
    {
        parttype = PartType.LCD;
        inputField_Arm.onValueChanged.AddListener(ReadValue);
        GameObject funcobj = GameObject.Find("eyes");
        if (funcobj != null)
        {
            func = funcobj.GetComponent<Ifunc>();
        }
        else Debug.Log("eyesがない");
        slide_area_x_r = 800f;
        slide_area_x_l = -1200f;
        slide_area_y = -200f;
    }

    protected override void SetScrollPos()
    {
        scrollRect.verticalNormalizedPosition = 0.2f;
    }

    private void ReadValue(int index)
    {
        value = index;
        issaved = false;
    }

    protected override void SetValue()
    {
        value = inputField_Arm.value;
    }
}
