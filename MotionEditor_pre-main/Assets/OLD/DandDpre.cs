using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class DandDpre : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{

    public Vector2 prevPos; //保存しておく初期position
    private Vector2 lane1Pos;//レーンに添わせる用
    private Vector2 PosX;//レーンに入っている要素分ずらす用
    private RectTransform rectTransform; // 移動したいオブジェクトのRectTransform
    private RectTransform parentRectTransform; // 移動したいオブジェクトの親(Panel)のRectTransform
    private RectTransform lane1Transform;//レーンのRectTransForm
    //public int whereObject = 0;//今オブジェクトがどこにあるか
    public int num = 33;//←←←partNによって変更する
    public int drag = 0;
    //private Control array;

    

    public virtual void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentRectTransform = rectTransform.parent as RectTransform;
        GameObject lane1pic = GameObject.Find("lane1");
        lane1Transform = lane1pic.GetComponent<RectTransform>();
        //レーン１の画像から座標抽出
        prevPos = rectTransform.anchoredPosition;
        PosX = new Vector2(0.0f, 0.0f);
        PosX.x = gameObject.GetComponent<RectTransform>().sizeDelta.x;
    }


    // ドラッグ開始時の処理
    public void OnBeginDrag(PointerEventData eventData)
    {
        // ドラッグ前の位置を記憶しておく
        // RectTransformの場合はpositionではなくanchoredPositionを使う
        drag = 1;

    }

    // ドラッグ中の処理
    public void OnDrag(PointerEventData eventData)
    {
        // eventData.positionから、親に従うlocalPositionへの変換を行う
        // オブジェクトの位置をlocalPositionに変更する

        Vector2 localPosition = GetLocalPosition(eventData.position);
        rectTransform.anchoredPosition = localPosition;
    }

    // ドラッグ終了時の処理
    public void OnEndDrag(PointerEventData eventData)
    {
        if (rectTransform.anchoredPosition.y <= lane1Transform.anchoredPosition.y && rectTransform.anchoredPosition.y >= (lane1Transform.anchoredPosition.y - lane1Transform.sizeDelta.y))//レーン1
        {
            if (Control.instance.Checklean1(num) == 0)//二回目でなければ
            {
                Control.instance.Addlean1(num);//リスト1に追加
            }
            if (Control.instance.Checklean2(num) == 1)
            {
                Control.instance.Removelean2(num);
            }//今までlane2にあった場合削除
            ChangePosition();
            //rectTransform.anchoredPosition = lane1Pos;
            //Debug.Log(whereObject);

        }
        else if (rectTransform.anchoredPosition.y <= -437.5f && rectTransform.anchoredPosition.y >= -750.0f)//レーン2
        {
            /*lane1Pos = rectTransform.anchoredPosition;
            PosX.x = gameObject.GetComponent<RectTransform>().sizeDelta.x;
            lane1Pos.x = -1030.0f;*/
            if (Control.instance.Checklean2(num) == 0)//二回目でなければ
            {
                Control.instance.Addlean2(num);
            }
            if (Control.instance.Checklean1(num) == 1)
            {
                Control.instance.Removelean1(num);
            }//今までlane1にあった場合削除
            ChangePosition();
            //Debug.Log(whereObject);

        }
        else//外側
        {

            if (Control.instance.Checklean1(num) == 1)
            {
                Control.instance.Removelean1(num);
            }//今までlane1にあった場合削除
            if (Control.instance.Checklean2(num) == 1)
            {
                Control.instance.Removelean2(num);
            }//今までlane2にあった場合削除
            rectTransform.anchoredPosition = prevPos;
            //Debug.Log(whereObject);
        }
        drag = 0;
    }

    // ドロップ終了時の処理
    public void OnDrop(PointerEventData eventData)
    {

    }

    // ScreenPositionからlocalPositionへの変換関数
    public Vector2 GetLocalPosition(Vector2 screenPosition)
    {
        Vector2 result = Vector2.zero;

        // screenPositionを親の座標系(parentRectTransform)に対応するよう変換する.
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, screenPosition, Camera.main, out result);

        return result;
    }

    public void ChangePosition()
    {
        //lane1Pos = rectTransform.anchoredPosition;
        lane1Pos.x = -1063.0f;

        if (Control.instance.Checklean1(num) != 0)
        {
            lane1Pos.y = lane1Transform.anchoredPosition.y;
            Debug.Log(Control.instance.Countlean1(num));
            for (int i = 0; i < Control.instance.Countlean1(num); i++)
            {
                lane1Pos.x = lane1Pos.x + PosX.x;
            }
        }
        else if (Control.instance.Checklean2(num) != 0)
        {
            lane1Pos.y = -580.0f;
            for (int i = 0; i < Control.instance.Countlean2(num); i++)
            {
                lane1Pos.x = lane1Pos.x + PosX.x;
            }
        }
        else
        {
            lane1Pos = prevPos;
        }
        rectTransform.anchoredPosition = lane1Pos;
    }

    public void Update()
    {
        if (drag == 0)
            ChangePosition();
    }

}
