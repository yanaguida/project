using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//a
//a

public class DandD_up : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{

    public Vector2 prevPos; //保存しておく初期position
    private Vector2 lane1Pos;//レーンに添わせる用
    private Vector2 PosX;//レーンに入っている要素分ずらす用

    private RectTransform rectTransform; // 移動したいオブジェクトのRectTransform
    private RectTransform parentRectTransform; // 移動したいオブジェクトの親のRectTransform

    public int num;//←←←partNによって変更する、オブジェクト側で入力可能
    public int drag = 0;//ドラッグされているか判定

    public ScrollRect scrollRect;//スクロールの量を取る用



    public virtual void Awake()
    {
        rectTransform = GetComponent<RectTransform>(); //このオブジェクトのrectTransform
        parentRectTransform = rectTransform.parent as RectTransform; //このオブジェクトの親のrectTransform

        prevPos = rectTransform.anchoredPosition; //初期位置を保存
        PosX = new Vector2(0.0f, 0.0f);
        PosX.x = gameObject.GetComponent<RectTransform>().sizeDelta.x; //このオブジェクトのx幅を保存

        scrollRect = GameObject.Find("Scroll View").GetComponent<ScrollRect>(); //スクロール部分のコンポーネントを取る
    }


    // ドラッグ開始時の処理
    public void OnBeginDrag(PointerEventData eventData)
    {
        // ドラッグ開始時にドラッグ変数を1(ドラッグ中)に変更
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

        Vector2 lane1;
        Vector2 lane2;
        Vector2 lane3;
        Vector2 lane4;

        //レーンごとに範囲を指定
        //scrollRect.verticalNormalizedPosition（どれだけスクロールされているか、範囲：0～1）をかけることで
        //初期状態で見えているレーン1・2のスクロールによる判定変更処理を行う
        //初期状態で隠れているレーン3・4は、一つ上のレーンの判定から1レーンの幅だけずらした範囲と定義する

        lane1.y = -437.5f * scrollRect.verticalNormalizedPosition;
        lane2.y = -750.0f * scrollRect.verticalNormalizedPosition;
        lane3.y = (lane2.y - 312.5f);
        lane4.y = (lane3.y - 312.5f);

        //レーン1
        if (rectTransform.anchoredPosition.y <= -75.0f && rectTransform.anchoredPosition.y >= lane1.y)
        {

            //レーン1が見えない範囲までスクロールされているなら
            if (scrollRect.verticalNormalizedPosition < 0.52)
            {
                ResetParts(); //リセット
            }
            else
            {
                //二回目でなければ
                if (Control.instance.Checklean1(num) == 0)
                {
                    //リスト1に追加
                    Control.instance.Addlean1(num);
                }
                if (Control.instance.Checklean2(num) == 1)
                {
                    //今までlane2にあった場合削除
                    Control.instance.Removelean2(num);
                }
                if (Control.instance.Checklean3(num) == 1)
                {
                    //今までlane3にあった場合削除
                    Control.instance.Removelean3(num);
                }
                if (Control.instance.Checklean4(num) == 1)
                {
                    //今までlane4にあった場合削除
                    Control.instance.Removelean4(num);
                }
                //場所変更
                ChangePosition();

            }
        }

        //以下、同様にレーン2～4及び外側

        //レーン2
        else if (rectTransform.anchoredPosition.y <= lane1.y && rectTransform.anchoredPosition.y >= lane2.y)
        {

            if (scrollRect.verticalNormalizedPosition < 0.1)
            {
                ResetParts();
            }
            else
            {
                if (Control.instance.Checklean2(num) == 0)
                {
                    Control.instance.Addlean2(num);
                }
                if (Control.instance.Checklean1(num) == 1)
                {
                    Control.instance.Removelean1(num);
                }
                if (Control.instance.Checklean3(num) == 1)
                {
                    Control.instance.Removelean3(num);
                }
                if (Control.instance.Checklean4(num) == 1)
                {
                    Control.instance.Removelean4(num);
                }
                ChangePosition();
            }
        }
        //レーン3
        else if (rectTransform.anchoredPosition.y <= lane2.y && rectTransform.anchoredPosition.y >= lane3.y)
        {
            if (scrollRect.verticalNormalizedPosition > 0.85)
            {
                ResetParts();
            }
            else
            {
                if (Control.instance.Checklean3(num) == 0)
                {
                    Control.instance.Addlean3(num);
                }
                if (Control.instance.Checklean1(num) == 1)
                {
                    Control.instance.Removelean1(num);
                }
                if (Control.instance.Checklean2(num) == 1)
                {
                    Control.instance.Removelean2(num);
                }
                if (Control.instance.Checklean4(num) == 1)
                {
                    Control.instance.Removelean4(num);
                }
                ChangePosition();
            }
        }
        //レーン4
        else if (rectTransform.anchoredPosition.y <= lane3.y && rectTransform.anchoredPosition.y >= lane4.y)
        {
            if (scrollRect.verticalNormalizedPosition > 0.4)
            {
                ResetParts();
            }
            else
            {
                if (Control.instance.Checklean4(num) == 0)
                {
                    Control.instance.Addlean4(num);
                }
                if (Control.instance.Checklean1(num) == 1)
                {
                    Control.instance.Removelean1(num);
                }
                if (Control.instance.Checklean2(num) == 1)
                {
                    Control.instance.Removelean2(num);
                }
                if (Control.instance.Checklean3(num) == 1)
                {
                    Control.instance.Removelean3(num);
                }
                ChangePosition();
            }
        }
        //外側
        else
        {
            ResetParts();
        }

        //最後にドラッグ判定変数を0にする
        drag = 0;
    }


