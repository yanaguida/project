using UnityEngine;
using UnityEngine.UI;

public class Example : MonoBehaviour
{
    public Texture newSprite;
    public Texture firstImage;
    private RawImage image;
    public int num = 1;

    void Start()
    {
        // SpriteRendererコンポーネントを取得します
        image = GetComponent<RawImage>();
    }

    void Update()
    {
        //firstImage.texture = image.texture;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            num= num+1;
        }

        if ((num % 2) == 1)
        {
            // 画像を切り替えます
            image.texture = newSprite;
        }
        else
        {
            // 画像を切り替えます
            image.texture = firstImage;
        }
    }
}
