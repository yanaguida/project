using UnityEngine;
using System.Collections;

public class SwitchOn : MonoBehaviour
{
    public Renderer targetRenderer;         // LEDのRenderer
    public Color smileColor = Color.red; // 発光色
    public Color sadColor = Color.blue;
    public Color winkColor = Color.green;
    public float intensity = 2f;            // 発光強度

    private Material mat;

    void Start()
    {
        mat = targetRenderer.material;
        TurnOff(); // 初期状態：OFF
    }

    public IEnumerator Toggle(float desiredtime, string emotion)
    {
        TurnOn(emotion);
        yield return new WaitForSeconds(desiredtime);
        TurnOff();
    }

    private void TurnOn(string emotion)
    {
        mat.EnableKeyword("_EMISSION");
        if(emotion == "Smile")
        mat.SetColor("_EmissionColor", smileColor * intensity);
        if(emotion == "Sad")
        mat.SetColor("_EmissionColor", sadColor * intensity);
        if(emotion == "Wink")
        mat.SetColor("_EmissionColor", winkColor * intensity);
    }

    private void TurnOff()
    {
        mat.DisableKeyword("_EMISSION");
        mat.SetColor("_EmissionColor", Color.clear); 
    }
}
