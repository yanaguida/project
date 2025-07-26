using UnityEngine;
using System;
using System.IO;
using TMPro;

public class FileOutput : MonoBehaviour
{
    public TMP_InputField inputField;
    string fileName;

    public void OnClick()
    {
        fileName = inputField.text;
        // GetValuesから値を受け取り、ファイル出力をする
        // Lane1_partsNumber : *, *, *, *
        // values : *, *, *, *
        string lane1set = "Lane1_partsNumber : " + Control.instance.Getlean(1) + "\nvalues : " + Control.instance.GetValues(1);
        string lane2set = "Lane2_partsNumber : " + Control.instance.Getlean(2) + "\nvalues : " + Control.instance.GetValues(2);
        string lane3set = "Lane3_partsNumber : " + Control.instance.Getlean(3) + "\nvalues : " + Control.instance.GetValues(3);
        string lane4set = "Lane4_partsNumber : " + Control.instance.Getlean(4) + "\nvalues : " + Control.instance.GetValues(4);

        string path;

#if UNITY_EDITOR
            path = Application.dataPath + "/" + fileName + ".txt";

#else
        path = Directory.GetCurrentDirectory() + "/" + fileName + ".txt";

#endif

        /*
        //path = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "/motion.txt";
        path = Directory.GetCurrentDirectory() + "/motion.txt";
        */

        File.WriteAllText(path, lane1set + "\n\n" + lane2set + "\n\n" + lane3set + "\n\n" + lane4set);
    }

    public void TextReset()
    {
        inputField.text = null;
    }
}