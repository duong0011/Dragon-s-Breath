using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[CreateAssetMenu(menuName ="My Game/ Stats")]
public class Stats : ScriptableObject
{
    public string nameObj;
    public string descriptionObj;
   
    public List<StatInfo> statsInfo = new ();
    public Dictionary<Stat, float> stats = new();


    [System.NonSerialized]
    public UnityEvent<int> healthChangeEvent;
    [System.NonSerialized]
    public UnityEvent<float> expChangeEvent;
    [NonSerialized]
    public UnityEvent<float> levelUpEvent;

    public event Action OnRestoreFullHealth;

    private void OnEnable()
    {
        expChangeEvent ??= new UnityEvent<float>();
        levelUpEvent ??= new UnityEvent<float>();
        Initialize();
    }
    public void Initialize()
    {
        foreach (var stat in statsInfo)
        {
            stats.Add(stat.statType, stat.statValue);
        }
    }
    public float GetStat(Stat stat)
    {
        if( stats.TryGetValue(stat, out float value))
        {
            return value; 
        }
        Debug.LogError($"No stat value found for {stat} ont {this.name}");
        return 0f;
        
    }
    
    public float ChangeStat(Stat stat, float amount)
    {
        if (stats.TryGetValue(stat, out _))
        {
            stats[stat] = amount;
            return stats[stat];
        }
        return -1f;
    }
    //Add experience to Dictionary not to ScriptableObj
    public void AddExperience(float exp)
    {
        float curExp = GetStat(Stat.CurExp);
        float requireExp = GetStat(Stat.BaseExperience) * (float)Math.Pow(GetStat(Stat.Level), 1.55f);
        ChangeStat(Stat.CurExp, curExp + exp);
        while(GetStat(Stat.CurExp) >= requireExp)
        {
            float curLevel = GetStat(Stat.Level);
            ChangeStat(Stat.Level, curLevel + 1f);
            curExp = GetStat(Stat.CurExp);
            ChangeStat(Stat.CurExp, curExp - requireExp);
            levelUpEvent.Invoke(GetStat(Stat.Level));
            OnRestoreFullHealth?.Invoke();
        }
        float curPercentExp = GetStat(Stat.CurExp) / (GetStat(Stat.BaseExperience) * (float)Math.Pow(GetStat(Stat.Level), 1.55f));
        //Debug.Log($"You just get {exp} exp, require {requireExp} to level up");
        expChangeEvent.Invoke(curPercentExp);
    }
}
[System.Serializable]
public class StatInfo
{
    public Stat statType;
    public float statValue;
}

public enum Stat
{
    Level,
    RespawnTime,
    MaxHealth,
    Damage,
    Armor,
    MagicResistance,
    AttackCooldown,
    DistanceAttack,
    DistanceChase,
    StopDistance,
    DistanceBack,
    CheckObjInRadius,
    BaseExperience,
    CurExp,
    ExperienceWhenDie,
}
