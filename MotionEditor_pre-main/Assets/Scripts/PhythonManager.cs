using UnityEngine;
using System.IO;
using System.Diagnostics;

public class PythonManager : FileAbstract
{
    public override void OnClick()
    {
        base.OnClick();

        string inputDir = Application.dataPath + "/Output";
        string path = Path.Combine(inputDir, fileName);

        ProcessStartInfo psi = new ProcessStartInfo();

        // Python.exe のフルパス（Anaconda でも公式でも可）
        psi.FileName = @"C:\Users\kamiy\.conda\envs\esp32\python.exe";

        // Python スクリプトのフルパスと引数"C:\Users\send_motion1.ipynb"
        psi.Arguments = $"\"C:\\Users\\test.py\" \"{path}\"";

        psi.UseShellExecute = false;
        psi.RedirectStandardOutput = true;
        psi.RedirectStandardError = true;
        psi.CreateNoWindow = true;

        Process p = Process.Start(psi);

        string output = p.StandardOutput.ReadToEnd();
        string error = p.StandardError.ReadToEnd();

        p.WaitForExit();

        UnityEngine.Debug.Log("Python Output:\n" + output);

        if (!string.IsNullOrEmpty(error))
        {
            UnityEngine.Debug.LogError("Python Error:\n" + error);
        }
    }
}