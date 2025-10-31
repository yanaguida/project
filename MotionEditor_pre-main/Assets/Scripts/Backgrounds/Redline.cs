using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Redline : MonoBehaviour
{
    private GameObject redlineGO;
    private RectTransform redline;
    private Vector2 prevPos;
    private float speed = ValueBox.GetDis();

    void Awake()
    {
        redlineGO = this.gameObject;
        redline = redlineGO.GetComponent<RectTransform>();
        prevPos = redline.anchoredPosition;
        redlineGO.SetActive(false);
    }

    public IEnumerator StartRedline(){
        redlineGO.SetActive(true);
        while (true){
            redline.anchoredPosition += Vector2.right * speed * Time.deltaTime;
            yield return null;
        }
    }

    public void ResetRedline(Coroutine redlineCO){
        redlineGO.SetActive(false);
        StopCoroutine(redlineCO);
        redline.anchoredPosition = prevPos;
    }
}
