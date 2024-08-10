using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


namespace Inventory.UI
{
    public class UIInventoryPage : MonoBehaviour
    {
        [SerializeField]
        private UIInventoryItem itemPrefab;

        [SerializeField]
        private RectTransform contentPanel;

        [SerializeField]
        private UIInventoryDescription itemDescription;

        [SerializeField]
        private MouseFollower mouseFllower;

        private readonly List<UIInventoryItem> listOfUIItems = new();

        public event Action<int> OnDescriptionRequested,
            OnItemActionRequested, OnStartDragging, OnSelectItemRequest, OnTransferItem;
        public event Action<int, int> OnSwapItem;
        public event Action OnUnsetItem;

        private int currentlyItemDraggedItemIndex = -1;
       // private GameObject currentItem;
        public int CurrentItemSelcted { private set; get; }
        private void Awake()
        {
            Hide();
            itemDescription.ResetDesription();
            mouseFllower.Toggle(false);
        }
        public void InitializeInventoryUI(int inventorySize)
        {
            for (int i = 0; i < inventorySize; i++)
            {
                UIInventoryItem uiItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
                uiItem.transform.SetParent(contentPanel, false);
                uiItem.transform.localScale = Vector3.one;

                // Assuming you need the item to be positioned within the content panel, adjust if needed
                // Set the local position relative to the parent
                uiItem.transform.localPosition = new Vector3(uiItem.transform.localPosition.x, uiItem.transform.localPosition.y, 0);
                listOfUIItems.Add(uiItem);
                uiItem.OnItemClick += HandleItemSlection;
                uiItem.OnItemBeingDrag += HandleBeingDrag;
                uiItem.OnItemDroppedOn += HandleSwap;
                uiItem.OnItemEndDrag += HandleEndDrag;
                uiItem.OnRightMouseBtnClick += HandleShowItemActions;
                uiItem.OnMouseEnter += HandleMouseEnter;
                uiItem.OnMouseExit += HandleMouseExit;
            }
        }


        public void UpdateData(int itemIndex,
            Sprite itemSprite, int itemQuantity)
        {
            if (listOfUIItems.Count > itemIndex)
            {
                listOfUIItems[itemIndex].SetData(itemSprite, itemQuantity);
            }
        }

        private void HandleMouseExit(UIInventoryItem inventoryItemUI)
        {
            itemDescription.gameObject.SetActive(false);
        }
        private void HandleMouseEnter(UIInventoryItem inventoryItemUI)
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1)
                return;
            itemDescription.gameObject.SetActive(true);
            Vector3[] corners1 = new Vector3[4];
            inventoryItemUI.GetComponent<RectTransform>().GetWorldCorners(corners1);
            Vector3 bottomRight1 = corners1[3];
            Vector3[] corners2 = new Vector3[4];
            itemDescription.GetComponent<RectTransform>().GetWorldCorners(corners2);
            Vector3 topLeft2 = corners2[1];
            Vector3 offset = new(bottomRight1.x - topLeft2.x, bottomRight1.y - topLeft2.y, 0);
            itemDescription.GetComponent<RectTransform>().position += offset;
            OnDescriptionRequested?.Invoke(index);

        }
        private void HandleShowItemActions(UIInventoryItem inventoryItemUI)
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1)
                return;
            OnItemActionRequested?.Invoke(index);
        }
        private void HandleEndDrag(UIInventoryItem inventoryItemUI)
        {
            ResetDraggedItem();
            OnUnsetItem?.Invoke();
        }
        private void HandleSwap(UIInventoryItem inventoryItemUI)
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if(index == -1 ) return;
            if (currentlyItemDraggedItemIndex == -1 && index != -1)
            {
                OnTransferItem?.Invoke(index);
                return;
            }
            OnSwapItem?.Invoke(currentlyItemDraggedItemIndex, index);

        }

        private void ResetDraggedItem()
        {
            mouseFllower.Toggle(false);
            currentlyItemDraggedItemIndex = -1;
        }

        private void HandleBeingDrag(UIInventoryItem inventoryItemUI)
        {
           
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1)
                return;
            currentlyItemDraggedItemIndex = index;
            HandleItemSlection(inventoryItemUI);
            OnStartDragging?.Invoke(index);
        }

        public void CreateDraggedItem(Sprite sprite, int quantyti)
        {
            mouseFllower.Toggle(true);
            mouseFllower.SetData(sprite, quantyti);
        }

        private void HandleItemSlection(UIInventoryItem inventoryItemUI)
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1)
                return;
            OnSelectItemRequest?.Invoke(index);

        }

        public void Show()
        {
            gameObject.SetActive(true);
            ResetSelection();
        }

        public void ResetSelection()
        {
            itemDescription.ResetDesription();
            DeselectAllItems();
        }
        private void DeselectAllItems()
        {
            foreach (UIInventoryItem item in listOfUIItems)
            {
                item.Deselect();
            }
        }
        public void Hide()
        {
            gameObject.SetActive(false);
            itemDescription.gameObject.SetActive(false);
            ResetDraggedItem();
        }

        public void UpdateDesription(string name, Sprite itemImage, string description)
        {
            itemDescription.SetDescription(name, itemImage, description);
        }

        public void SetlectItem(int itemIndex)
        {
            DeselectAllItems();
            listOfUIItems[itemIndex].Select();
            CurrentItemSelcted = itemIndex;
        }

        internal void ResetAllItems()
        {
            foreach (var item in listOfUIItems)
            {
                item.ResetData();
                item.Deselect();
            }
        }
    }
}