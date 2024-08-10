using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="My Game/Enemy/DropList")]
public class EnemyDropItemSO : ScriptableObject
{
    public List<ItemDrop> Items = new();
}
[Serializable]
public struct ItemDrop
{
    public Item item;
    public int maxQuantityDrop;

}
