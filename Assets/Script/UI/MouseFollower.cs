using Inventory.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    
    private Canvas canvas;

    private UIInventoryItem item;

    private void Awake()
    {
        canvas = transform.root.GetComponent<Canvas>();
        item = GetComponentInChildren<UIInventoryItem>();
    }
    public void SetData(Sprite sprite, int quantity)
    {
        item.SetData(sprite, quantity);
    }
    private void Update()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            Input.mousePosition,
            canvas.worldCamera,
            out Vector2 position
            );
        transform.position = canvas.transform.TransformPoint(position);
    }
    public void Toggle(bool val)
    {
        if (gameObject.activeSelf == false && val == false) return;
        gameObject.SetActive(val);
    }
}
