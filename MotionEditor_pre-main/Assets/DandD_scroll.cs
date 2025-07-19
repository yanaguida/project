using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//a
public class DandD_scroll : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
{

    public Vector2 prevPos; //保存しておく初期position
    private Vector2 lane1Pos;//レーンに添わせる用
    private Vector2 PosX;//レーンに入っている要素分ずらす用

    private RectTransform rectTransform; // 移動したいオブジェクトのRectTransform
    private RectTransform parentRectTransform; // 移動したいオブジェクトの親(Panel)のRectTransform

    public int num; //←←←partNによって変更する、オブジェクト側で入力可能
    public int drag = 0; //ドラッグされているか判定
    public int clicks = 0; //クリックされた回数



    public virtual void Awake()
    {
        rectTransform = GetComponent<RectTransform>(); //このオブジェクトのrectTransform
        parentRectTransform = rectTransform.parent as RectTransform; //このオブジェクトの親のrectTransform

        prevPos = rectTransform.anchoredPosition; //初期位置を保存
        PosX = new Vector2(0.0f, 0.0f);
        PosX.x = gameObject.GetComponent<RectTransform>().sizeDelta.x; //このオブジェクトのx幅を保存
    }

    //クリックされたときの処理
    public void OnPointerClick(PointerEventData eventData)
    {
        //クリック回数を1増やす
        clicks++;

        //ドラッグ中のクリックでないなら
        if (drag == 0)
        {
            //クリック回数が奇数なら
            //SetChoicePartsにこのオブジェクトの番号の変数をセット
            if ((clicks % 2) == 1)
            {
                Control.instance.SetChoiceParts(num);
            }

            //クリック回数が偶数なら
            //SetChoicePartsを0にする
            else if ((clicks % 2) != 1)
            {
                Control.instance.SetChoiceParts(0);
            }
        }
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
        //レーン1
        if (rectTransform.anchoredPosition.y <= -75.0f && rectTransform.anchoredPosition.y >= -437.5f)
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

        //以下、同様にレーン2～4及び外側

        //レーン2
        else if (rectTransform.anchoredPosition.y <= -437.5f && rectTransform.anchoredPosition.y >= -750.0f)
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
        //レーン3
        else if (rectTransform.anchoredPosition.y <= -750.0f && rectTransform.anchoredPosition.y >= -1000.5f)
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
        //レーン4
        else if (rectTransform.anchoredPosition.y <= -1000.5f && rectTransform.anchoredPosition.y >= -1375.0f)
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
        else//外側
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
        
        //リセットされるときに選択も解除
        Control.instance.SetChoiceParts(0);
    }


    // ドロップ終了時の処理
    public void OnDrop(PointerEventData eventData)
    {
        //なにもなし
    }

    // ScreenPositionからlocalPositionへの変換関数
    public Vector2 GetLocalPosition(Vector2 screenPosition)
{
    Vector2 result; // ← これが必要
    RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, screenPosition, Camera.main, out result);

    return result;
}


    //オブジェクトの位置を変更
   /* private void ChangePosition()
    {
        //最初のパーツが置かれるxの値
        lane1Pos.x = -1058.0f;

        //レーン1の配列にこのオブジェクトの番号が含まれるならば
        if (Control.instance.Checklean1(num) != 0)
        {
            //レーン1の位置
            lane1Pos.y = -323.0f;

            //そのレーンに既に置かれているパーツの分だけずらす
            for (int i = 0; i < Control.instance.Countlean1(num); i++)
            {
                lane1Pos.x = lane1Pos.x + PosX.x;
            }
        }
        //以下同様にレーン2
        else if (Control.instance.Checklean2(num) != 0)
        {
            lane1Pos.y = -580.0f;
            for (int i = 0; i < Control.instance.Countlean2(num); i++)
            {
                lane1Pos.x = lane1Pos.x + PosX.x;
            }
        }
        //レーン3
        else if (Control.instance.Checklean3(num) != 0)
        {
            lane1Pos.y = -837.0f;
            for (int i = 0; i < Control.instance.Countlean3(num); i++)
            {
                lane1Pos.x = lane1Pos.x + PosX.x;
            }
        }
        //レーン4
        else if (Control.instance.Checklean4(num) != 0)
        {
            lane1Pos.y = -1094.0f;
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
*/
// オブジェクトの位置を変更
private void ChangePosition()
{
    float step = 120.0f; // 吸着単位

    Vector2 currentPos = rectTransform.anchoredPosition;
    lane1Pos.x = Mathf.Round(currentPos.x / step) * step;

    // Y軸をレーンごとにスナップ
    if (Control.instance.Checklean1(num) != 0)
    {
        lane1Pos.y = -323.0f;
    }
    else if (Control.instance.Checklean2(num) != 0)
    {
        lane1Pos.y = -580.0f;
    }
    else if (Control.instance.Checklean3(num) != 0)
    {
        lane1Pos.y = -837.0f;
    }
    else if (Control.instance.Checklean4(num) != 0)
    {
        lane1Pos.y = -1094.0f;
    }
    else
    {
        // 外側は元の位置へ
        lane1Pos = prevPos;
    }

    rectTransform.anchoredPosition = lane1Pos;
}

    public void Update()
    {

        //ドラッグされていないなら常にChangePosition()を実行
        //他のパーツが移動した際に即座に詰めるため
        if (drag == 0)
            ChangePosition();

        //別のパーツが選択された際はクリック回数をリセットする
        if (Control.instance.GetChoiceParts() != num)
            clicks = 0;

        //マウスの座標を直接参照してレーン外を判定
        //外側ならリセット
        Vector3 mousePos = Input.mousePosition;
        if ((drag == 1) && (mousePos.y > 600.0f))
        {
            ResetParts();
        }
    }



}
