using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITaskBar : MonoBehaviour
{
    public RectTransform menu;

    public event Action<RectTransform> OnMenuBeingDrag, OnMenuEndDrag;

    public void OnBeingDrag()
    {
 
        OnMenuBeingDrag?.Invoke(menu);
    }
    public void OnEndDrag()
    {
        OnMenuEndDrag?.Invoke(menu);
    }
}
