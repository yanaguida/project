using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public abstract class FileAbstract : MonoBehaviour
{
    public GameObject screen;
    public TMP_InputField inputField;
    protected List<ILane> allLanes = new List<ILane>();
    protected string fileName;

    void Awake()
    {
        // ILaneを継承しているMonoBehaviourをすべて取得
        allLanes = FindObjectsOfType<MonoBehaviour>().OfType<ILane>().ToList();
    }

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

