using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Inventory.Model
{
    public abstract class ItemSO : ScriptableObject
    {
        [field: SerializeField]
        public bool IsStackable { get; set; }

        public int ID => GetInstanceID();

        [field: SerializeField]
        public int MaxStackSize { get; set; }
        [field: SerializeField]
        public string Name { get; set; }

        [field: SerializeField]
        [field: TextArea(3, 10)]
        public string Description { get; set; }
        [field: SerializeField]
        public Sprite ItemImage { get; set; }
        [field: SerializeField]
        public int ItemLevel { get; set; }

        [field: SerializeField]
        public List<ItemParameter> DefaultParametersList { get; set; }    
     
    }
}
[Serializable]
public struct ItemParameter : IEquatable<ItemParameter>
{
    public ItemParameterSO itemparameter;
    public float value;

    public readonly bool Equals(ItemParameter other)
    {
        return other.itemparameter == itemparameter;
    }
}

