using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Pygmy.UI
{
    /// <summary>
    /// ドラッグ アンド ドロップ オブジェクト
    /// </summary>
    public class DragAndDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        /// <summary>
        /// ドラッグ開始イベント
        /// </summary>
        [System.Serializable]
        public class OnBeginDrag : UnityEvent { }

        /// <summary>
        /// ドラッグ中イベント
        /// </summary>
        [System.Serializable]
        public class OnDrag : UnityEvent { }

        /// <summary>
        /// ドラッグ終了イベント
        /// </summary>
        [System.Serializable]
        public class OnEndDrag : UnityEvent { }

        [SerializeField, Header("縦方向のドラッグをチェックする")]
        private bool m_isCheckStartVertical;
        public bool isCheckStartVertical
        {
            get { return m_isCheckStartVertical; }
            set { m_isCheckStartVertical = value; }
        }

        [SerializeField, Header("横方向のドラッグをチェックする")]
        private bool m_isCheckStartHorizontal;
        public bool isCheckStartHorizontal
        {
            get { return m_isCheckStartHorizontal; }
            set { m_isCheckStartHorizontal = value; }
        }

        /// <summary>
        /// UI Event 
        /// </summary>
        [SerializeField, Header("ドラッグ開始")]
        private readonly OnBeginDrag m_OnBeginDrag = new OnBeginDrag();
        public OnBeginDrag onBeginDrag
        {
            get { return m_OnBeginDrag; }
        }

        [SerializeField, Header("ドラッグ中")]
        private readonly OnDrag m_OnDrag = new OnDrag();
        public OnDrag onDrag
        {
            get { return m_OnDrag; }
        }

        [SerializeField, Header("ドラッグ終了")]
        private readonly OnEndDrag m_OnEndDrag = new OnEndDrag();
        public OnEndDrag onEndDrag
        {
            get { return m_OnEndDrag; }
        }

        /// <summary>
        /// スクロール中か？
        /// </summary>
        private bool m_isScroll;
        public bool isScroll
        {
            get { return m_isScroll; }
            set { m_isScroll = value; }
        }


        /// <summary>
        /// Active確認
        /// </summary>
        private bool m_IsActive;
        public bool isActive
        {
            get { return m_IsActive; }
            set { m_IsActive = value; }
        }

        /// <summary>
        /// ドラッグ操作中か？
        /// </summary>
        private bool m_IsDragging;
        public bool isDragging
        {
            get { return m_IsDragging; }
            set { m_IsDragging = value; }
        }

        /// <summary>
        /// RootのＴｒａｎｓｆｏｒｍ
        /// </summary>
        private RectTransform m_RootRectTransform;
        public RectTransform rootRectTransform
        {
            get { return m_RootRectTransform ?? (m_RootRectTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>()); }
        }

        /// <summary>
        /// 親のScrollRect
        /// </summary>
        private ScrollRect m_ParentScrollRect;
        public ScrollRect parentScrollRect
        {
            get { return m_ParentScrollRect ?? (m_ParentScrollRect = GetComponentInParent<ScrollRect>()); }
        }

        /// <summary>
        /// ドラッグ開始時の親を保持
        /// </summary>
        private Transform m_PrevParent;
        public Transform prevParent
        {
            get { return m_PrevParent ?? (m_PrevParent = transform.parent); }
        }

        /// <summary>
        /// ドラッグ処理開始時の座標保持
        /// </summary>
        private Vector3 prevPosition;

        /// <summary>
        /// セットアップ
        /// </summary>
        public void Setup(bool _isCheckStartVertical, bool _isCheckStartHorizontal)
        {
            isCheckStartVertical = _isCheckStartVertical;
            isCheckStartHorizontal = _isCheckStartHorizontal;
            isDragging = false;
            isScroll = false;
            isActive = true;
        }

        /// <summary>
        /// リセット
        /// </summary>
        private void Reset()
        {
            gameObject.transform.SetParent(prevParent);
            gameObject.transform.localPosition = prevPosition;

            isDragging = false;
            isScroll = false;
        }

        /// <summary>
        /// 縦ドラッグ操作を確認します
        /// </summary>
        private bool IsVerticalDrag(PointerEventData _event)
        {
            if (Mathf.Abs(_event.delta.x) < Mathf.Abs(_event.delta.y))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 横ドラッグ操作を確認します
        /// </summary>
        private bool IsHorizontalDrag(PointerEventData _event)
        {
            if (Mathf.Abs(_event.delta.x) > Mathf.Abs(_event.delta.y))
            {
                return true;
            }
            return false;
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            // 縦横どちらも検知
            if (isCheckStartVertical && isCheckStartHorizontal)
            {
                if (IsVerticalDrag(eventData) || IsHorizontalDrag(eventData))
                {
                    isDragging = true;
                }
            }
            // 縦だけ検知
            else if (isCheckStartVertical)
            {
                if (IsVerticalDrag(eventData) && !IsHorizontalDrag(eventData))
                {
                    isDragging = true;
                }
            }
            // 横だけ検知
            else if (isCheckStartVertical)
            {
                if (!IsVerticalDrag(eventData) && IsHorizontalDrag(eventData))
                {
                    isDragging = true;
                }
            }

            if (isDragging)
            {
                this.gameObject.transform.SetParent(rootRectTransform);
                onBeginDrag.Invoke();
            }
            else
            {
                // 親のスクロールビューを操作。
                if (parentScrollRect != null)
                {
                    isScroll = true;
                    parentScrollRect.OnBeginDrag(eventData);
                }
            }
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            if (isDragging)
            {
                prevPosition = transform.localPosition;

                var tmpPosition = Vector2.zero;

                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    rootRectTransform,
                    eventData.position,
                    eventData.enterEventCamera, out tmpPosition);

                transform.localPosition = tmpPosition;

                onDrag.Invoke();
            }
            else
            {
                // 親のスクロールビューを操作。
                if (parentScrollRect != null)
                {
                    parentScrollRect.OnDrag(eventData);
                }
            }
        }


        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            if (isDragging)
            {
                onEndDrag.Invoke();
            }
            else
            {
                // 親のスクロールビューを操作。
                if (parentScrollRect != null)
                {
                    isScroll = false;
                    parentScrollRect.OnEndDrag(eventData);
                }
            }
            Reset();
        }
    }
}
