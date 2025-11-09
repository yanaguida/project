using UnityEngine;
using System.Collections;

public class LED : MonoBehaviour, Ifunc
{
    [SerializeField] private PartType parttype;
    [SerializeField] private Renderer targetRenderer;         // LEDのRenderer
    [SerializeField] private Color smileColor = Color.red; // 発光色
    [SerializeField] private Color sadColor = Color.blue;
    [SerializeField] private Color winkColor = Color.green;
    [SerializeField] private float intensity = 2f;            // 発光強度
    private Material mat;

    public PartType CorrespondPart() => parttype;

    void Start()
    {
        mat = targetRenderer.material;
        TurnOff(); // 初期状態：OFF
    }

    public IEnumerator Action(float start, float desiredtime, float emotion)
    {
        yield return new WaitForSeconds(start);
        TurnOn(emotion);
        yield return new WaitForSeconds(desiredtime);
        TurnOff();
    }

    private void TurnOn(float emotion)
    {
        mat.EnableKeyword("_EMISSION");
        if (emotion == 0)
            mat.SetColor("_EmissionColor", smileColor * intensity);
        if (emotion == 1)
            mat.SetColor("_EmissionColor", sadColor * intensity);
        if (emotion == 2)
            mat.SetColor("_EmissionColor", winkColor * intensity);
    }

    public void TurnOff()
    {
        mat.DisableKeyword("_EMISSION");
        mat.SetColor("_EmissionColor", Color.clear);
    }
}
