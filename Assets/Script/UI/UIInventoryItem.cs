using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class UIInventoryItem : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler,
        IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private Image itemImage;
        [SerializeField]
        private TMP_Text quantityTxt;
        [SerializeField]
        private Image border;
        [SerializeField]
        private Image background;

        //public int id;
        public event Action<UIInventoryItem> OnItemClick, OnItemDroppedOn, OnItemBeingDrag, OnItemEndDrag, OnRightMouseBtnClick, OnMouseEnter, OnMouseExit;

        private bool empty = true;

        private void Awake()
        {
            ResetData();
            Deselect();
        }

        public void ResetData()
        {
            itemImage.gameObject.SetActive(false);
            background.gameObject.SetActive(false);
            empty = true;
        }

        public void Deselect()
        {
            border.gameObject.SetActive(false);
        }
        public void SetData(Sprite sprite, int quantity)
        {
            itemImage.gameObject.SetActive(true);
            background.gameObject.SetActive(true);
            itemImage.sprite = sprite;
            quantityTxt.text = quantity + "";
            empty = false;
        }

        public void Select()
        {
            border.gameObject.SetActive(true);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                OnRightMouseBtnClick?.Invoke(this);
            }
            else
            {
                OnItemClick?.Invoke(this);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (empty)
            {
                return;
            }

            OnItemBeingDrag?.Invoke(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnItemEndDrag?.Invoke(this);
        }

        public void OnDrag(PointerEventData eventData)
        {

        }

        public void OnDrop(PointerEventData eventData)
        {
            OnItemDroppedOn?.Invoke(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (empty)
                return;
            OnMouseEnter?.Invoke(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnMouseExit?.Invoke(this);
        }
    }
}