    //リセットする
    void ResetParts()
    {
        if (Control.instance.Checklean1(num) == 1)
        {
            //今までlane1にあった場合削除
            Control.instance.Removelean1(num);
        }
        if (Control.instance.Checklean2(num) == 1)
        {
            //今までlane2にあった場合削除
            Control.instance.Removelean2(num);
        }
        if (Control.instance.Checklean3(num) == 1)
        {
            //今までlane3にあった場合削除
            Control.instance.Removelean3(num);
        }
        if (Control.instance.Checklean4(num) == 1)
        {
            //今までlane4にあった場合削除
            Control.instance.Removelean4(num);
        }

        //rectTransformを初期位置に戻す
        rectTransform.anchoredPosition = prevPos;
    }


    // ドロップ終了時の処理
    public void OnDrop(PointerEventData eventData)
    {
        //なにもなし
    }

    // ScreenPositionからlocalPositionへの変換関数
    public Vector2 GetLocalPosition(Vector2 screenPosition)
    {
        Vector2 result = Vector2.zero;

        // screenPositionを親の座標系(parentRectTransform)に対応するよう変換する.
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, screenPosition, Camera.main, out result);

        return result;
    }

    //オブジェクトの位置を変更する
    private void ChangePosition()
    {
        //最初のパーツが置かれるxの値
        lane1Pos.x = -1063.0f;

        //レーン1の配列にこのオブジェクトの番号が含まれるならば
        if (Control.instance.Checklean1(num) != 0)
        {
            //画面外へ飛ばす
            lane1Pos.y = 3000.0f;

            //そのレーンに既に置かれているパーツの分だけずらす
            for (int i = 0; i < Control.instance.Countlean1(num); i++)
            {
                lane1Pos.x = lane1Pos.x + PosX.x;
            }
        }
        //以下同様にレーン2
        else if (Control.instance.Checklean2(num) != 0)
        {
            lane1Pos.y = 3000.0f;
            for (int i = 0; i < Control.instance.Countlean2(num); i++)
            {
                lane1Pos.x = lane1Pos.x + PosX.x;
            }
        }
        //レーン3
        else if (Control.instance.Checklean3(num) != 0)
        {
            lane1Pos.y = 3000.0f;
            for (int i = 0; i < Control.instance.Countlean3(num); i++)
            {
                lane1Pos.x = lane1Pos.x + PosX.x;
            }
        }
        //レーン4
        else if (Control.instance.Checklean4(num) != 0)
        {
            lane1Pos.y = 3000.0f;
            for (int i = 0; i < Control.instance.Countlean4(num); i++)
            {
                lane1Pos.x = lane1Pos.x + PosX.x;
            }
        }
        //外側
        else
        {
            lane1Pos = prevPos;
        }
        //オブジェクトの位置を変更
        rectTransform.anchoredPosition = lane1Pos;
    }

    public void ClickInput()
    {
        Control.instance.SetCPfromClick(num);
    }

    public void Update()
    {
        //ドラッグされていないなら常にChangePosition()を実行
        //他のパーツが移動した際に即座に詰めるため
        if (drag == 0)
            ChangePosition();

    }

}
