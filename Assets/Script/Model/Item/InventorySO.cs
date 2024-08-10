using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu(menuName = "My Game/ InventorySO")]
    public class InventorySO : ScriptableObject
    {
        [SerializeField]
        private List<InventoryItem> inventoryItems;
        [field: SerializeField]
        public int Size { get; private set; } = 10;

        public event Action<Dictionary<int, InventoryItem>> OnInventoryUpdated;
        public void Initialize()
        {
            inventoryItems = new();
            for (int i = 0; i < Size; i++)
            {
                inventoryItems.Add(InventoryItem.GetEmptyItem());
            }
        }

        public int AddItem(ItemSO item, int quantity, List<ItemParameter> itemState = null)
        {
            if(item.IsStackable == false)
            {
                while (quantity > 0 && IsInventoryFull() == false)
                {
                    quantity -= AddItemToFirstFreeSlot(item, 1, itemState);
                }
                InformAboutChange();
                return quantity;
            }
            quantity = AddStackableItem(item, quantity);
            InformAboutChange();
            return quantity; 
            
        }

        private int AddItemToFirstFreeSlot(ItemSO item, int quantity, List<ItemParameter> itemState = null)
        {
            InventoryItem newItem = new()
            {
                item = item,
                quantity = quantity,
                itemState =
                    new(itemState == null ? item.DefaultParametersList : itemState)

            };
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty)
                {
                    inventoryItems[i]= newItem;
                    return quantity;
                }
            }
            return 0;
        }

        private bool IsInventoryFull() 
            => inventoryItems.Where(item => item.IsEmpty).Any() == false;

        private int AddStackableItem(ItemSO item, int quantity)
        {
            
            for (int i = 0;i < inventoryItems.Count;i++)
            {
                
                if (inventoryItems[i].IsEmpty)
                {
                    continue;
                }
                if (inventoryItems[i].item.ID == item.ID)
                {
                   
                    int amountPossibleTotake = inventoryItems[i].item.MaxStackSize - inventoryItems[i].quantity;
                    if(quantity > amountPossibleTotake)
                    {
                        inventoryItems[i] = inventoryItems[i]
                            .ChangeQuantity(inventoryItems[i].item.MaxStackSize);
                        quantity -= amountPossibleTotake;
                    }
                    else
                    {
                        inventoryItems[i] = inventoryItems[i]
                            .ChangeQuantity(inventoryItems[i].quantity + quantity);
                        InformAboutChange();
                        return 0;
                    }
                }
            }
           
            while (quantity > 0 && !IsInventoryFull())
            {
                int newQuantity = Mathf.Clamp(quantity, 0, item.MaxStackSize);
                
                quantity -= newQuantity;
                AddItemToFirstFreeSlot(item, newQuantity);
            }
            return quantity;
        }

        public void AddItem(InventoryItem item)
        {   
            AddItem(item.item, item.quantity);
        }
        public Dictionary<int, InventoryItem> GetCurrentInventoryState()
        {
            Dictionary<int, InventoryItem> returnValue = new();
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty) continue;
                returnValue[i] = inventoryItems[i];
            }
            return returnValue;
        }

        public InventoryItem GetItemAt(int itemIndex)
        {
            return inventoryItems[itemIndex];
        }

        public void SwapItems(int itemIndex1, int itemIndex2)
        {
            (inventoryItems[itemIndex2], inventoryItems[itemIndex1]) = (inventoryItems[itemIndex1], inventoryItems[itemIndex2]);
            InformAboutChange();
        }

        private void InformAboutChange()
        {
            OnInventoryUpdated?.Invoke(GetCurrentInventoryState());
        }
        public InventoryItem ChangeItemAt(int itemIndex, InventoryItem item)
        {
            InventoryItem tmp = inventoryItems[itemIndex];
            inventoryItems[itemIndex] = item;
            InformAboutChange();
            return tmp;
        }

        public void RemoveItem(int itemIndex, int amount)
        {
            if(inventoryItems.Count > itemIndex)
            {
                if (inventoryItems[itemIndex].IsEmpty)
                {
                    return;
                }
                int reminder = inventoryItems[itemIndex].quantity - amount;
                if(reminder <= 0)
                {
                    inventoryItems[itemIndex] = InventoryItem.GetEmptyItem();
                }
                else
                {
                    inventoryItems[itemIndex] = inventoryItems[itemIndex]
                        .ChangeQuantity(reminder);
                }
                InformAboutChange();
            }
        }
    }
    [Serializable]
    public struct InventoryItem
    {
        public int quantity;
        public ItemSO item;
        public List<ItemParameter> itemState;
        public readonly bool IsEmpty => item == null;

        public InventoryItem ChangeQuantity(int newQuantity)
        {
            return new InventoryItem
            {
                item = this.item,
                quantity = newQuantity,
                itemState = new(this.itemState)
            };
        }

        public static InventoryItem GetEmptyItem()
            => new()
            {
                item = null,
                quantity = 0,
                itemState = new()
            };
    }

}
