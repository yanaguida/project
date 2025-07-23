using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class Moving : MonoBehaviour
{
    public float WhereScroll;
    public ScrollRect scrollRect;


    //スクロール
    void Start()
    {
    scrollRect = GetComponent<ScrollRect>();
    }

    // Update is called once per frame
    void Update()
    {
        //なにもしない
    }

}
