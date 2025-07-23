using UnityEngine;
using UnityEngine.UI;

public class Images : MonoBehaviour
{
    public Texture newTexture1;
    public Texture newTexture2;
    public Texture newTexture3;
    public Texture newTexture4;
    public Texture newTexture5;
    public Texture newTexture6; //切り替えたい画像6枚
    public Texture firstImage; //最初の画像
    private RawImage image;

    void Start()
    {
        // このオブジェクトからRawImageコンポーネントを取得
        image = GetComponent<RawImage>();
    }

    void Update()
    {
        
        //選択されているパーツによって分岐
        if ((Control.instance.GetChoiceParts() == 1) || (Control.instance.GetChoiceParts() == 11) || (Control.instance.GetChoiceParts() == 12))
        {
            // 画像を切り替える
            image.texture = newTexture1;
        }
        else if ((Control.instance.GetChoiceParts() == 2) || (Control.instance.GetChoiceParts() == 21) || (Control.instance.GetChoiceParts() == 22))
        {
            // 画像を切り替える
            image.texture = newTexture2;
        }
        else if ((Control.instance.GetChoiceParts() == 3) || (Control.instance.GetChoiceParts() == 31) || (Control.instance.GetChoiceParts() == 32))
        {
            // 画像を切り替える
            image.texture = newTexture3;
        }
        else if ((Control.instance.GetChoiceParts() == 4) || (Control.instance.GetChoiceParts() == 41) || (Control.instance.GetChoiceParts() == 42))
        {
            // 画像を切り替える
            image.texture = newTexture4;
        }
        else if ((Control.instance.GetChoiceParts() == 5) || (Control.instance.GetChoiceParts() == 51) || (Control.instance.GetChoiceParts() == 52))
        {
            // 画像を切り替える
            image.texture = newTexture5;
        }
        else if ((Control.instance.GetChoiceParts() == 6) || (Control.instance.GetChoiceParts() == 61) || (Control.instance.GetChoiceParts() == 62))
        {
            // 画像を切り替える
            image.texture = newTexture6;
        }
        else
        {
            // 画像を切り替える
            image.texture = firstImage;
        }
    }
}
