using UnityEngine;

public class Reset : MonoBehaviour
{
    public Playback playback;
    public Redline redline;
    public LED led;
    public Motors RightMoterScript;
    public Motors LeftMoterScript;

    public void Awake()
    {
        if (playback == null) Debug.Log("Restsスクリプトでplaybackがnull");
        if (redline == null) Debug.Log("Restsスクリプトでredlineがnull");
        if (led == null) Debug.Log("Restsスクリプトでledがnull");
    }

    public void ResetAdachi()
    {
        if (playback.GetModelState() == ModelState.Restart || playback.GetModelState() == ModelState.Play)
        {
            playback.Initialize();
            redline.ResetRedline();
            led.TurnOff();
            RightMoterScript.CurrentAngle = 0f;
            LeftMoterScript.CurrentAngle = 0f;
            RightMoterScript.initialize();
            LeftMoterScript.initialize();
        }
    }
}
