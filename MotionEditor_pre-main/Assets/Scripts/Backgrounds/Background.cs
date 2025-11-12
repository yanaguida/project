using UnityEngine;

public class Background : MonoBehaviour
{
    public RectTransform bg_meta;
    public RectTransform bg_emo;
    public RectTransform bg_eva;
    public RectTransform bg_mira;
    public ScrollView scrollview;
    private Vector2 meta_pos, emo_pos, eva_pos, mira_pos;
    private int i = 0;

    void Start()
    {
        emo_pos = bg_emo.anchoredPosition;
        meta_pos = bg_meta.anchoredPosition;
        eva_pos = bg_eva.anchoredPosition;
        mira_pos = bg_mira.anchoredPosition;
    }

    public void OnClick()
    {
        if (i % 4 == 0)
        {
            bg_meta.anchoredPosition = mira_pos;
            bg_emo.anchoredPosition = meta_pos;
            bg_eva.anchoredPosition = emo_pos;
            bg_mira.anchoredPosition = eva_pos;
            scrollview.RawImageActivate(false);
        }
        else if (i % 4 == 1)
        {
            bg_meta.anchoredPosition = eva_pos;
            bg_emo.anchoredPosition = mira_pos;
            bg_eva.anchoredPosition = meta_pos;
            bg_mira.anchoredPosition = emo_pos;
        }
        else if (i % 4 == 2)
        {
            bg_meta.anchoredPosition = emo_pos;
            bg_emo.anchoredPosition = eva_pos;
            bg_eva.anchoredPosition = mira_pos;
            bg_mira.anchoredPosition = meta_pos;
            scrollview.SetMetaImage();
        }
        else if (i % 4 == 3)
        {
            bg_meta.anchoredPosition = meta_pos;
            bg_emo.anchoredPosition = emo_pos;
            bg_eva.anchoredPosition = eva_pos;
            bg_mira.anchoredPosition = mira_pos;
            scrollview.RawImageActivate(true);
        }
        i++;
    }
}
