using UnityEngine;
using UnityEngine.UI;

public class ChengeBack : MonoBehaviour
{
    public Texture newTexture1; //切り替えて表示したい画像
    public Texture firstImage; //元の画像
    private RawImage image;
    private DandD_scroll parent;

    void Start()
    {
        //RawImageコンポーネントをこのオブジェクトから取得
        image = GetComponent<RawImage>();

        //親であるモーションパーツから、スクリプトのコンポーネントを取得
        parent = GetComponentInParent<DandD_scroll>() ;
    }

    void Update()
    {

        //選択されているパーツが親であるなら
        if (Control.instance.GetChoiceParts() == parent.num)
        {
            // 画像を切り替える
            image.texture = newTexture1;
        }
        else
        {
            // 画像を切り替える
            image.texture = firstImage;
        }
    }
}
