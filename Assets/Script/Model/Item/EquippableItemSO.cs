using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu(menuName ="My Game/EquippableItem")]
    public class EquippableItemSO : ItemSO, IItemAction, IDestroyableItem
    {
        public string ActionName => "Equip";

        public AudioClip ActionSFX { get; private set; }

        public bool PerformAction(GameObject player, List<ItemParameter> itemState)
        {
            throw new System.NotImplementedException();
        }

        public List<ItemStatInfo> itemStatInfo = new();
        public Dictionary<ItemStat, float> itemStats = new();

        private void OnEnable()
        {
            Initialize();
        }
        public void Initialize()
        {
            foreach (var ItemStatInfo in itemStatInfo)
            {
                itemStats.Add(ItemStatInfo.statType, ItemStatInfo.statValue);
            }
        }
        public void UpdateStat()
        {
            for (int i = 0; i < itemStatInfo.Count; i++)
            {
                itemStatInfo[i] = new ItemStatInfo(itemStatInfo[i].statType, itemStats[itemStatInfo[i].statType]); 
            }
           
        }
        public float GetStat(ItemStat stat)
        {
            if (itemStats.TryGetValue(stat, out float value))
            {
                return value;
            }
            Debug.LogError($"No stat value found for {stat} ont {this.name}");
            return 0;

        }
        public float ChangeStat(ItemStat stat, float amount)
        {
            if (!itemStats.TryGetValue(stat, out _))
            {
                return -1f;
            }
            itemStats[stat] = amount;
            return itemStats[stat];
        }
    }
}
public enum ItemStat
{
    LevelUpgrade,
    Agi,
    Strength,
    Inteligen,
    PhysicDamage,
    MagicDamage,
    HP,
    Speed,
    Mana,
    Arrmor,
    MagicResictance,
    Critical,
    CriticalResitance,
    AllStat
    
}


[System.Serializable]
public class ItemStatInfo
{
    public ItemStat statType;
    public float statValue;
    public ItemStatInfo(ItemStat statType, float statValue)
    {
        this.statType = statType;
        this.statValue = statValue;
    }
}

