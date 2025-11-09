using UnityEngine;
using TMPro;
using System.Collections.Generic;

public abstract class FileAbstract : MonoBehaviour
{
    public GameObject screen;
    public TMP_InputField inputField;
    protected List<Icons> IconList = new List<Icons>();
    protected string fileName;

    public virtual void OnClick()
    {
        fileName = inputField.text;
        if (string.IsNullOrEmpty(fileName))
        {
            Debug.LogWarning("ファイル名が入力されていません。");
            return;
        }
    }

    public void OnClickActivate(bool i) => screen.SetActive(i);

    public void TextReset() => inputField.text = string.Empty;
}

