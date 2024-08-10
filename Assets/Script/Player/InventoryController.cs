using Inventory.Model;
using Inventory.UI;
using System;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField]
        private UIInventoryPage inventoryUI;

        [SerializeField]
        private UITaskBar uiTaskBar;

     
        [field: SerializeField]
        public InventorySO InventoryData { private set; get; }

        public Owner owner;
        public List<InventoryItem> initialItems = new();  
        public event Action<int> OnSendItemToInvenotry, OnSendItemToChest, OnTransferItemToChest, OnTransferItemToEventory, OnSetItemIndexBeingDrag;
        public event Action OnUnSetItemDrag;

        private Canvas canvas;
        private Vector2 dirToMouse;
        private float distanceToMouse;

        private bool isMovingInventory = false;
        private void Start()
        {
            PrepareUI();
            PrepareInventoryData();
            uiTaskBar.OnMenuBeingDrag += HandleBeingDrag;
            uiTaskBar.OnMenuEndDrag += HandleEndDrag;
            canvas = owner == Owner.Inventory ? inventoryUI.transform.root.GetComponent<Canvas>() : inventoryUI.transform.root.GetComponentInChildren<Canvas>();
        }

        
        private void PrepareInventoryData()
        {
            InventoryData.Initialize();
            InventoryData.OnInventoryUpdated += UpdateInventoryUI;
            foreach (InventoryItem item in initialItems)
            {
                if (item.IsEmpty)
                    continue;
                InventoryData.AddItem(item);
            }
        }

        private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            inventoryUI.ResetAllItems();
            foreach (var item in inventoryState)
            {
                inventoryUI.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity);
            }
        }

        private void PrepareUI()
        {
            inventoryUI.InitializeInventoryUI(InventoryData.Size);
            inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
            inventoryUI.OnSwapItem += HandleSwapItems;
            inventoryUI.OnStartDragging += HandleStartDragging;
            inventoryUI.OnItemActionRequested += HandlItemAcionRequestl;
            inventoryUI.OnSelectItemRequest += HandleSelectItem;
            inventoryUI.OnUnsetItem += HandleUpSetItem;
            inventoryUI.OnTransferItem += HandleTransferItem;
            
        }

        private void HandleTransferItem(int itemIndex)
        {
            if(owner == Owner.Chest)
            {
                OnTransferItemToChest?.Invoke(itemIndex);
            }
            else
            {
      
                OnTransferItemToEventory?.Invoke(itemIndex);
            }

        }

        private void HandleUpSetItem()
        {
            OnUnSetItemDrag?.Invoke();
        }

        private void HandleSelectItem(int itemIndex)
        {
            InventoryItem inventoryItem = InventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
            {
                inventoryUI.ResetSelection();
                return;
            }
            inventoryUI.SetlectItem(itemIndex);

        }

        private void HandlItemAcionRequestl(int itemIndex)
        {
            
            InventoryItem inventoryItem = InventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;
            if (owner == Owner.Inventory) 
            {
                if (GetComponent<ChestInteraction>().ChestIsInteracing != null) { 
                    OnSendItemToChest?.Invoke(itemIndex);
                }
                else
                {
                    IItemAction itemAction = inventoryItem.item as IItemAction;
                    itemAction?.PerformAction(gameObject, null);
                    if (inventoryItem.item is IDestroyableItem)
                    {
                        InventoryData.RemoveItem(itemIndex, 1);
                    }
                }
                return;
            }
            if (owner == Owner.Chest)
            {
                OnSendItemToInvenotry?.Invoke(itemIndex); 
            }

        }

        private void HandleStartDragging(int itemIndex)
        {
            InventoryItem inventoryItem = InventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;
            inventoryUI.CreateDraggedItem(inventoryItem.item.ItemImage, inventoryItem.quantity);
            OnSetItemIndexBeingDrag?.Invoke(itemIndex);
        }

        private void HandleSwapItems(int itemIndex1, int itemIndex2)
        {
            InventoryData.SwapItems(itemIndex1, itemIndex2);
        }

        private void HandleDescriptionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = InventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;
            ItemSO item = inventoryItem.item;
            string description = PrepareDescription(inventoryItem);
            inventoryUI.UpdateDesription(item.name, item.ItemImage, description);

        }
        private string PrepareDescription(InventoryItem inventoryItem)
        {
            StringBuilder stringBuilder = new();
            stringBuilder.Append(inventoryItem.item.Description);
            stringBuilder.AppendLine();
            for (int i = 0; i < inventoryItem.itemState.Count; i++)
            {
                stringBuilder.Append($"{inventoryItem.itemState[i].itemparameter.ParameterName}" +
                    $":{inventoryItem.itemState[i].value} / {inventoryItem.item.DefaultParametersList[i].value}");
                stringBuilder.AppendLine();
            }
            return stringBuilder.ToString();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I) && owner == Owner.Inventory)
            {
                if (inventoryUI.isActiveAndEnabled == false)
                {
                    ShowInventory();
                    UpdateData();
                }
                else
                {
                    HideInventory();
                }
            }
            if(isMovingInventory)
            {
               RectTransformUtility.ScreenPointToLocalPointInRectangle(
                   (RectTransform)canvas.transform,
                   Input.mousePosition,
                   canvas.worldCamera,
                   out Vector2 position
                );
                inventoryUI.GetComponent<RectTransform>().anchoredPosition = position-dirToMouse*distanceToMouse;
            }
        }
        private void HandleEndDrag(RectTransform menu)
        {
            isMovingInventory = false;
        }

        private void HandleBeingDrag(RectTransform menu)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
               (RectTransform)canvas.transform,
               Input.mousePosition,
               canvas.worldCamera,
               out Vector2 position
               );
            distanceToMouse = Vector2.Distance(position, inventoryUI.GetComponent<RectTransform>().anchoredPosition);
            dirToMouse = (position - inventoryUI.GetComponent<RectTransform>().anchoredPosition).normalized;
            isMovingInventory = true;
        }
        public void ShowInventory()
        {
            inventoryUI.Show();
        }
        public void HideInventory()
        {
            inventoryUI.Hide();
        }
        public void UpdateData()
        {
            foreach (var item in InventoryData.GetCurrentInventoryState())
            {
                inventoryUI.UpdateData(
                    item.Key,
                    item.Value.item.ItemImage,
                    item.Value.quantity
                    );
            }
        }
    }
}
public enum Owner
{
    
    Chest,
    Inventory,
    Nobody
}