using Inventory;
using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Inventory.Model
{
    public class ChestInteraction : MonoBehaviour
    {
        [SerializeField]
        private InventoryController playerIventory;

        public InventoryController ChestIsInteracing { private set; get; }

        [field: SerializeField]
        public AudioSource AudioSFX { get; set; }

        private int itemIndexBeingDrag = -1;
       

        public void SelectChest(InventoryController chest)
        {
            ChestIsInteracing = chest;
            RepareChest();
            RepareInventory();

          
        }
        private void RepareChest()
        {
            ChestIsInteracing.OnSendItemToInvenotry += HandleSendItemToInventory;
            ChestIsInteracing.OnTransferItemToChest += HandleTransferItemToChest;
            ChestIsInteracing.OnSetItemIndexBeingDrag += HandleItemIndexBeingDrag;
        }
        private void RepareInventory()
        {
            playerIventory.OnSendItemToChest += HandleSendItemToChest;
            playerIventory.OnTransferItemToEventory += HandleTransferItemToInventory;
            playerIventory.OnSetItemIndexBeingDrag += HandleItemIndexBeingDrag;
        }
        private void HandleItemIndexBeingDrag(int itemIndex)
        {
            itemIndexBeingDrag = itemIndex;
        }

        private void HandleTransferItemToChest(int itemIndex)
        {
            InventoryItem item = playerIventory.GetComponent<InventoryController>().InventoryData.GetItemAt(itemIndexBeingDrag);
            InventoryItem itemChanged = ChestIsInteracing.GetComponent<InventoryController>().InventoryData.ChangeItemAt(itemIndex, item);
            playerIventory.GetComponent<InventoryController>().InventoryData.ChangeItemAt(itemIndexBeingDrag, itemChanged);
            AudioSFX.Play();
        }
        private void HandleTransferItemToInventory(int itemIndex)
        {
            InventoryItem item = ChestIsInteracing.GetComponent<InventoryController>().InventoryData.GetItemAt(itemIndexBeingDrag);
            InventoryItem itemChanged = playerIventory.GetComponent<InventoryController>().InventoryData.ChangeItemAt(itemIndex, item);
            ChestIsInteracing.GetComponent<InventoryController>().InventoryData.ChangeItemAt(itemIndexBeingDrag, itemChanged);
            AudioSFX.Play();
        }

        private void HandleSendItemToChest(int itemIndex)
        {
            InventoryItem item = playerIventory.InventoryData.GetItemAt(itemIndex);
            ChestIsInteracing.InventoryData.AddItem(item);
            playerIventory.InventoryData.RemoveItem(itemIndex, item.quantity);
            AudioSFX.Play();
        }
        private void HandleSendItemToInventory(int itemIndex)
        {
            InventoryItem item = ChestIsInteracing.InventoryData.GetItemAt(itemIndex);
            playerIventory.InventoryData.AddItem(item);
            ChestIsInteracing.InventoryData.RemoveItem(itemIndex, item.quantity);
            AudioSFX.Play();
        }
        public void DeSelectChest()
        {
            ChestIsInteracing.OnSendItemToInvenotry -= HandleSendItemToInventory;
            ChestIsInteracing.OnTransferItemToChest -= HandleTransferItemToChest;
            ChestIsInteracing.OnSetItemIndexBeingDrag -= HandleItemIndexBeingDrag;
            playerIventory.OnSendItemToChest -= HandleSendItemToChest;
            playerIventory.OnTransferItemToEventory -= HandleTransferItemToInventory;
            playerIventory.OnSetItemIndexBeingDrag -= HandleItemIndexBeingDrag;
            ChestIsInteracing = null;
        }
    }
}