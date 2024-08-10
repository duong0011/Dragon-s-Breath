using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu(menuName ="My Game/EdibleItemSO")]
    public class EdibleItem : ItemSO, IDestroyableItem, IItemAction
    {
        [SerializeField]
        private List<ModifierData> modifierData = new();
        public string ActionName => "Consume";

        public AudioClip ActionSFX {get; private set;}

        public bool PerformAction(GameObject player, List<ItemParameter> itemState = null)
        {
            foreach ( ModifierData data in modifierData)
            {
                data.statModifier.AffectPlayer(player, data.value);
            }
            return true; 
        }
    }
    public interface IDestroyableItem
    {

    }

    public interface IItemAction
    {
        public string ActionName { get; }
        public AudioClip ActionSFX { get; }
        bool PerformAction(GameObject player, List<ItemParameter> itemState);
    }
    [Serializable]
    public class ModifierData
    {
        public PlayerStatsModifierSO statModifier;
        public float value;
    }
}