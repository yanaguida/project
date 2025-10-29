using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour
{
    private RectTransform rectTransform;
    public RectTransform bg_meta;
    public RectTransform bg_emo;
    private Vector2 meta_pos,emo_pos;
    private int i=0;

    void Start(){
        //bg_emo = rectTransform.Find("background_emo") as RectTransform;
        //bg_meta = rectTransform.Find("background_meta") as RectTransform;
        emo_pos = bg_emo.anchoredPosition;
        meta_pos = bg_meta.anchoredPosition;
    }

    public void OnClick(){
        if(i%2==0){
            bg_meta.anchoredPosition = emo_pos;
            bg_emo.anchoredPosition = meta_pos;
        }
        else{
            bg_meta.anchoredPosition = meta_pos;
            bg_emo.anchoredPosition = emo_pos;
        }
        i++;
    }
}
