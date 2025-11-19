using UnityEngine;
using TMPro;

public class HeadIcon : IconData, IIcon
{
    public TMP_InputField inputField_Arm;

    void Start()
    {
        parttype = PartType.Head;
        inputField_Arm.onValueChanged.AddListener(ReadValue);
        GameObject funcobj = GameObject.Find("Head");
        if (funcobj != null)
        {
            func = funcobj.GetComponent<Ifunc>();
        }
        else Debug.Log("Headがない");
        slide_area_x_r = 100f;
        slide_area_x_l = -2000f;
        slide_area_y = -600f;
    }


    protected override void SetScrollPos()
    {
        scrollRect.verticalNormalizedPosition = 0.5f;
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
