using UnityEngine;
using TMPro;

public class SingingIcon : IconData, IIcon
{
    public TMP_Dropdown inputField_Arm;

    void Start()
    {
        parttype = PartType.Singing;
        inputField_Arm.onValueChanged.AddListener(ReadValue);
        GameObject funcobj = GameObject.Find("mouse");
        if (funcobj != null)
        {
            func = funcobj.GetComponent<Ifunc>();
        }
        else Debug.Log("mouseがない");
        slide_area_x_r = 500f;
        slide_area_x_l = -1600f;
        slide_area_y = -200f;
    }

    protected override void SetScrollPos()
    {
        scrollRect.verticalNormalizedPosition = 0f;
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
