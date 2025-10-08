using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Memory : MonoBehaviour
{
    public GameObject zeroGO;
    public GameObject oneGO;
    private RectTransform zero;
    private RectTransform one;
    private RectTransform clonert;
    private Vector2 memorypos;
    private TextMeshProUGUI memorytext;
    private const int memorynum = 20;
    private int step = 5;
    private float dif;

    void Start(){
        initialization();
        StartCoroutine(SpawnMemory());
    }

    private void initialization(){
        if(zeroGO!=null&&oneGO!=null){
            zero = zeroGO.GetComponent<RectTransform>();
            one = oneGO.GetComponent<RectTransform>();
        }
        else{
            Debug.Log("0sまたは1sのGOの取得に失敗");
        }

        dif = Mathf.Abs(zero.anchoredPosition.x-one.anchoredPosition.x);

        memorypos.x = one.anchoredPosition.x+dif;
        memorypos.y = one.anchoredPosition.y;
    }

    private IEnumerator SpawnMemory(){
        for(int i=0;i<memorynum;i++){
            GameObject clone = Instantiate(oneGO, this.transform);
            clonert = clone.GetComponent<RectTransform>();
            memorytext = clone.GetComponent<TextMeshProUGUI>();
            clonert.anchoredPosition = memorypos;
            step += int.Parse(memorytext.text);
            memorytext.text = step.ToString(); 
            memorypos.x += dif; 
            yield return null; 
            }
    }
}
