using UnityEngine;
using TMPro;

public class SelectIcon : Icons
{
    public TMP_Dropdown dropdown;

    private void Start()
    {
        dropdown.onValueChanged.AddListener(SetEmotion);
    }

    private void SetEmotion(int index)
    {
        value = index;
        issaved = false;
    }
}
