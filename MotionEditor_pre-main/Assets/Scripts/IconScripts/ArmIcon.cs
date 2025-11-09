using UnityEngine;
using TMPro;

public class ArmIcon : Icons
{
    public TMP_InputField inputField_Arm;

    void Start()
    {
        inputField_Arm.onValueChanged.AddListener(SetValue);
    }

    private void SetValue(string targettime)
    {
        if (float.TryParse(targettime, out float x))
        {
            value = x;
            issaved = false;
        }
    }
}