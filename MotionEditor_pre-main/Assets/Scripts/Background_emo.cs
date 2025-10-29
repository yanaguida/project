using UnityEngine;
using UnityEngine.UI;

public class UIColorMultiShift : MonoBehaviour
{
    [Header("各色の遷移にかける時間（秒）")]
    public float transitionDuration = 2f;

    [Header("ループさせるか")]
    public bool loop = true;

    private RawImage rawImage;
    private float timer = 0f;
    private int currentIndex = 0;

    private Color[] colorSequence;

    void Start()
    {
        rawImage = GetComponent<RawImage>();

        // 🎨 カラーシーケンス（灰→寒色4→暖色4）
        colorSequence = new Color[]
        {
            // ⚪️ スタート：中間の灰色
            new Color(0.5f, 0.5f, 0.5f, 1f),

            // ❄️ 寒色（青・水・青緑系）
            new Color(0.2f, 0.5f, 1f, 1f),   // 寒色①：青
            new Color(0.0f, 0.7f, 0.9f, 1f), // 寒色②：水色
            new Color(0.3f, 0.8f, 0.8f, 1f), // 寒色③：青緑
            new Color(0.6f, 0.9f, 1f, 1f),   // 寒色④：明るい水色（柔らかい寒色）

            // 🔥 暖色（黄・橙・赤系）
            new Color(1f, 0.95f, 0.4f, 1f),  // 暖色①：黄色
            new Color(1f, 0.8f, 0.3f, 1f),   // 暖色②：黄橙
            new Color(1f, 0.55f, 0.2f, 1f),  // 暖色③：オレンジ
            new Color(1f, 0.3f, 0.3f, 1f),   // 暖色④：赤（最も暖かい）
        };

        rawImage.color = colorSequence[0];
    }

    void Update()
    {
        if (colorSequence == null || colorSequence.Length < 2) return;

        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / transitionDuration);

        int nextIndex = currentIndex + 1;

        // 範囲チェック
        if (nextIndex >= colorSequence.Length)
        {
            if (loop)
            {
                nextIndex = 0; // ループ
            }
            else
            {
                return; // 終了
            }
        }

        // 現在の色 → 次の色へ補間
        rawImage.color = Color.Lerp(colorSequence[currentIndex], colorSequence[nextIndex], t);

        // 遷移完了時
        if (t >= 1f)
        {
            timer = 0f;
            currentIndex = nextIndex;
        }
    }
}