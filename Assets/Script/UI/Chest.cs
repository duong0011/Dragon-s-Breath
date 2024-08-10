using Inventory;
using Inventory.Model;
using UnityEngine;

public class Chest : MonoBehaviour
{
    
    private InventoryController chestUI;

    private void Awake()
    {
        chestUI = GetComponent<InventoryController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ShowChestUI();
            collision.GetComponent<ChestInteraction>().SelectChest(GetComponent<InventoryController>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HideChestUI();
            collision.GetComponent<ChestInteraction>().DeSelectChest();
        }
    }

    private void ShowChestUI()
    {
        if (chestUI != null)
        {
            chestUI.ShowInventory();
            chestUI.UpdateData();
        }
    }

    private void HideChestUI()
    {
        if (chestUI != null)
        {
            chestUI.HideInventory();
        }
    }
}
