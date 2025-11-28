using UnityEngine;
using System.Collections;

public class Redline : MonoBehaviour
{
    private GameObject redlineGO;
    private RectTransform redline;
    private Vector2 prevPos;
    private float speed = ValueBox.GetDis();
    float time = 0;

    void Awake()
    {
        redlineGO = this.gameObject;
        redline = redlineGO.GetComponent<RectTransform>();
        prevPos = redline.anchoredPosition;
        redlineGO.SetActive(false);
    }

    public IEnumerator StartRedline(float d)
    {
        redlineGO.SetActive(true);
        while (d > time)
        {
            redline.anchoredPosition += Vector2.right * speed * Time.deltaTime;
            time += Time.deltaTime;
            yield return null;
        }
        ResetRedline();
    }

    public void ResetRedline()
    {
        redlineGO.SetActive(false);
        time = 0;
        redline.anchoredPosition = prevPos;
    }
}
